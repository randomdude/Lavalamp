﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using virtualNodeNetwork.PICStuff;

namespace virtualNodeNetwork
{
    /// <summary>
    /// Comminications to the gpSim simulator all goes through this class. It handles all simulator-specific
    /// operations.
    /// </summary>
    public class gpSim : IDisposable
    {
        private static readonly string _gpSimBinary = Properties.Settings.Default.gpSimExecutable;

        // Strings which are sent to or recieved from gpSim
        private const string _promptString = @"**gpsim> ";
        private const string _errorString = @"***error: ";
        private const string _processorListStart = "Processor List\r\n";
        private const string _cmdProcessor = @"processor";
        private const string _breakpointCannotBeSetOnString = @"break cannot be set on";
        private const string _bpHitString = @"message:";
        private const string _runCommandString = "run";
        private const string _runningString = "running...\r\n";
        private const string _emptyProcessorListString = "(empty)";

        /// <summary>
        /// Was a 'run' command the last issued?
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// Our instance of GPSim
        /// </summary>
        private readonly Process _gpSimProcess;

        /// <summary>
        /// Data is placed in this StringBuilder asynch by another thread.
        /// </summary>
        private StringBuilder _stdoutsofar = new StringBuilder();

        /// <summary>
        /// The amount of stdout our main thread has processed
        /// </summary>
        private int _stdoutProcessedCount = 0;

        /// <summary>
        /// Lock this before using the stdoutsofar
        /// </summary>
        private readonly object _stdOutLock = new object();

        /// <summary>
        /// Used for the asynch read only
        /// </summary>
        private readonly byte[] _charInBuf = new byte[1];

        /// <summary>
        /// The type of PIC we should simulate
        /// </summary>
        public chipType chipType;

        /// <summary>
        /// This is called once a breakpoint is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hit"></param>
        public delegate void breakpointHandler(gpSim sender, breakpoint hit);

        /// <summary>
        /// Our breakpoints, keyed by id
        /// </summary>
        private readonly Dictionary<int, breakpoint> _breakpoints = new Dictionary<int, breakpoint>();

        /// <summary>
        /// Fire our Event on this thread.
        /// </summary>
        private readonly ISynchronizeInvoke _eventHandler;

        public gpSim(string filename, ISynchronizeInvoke newEventHandler)
        {
            if (!File.Exists(filename + ".cod") || !File.Exists(filename + ".lst"))
                throw new GPSimException("File not found");

            _eventHandler = newEventHandler;

            // GPSim is (strangely) throwing an AV if it loads a .cod from an absolute path. To work around this, we 
            // set the working directory to the location of the file to load, and load it via a relative path.
            FileInfo fileinfo = new FileInfo(filename);

            // Start up the executable. 
            ProcessStartInfo info = new ProcessStartInfo(_gpSimBinary)
                                        {
                                            Arguments = " -i -S=disable ",
                                            RedirectStandardOutput = true,
                                            RedirectStandardInput = true,
                                            UseShellExecute = false,
                                            WorkingDirectory = fileinfo.DirectoryName
                                        };
            _gpSimProcess = new Process();
            _gpSimProcess.StartInfo = info;
            _gpSimProcess.Start();
            //
            // Because we need to read non-line-buffered output, we dcannot use the OutputDataReceived event
            // of the Process (really). Because of this, we instead perform async reads on the base Stream
            // instead
            //
            // FIXME - won't we miss data from gpsim if it's scheduled to run before our read?
            Thread readThread = new Thread(this.readThread) { Name = "GPSim input polling thread" };
            readThread.Start();

            // Wait for our initial prompt to be returned.
            waitForPrompt();

            // And then load the code file.
            load(fileinfo.Name);
        }

        public void readThread()
        {
            while (true)
            {
                if (_gpSimProcess.StandardOutput.BaseStream.Read(_charInBuf, 0, 1) == 0)
                    throw new GPSimException("gpsim died?");
                gpSimOutputDataRecieved(null);
            }
        }

        /// <summary>
        /// Return a bool indicating if the class can be used - ie, if required binaries are available
        /// </summary>
        /// <returns></returns>
        public static bool isConfiguredCorrectly()
        {
            if (!File.Exists(_gpSimBinary))
                return false;

            return true;
        }

        /// <summary>
        /// Process a raw line of output from our child process, adding it to our string so that the main thread
        /// can process it.
        /// </summary>
        /// <param name="ar"></param>
        private void gpSimOutputDataRecieved(IAsyncResult ar)
        {
            try
            {
                //if (_gpSimProcess.StandardOutput.BaseStream.EndRead(ar) == 0)
                //    return;

                lock (_stdOutLock)
                {

                    if (_stdoutsofar.ToString().ToLower().Contains(_runningString))
                    {
                        _isRunning = true;
                    }

                    // If we see a 'Message:...' line, this indicates that a breakpoint was hit. 
                    if (_stdoutsofar.ToString().ToLower().Contains(_bpHitString) && _isRunning)
                    {
                        //lock (this)
                        {
                            handleBreakpointStop(_stdoutsofar);
                        }
                    }

                    _stdoutsofar.Append((char) _charInBuf[0]);
                }

                // eek stack overflow here eventually?!
                //_gpSimProcess.StandardOutput.BaseStream.BeginRead(_charInBuf, 0, 1, gpSimOutputDataRecieved, this);
            }
            catch (InvalidOperationException)
            {
                // This will happen once our Process has exited. I can't seem to see any way to check for this
                // other than just catching this exception :|
            }
        }

        private void waitForPrompt()
        {
            waitForSpecified(_promptString.ToLower());
        }

        private void waitForSpecified(string toWaitFor)
        {
            while (true)
            {
                lock (_stdOutLock)
                {
                    string soFar = _stdoutsofar.ToString().ToLower();
                    _stdoutProcessedCount = soFar.Length;

                    // Since we are waiting for our prompt, error out on any errors or unexpected
                    // occurances.
                    if (soFar.Contains(_errorString.ToLower()))
                        throw new GPSimException();

                    if (soFar.Contains(_breakpointCannotBeSetOnString.ToLower()))
                        throw new GPSimException();

                    // Otherwise, we should truncate the output and return
                    if (soFar.Contains(toWaitFor.ToLower()))
                    {
                        string trimmed = soFar.Substring( soFar.LastIndexOf(toWaitFor.ToLower()) + toWaitFor.Length );
                        _stdoutProcessedCount = 0;
                        _stdoutsofar = new StringBuilder(trimmed);

                        return;
                    }

                }

                // OK, no exit condition. Wait for more data.
                waitForNewData();
            }
        }

        private string getNextLine()
        {
            while (true)
            {
                lock (_stdOutLock)
                {
                    string stdOut = _stdoutsofar.ToString();
                    _stdoutProcessedCount = stdOut.Length;
                    // Since more than one line may have been returned, we need to split the line and make
                    // sure we only nom up the first.
                    if (stdOut.Contains("\n"))
                    {
                        string[] lines = stdOut.Split(new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

                        if (lines.Length == 0)
                            continue;

                        string foo = _stdoutsofar.ToString().Substring(lines[0].Length + 1);
                        _stdoutsofar = new StringBuilder( foo );

                        _stdoutProcessedCount = 0;
                        return lines[0].TrimEnd(new [] {'\r', '\n' } );
                    }
                }

                waitForNewData();
            }
        }
        
        private void waitForNewData()
        {
            // Wait for some new data to be appended to our stdOut by the process event handler. Make
            // sure we don't miss any by updating out start pos arg with the same value.
            while (true)
            {
                lock (_stdOutLock)
                {
                    if (_stdoutsofar.Length > _stdoutProcessedCount)
                    {
                        _stdoutProcessedCount = _stdoutsofar.Length;
                        return;
                    }
                }

                Thread.Sleep(0);
            }
        }

        public void addWriteBreakpoint(breakpoint bk)
        {
            lock (this)
            {
                writeLine("br w " + bk.location + ", \"" + bk.id + "\"");
                waitForPrompt();

                _breakpoints.Add(bk.id, bk);
            }
        }

        public void addWriteBreakpoint(string locationSymbol, breakpointHandler onByteWritten)
        {
            lock (this)
            {
                breakpoint newBP = new breakpoint()
                                       {
                                           location = locationSymbol,
                                           callback = onByteWritten
                                       };

                writeLine("br w " + locationSymbol + ", \"" + newBP.id + "\"");
                waitForPrompt();

                _breakpoints.Add(newBP.id, newBP);
            }
        }

        /// <summary>
        /// Load a file. Pass a .cod file, or previously specify the processor type.
        /// This will also set the chipType field.
        /// </summary>
        /// <param name="filename"></param>
        private void load(string filename)
        {
            string lineToExec = String.Format("load \"{0}.cod\"", filename );
            writeLine(lineToExec);
            waitForPrompt();

            // Now, ask gpsim for the processor list, and read our processor type from it. If the load
            // fails, gpsim will not report this (!), but the processor list will then be "(empty)".
            writeLine(_cmdProcessor);

            waitForSpecified(_processorListStart);
            // Now get the first element of our processor list.
            string processorLine = getNextLine();
            waitForPrompt();

            if (processorLine == _emptyProcessorListString)
                throw new GPSimException("GPSim failed to load file");

            chipType = chipType.parse(processorLine);
        }

        private void writeLine(string toWrite)
        {
            //Thread.Sleep(10);
            //_gpSimProcess.StandardInput.Flush();
            _gpSimProcess.StandardInput.Write(toWrite);
            _gpSimProcess.StandardInput.Write("\n");
            _gpSimProcess.StandardInput.Flush();
        }

        public void run()
        {
            lock (this)
            {
                writeLine(_runCommandString);
                waitForSpecified(_runningString);
            }
        }

        private void handleBreakpointStop(StringBuilder linesSoFar)
        {
            // We should see one or more 'Message:...' lines, which indicate which breakpoint 
            // was hit, as they return the 'message' we set when we made the breakpoint - so 
            // that'll be our breakpoint ID.
            // We are not guaranteed to have a line ending in \r\n, so omit any partial lines.
            string withoutPartials = linesSoFar.ToString().Substring(0, linesSoFar.ToString().LastIndexOf('\n'));
            if (withoutPartials.Length <= _stdoutProcessedCount)
                return;
            withoutPartials = withoutPartials.Substring(_stdoutProcessedCount);
            int n = 0;
            breakpoint toCall = null;
            foreach (string line in withoutPartials.Split('\n'))
            {
                string lineIn = line.ToLower().Trim(new[] { '\r', '\n', ' ', '\t' });

                if (lineIn.StartsWith("message:"))
                {
                    string idStr = lineIn.Split(':')[1];
                    int id = Convert.ToInt32(idStr);

                    int start = linesSoFar.ToString().ToLower().IndexOf(_bpHitString) + _bpHitString.Length + 2 + idStr.Length;
                    string foo = linesSoFar.ToString().Substring(start);

                    lock (_stdOutLock)   // fuck, too late, should've done this earlier!
                    {
                        if (_stdoutsofar.ToString() != linesSoFar.ToString())
                            throw new Exception();
                        _stdoutsofar = new StringBuilder(foo);
                        _stdoutProcessedCount = 0;
                    }

                    if (_breakpoints.ContainsKey(id))
                        toCall = _breakpoints[id];
                    break;
                }

                n += line.Length + 1;
            }

            if (toCall != null)
            {
                _isRunning = false;
                callBPHooks(toCall);
            }
        }

        private void callBPHooks(breakpoint hitBP)
        {
            _eventHandler.BeginInvoke(_breakpoints[hitBP.id].callback, new object[] {this, hitBP} );
        }

        public void doBreakin()
        {
            lock (this)
            {
//                Process p = Process.Start("C:\\cygwin\\bin\\kill.exe", "-s INT " + _gpSimProcess.Id);
//                p.WaitForExit();

                // Send a CTRL-C via p/invoke.
                //IntPtr eventHandle = CreateEvent(IntPtr.Zero, false, false, "gpsimInterrupt");
                IntPtr eventHandle = OpenEvent(0x001F0003, false, "gpsimInterrupt");
                var err = Marshal.GetLastWin32Error();
                SetEvent(eventHandle);

                //EnumWindows(enumCallback, (IntPtr) 0);
                waitForPrompt();
            }

        }

        private bool enumCallback(IntPtr hwnd, IntPtr lparam)
        {
            StringBuilder className = new StringBuilder(1024);
            GetClassName(hwnd, className, className.Capacity);

            if (className.ToString() != "ConsoleWindowClass")
                return true;

            StringBuilder windowText = new StringBuilder(1024);
            GetWindowText(hwnd, windowText, windowText.Capacity);

            if (!windowText.ToString().ToLower().EndsWith("gpsim.exe"))
                return true;

            // OK we have found our window!
            //int VK_CONTROL = 0x11;
            //uint wm_keydown = 0x100;
            //uint wm_keyup = 0x101;
            //int VK_C = 0x43;

            //PostMessage(hwnd, wm_keydown, VK_CONTROL, 0);       //Send Ctrl down 
            //PostMessage(hwnd, wm_keydown, VK_C, 0);             //Send 'c' down 
            //PostMessage(hwnd, wm_keyup, VK_C, 0);               //Send 'c' up 
            //PostMessage(hwnd, wm_keyup, VK_CONTROL, 0);         //Send Ctrl up

            return false;
        }

        public int readMemory(string symToRead)
        {
            lock (this)
            {
                if (_isRunning)
                    throw new GPSimException();

                // Simply write the symbol name here
                // (in before injection of some form!)
                writeLine(symToRead);

                // Output will be similar to :
                //   **gpsim> pir1
                //   pir1 = 0x10

                int n = 0;
                string[] foo= new string[10];

                while (true)
                {
                    string lineIn = getNextLine();
                    if (lineIn == null)
                        continue;

                    foo[n++] = lineIn;

                    // The prompt, as our string is echoed back to us
                    if (lineIn.Contains(_promptString))
                        continue;

                    // OK, this should be our line.
                    // Split by the equals sign in to the name and the value.
                    string[] chunks = lineIn.Split('=');
                    if (chunks.Length != 2)
                        throw new GPSimException();
                    string hexNum = chunks[1].Trim();

                    // Now lop off the leading 0x from the number
                    if (!hexNum.StartsWith("0x"))
                        throw new GPSimException();
                    hexNum = hexNum.Substring(2);

                    // And parse!
                    return int.Parse(hexNum, NumberStyles.HexNumber);
                }
            }
        }

        public TTypeToReturn readMemory<TTypeToReturn>(string symToRead)
            where TTypeToReturn : sfr, new()
        {
            lock (this)
            {
                int rawToRetrun = readMemory(symToRead);

                sfr toRet = new TTypeToReturn();
                toRet.setUnderlyingValue(rawToRetrun);

                return (TTypeToReturn)toRet;
            }
        }

        public void writeMemory(string symbolName, byte newVal)
        {
            lock (this)
            {
                if (_isRunning)
                    throw new GPSimException();

                // Write the new value via GPSim.
                string lineToSend = String.Format("{0} = {1}", symbolName, newVal);
                writeLine(lineToSend);
                waitForPrompt();

                // Read back to verify this worked.
                if (readMemory(symbolName) != newVal)
                    throw new GPSimException();
            }
        }

        public void writeMemory(string symbolName, int bitToChange, int newBitVal)
        {
            if (newBitVal != 0 && newBitVal != 1)
                throw new ArgumentException();

            // Do a read-modify-write of the specified byte.
            int oldVal = readMemory(symbolName);
            if (newBitVal == 1)
                oldVal |= bitToChange;
            else
                oldVal &= ~bitToChange;

            writeMemory(symbolName, (byte) oldVal);
        }

        public void Dispose()
        {
            try
            {
                try
                {
                    if (!_gpSimProcess.HasExited)
                    {
                        _gpSimProcess.Kill();
                        _gpSimProcess.WaitForExit(1000);
                    }
                }
                catch (InvalidOperationException)
                {
                    // Swallow these. Throwing on the GC thread is much worse than leaking.
                }

                _gpSimProcess.Dispose();
            }
            catch (ObjectDisposedException) { }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hwnd, uint Msg, int wParam, int lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
        [DllImport("kernel32.dll")]
        static extern bool SetEvent(IntPtr hEvent);
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);
    }
}