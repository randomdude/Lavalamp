using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using virtualNodeNetwork.PICStuff;

namespace virtualNodeNetwork
{
    /// <summary>
    /// Comminications to the gpSim simulator all goes through this class. It handles all simulator-specific
    /// operations.
    /// </summary>
    public class gpSim : IDisposable
    {
        /// <summary>
        /// FIXME: Eeeeek, hardcoding is baaad!
        /// </summary>
        private const string gpSimBinary = @"C:\Program Files (x86)\gpsim\bin\gpsim.exe";

        // Strings which are sent to or recieved from gpSim
        private const string promptString = @"**gpsim> ";
        private const string errorString = @"***error: ";
        private const string processorListStart = "Processor List\r\n";
        private const string cmdProcessor = @"processor";
        private const string breakpointCannotBeSetOnString = @"break cannot be set on";
        private const string bpHitString = @"message:";
        private const string runningString = "running...\r\n";
        // The keystroke we use to pause PIC execution
        private static readonly string breakIn = ((char)0x03).ToString();

        /// <summary>
        /// Was a 'run' command the last issued?
        /// </summary>
        private bool isRunning;

        public bool exitTime;

        /// <summary>
        /// Our instance of GPSim
        /// </summary>
        private readonly Process gpSimProcess;

        /// <summary>
        /// Data is placed in this StringBuilder asynch by another thread.
        /// </summary>
        private StringBuilder stdoutsofar = new StringBuilder();

        /// <summary>
        /// The amount of stdout our main thread has processed
        /// </summary>
        private int stdoutProcessedCount = 0;

        /// <summary>
        /// Lock this before using the stdoutsofar
        /// </summary>
        private readonly object stdOutLock = new object();

        /// <summary>
        /// Used for the asynch read only
        /// </summary>
        private readonly byte[] charInBuf = new byte[1];

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
        private readonly Dictionary<int, breakpoint> breakpoints = new Dictionary<int, breakpoint>();

        /// <summary>
        /// Fire our Event on this thread.
        /// </summary>
        private readonly ISynchronizeInvoke _eventHandler;

        public gpSim(string filename, ISynchronizeInvoke newEventHandler)
        {
            _eventHandler = newEventHandler;

            // Start up the executable. 
            ProcessStartInfo info = new ProcessStartInfo(gpSimBinary);
            info.Arguments = " -i ";    // command-line mode
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            gpSimProcess = new Process();
            gpSimProcess.StartInfo = info;
            gpSimProcess.Start();
            //
            // Because we need to read non-line-buffered output, we dcannot use the OutputDataReceived event
            // of the Process (really). Because of this, we instead perform async reads on the base Stream
            // instead
            //
            // FIXME - won't we miss data from gpsim if it's scheduled to run before our read?
            gpSimProcess.StandardOutput.BaseStream.BeginRead(charInBuf, 0, 1, gpSimOutputDataRecieved, this);

            // Wait for our initial prompt to be returned.
            waitForPrompt();

            // and then load the code file.
            load(filename + ".cod");
        }

        private int consumeNextPrompt;

        private StringBuilder promptNommer = new StringBuilder();
        /// <summary>
        /// Process a raw line of output from our child process, adding it to our string so that the main thread
        /// can process it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ar"></param>
        private void gpSimOutputDataRecieved(IAsyncResult ar)
        {
            try
            {
                if (gpSimProcess.StandardOutput.BaseStream.EndRead(ar) == 0)
                    return;

                lock (stdOutLock)
                {

                    if (stdoutsofar.ToString().ToLower().Contains(runningString))
                    {
                        isRunning = true;
                    }

                    // If we see a 'Message:...' line, this indicates that a breakpoint was hit. 
                    if (stdoutsofar.ToString().ToLower().Contains(bpHitString) && isRunning)
                    {
                        //lock (this)
                        {
                            if (handleBreakpointStop(stdoutsofar))
                            {
                                isRunning = false;
                                consumeNextPrompt = 0;
                                promptNommer = new StringBuilder();
                            }
                        }
                    }
                    
                    if (consumeNextPrompt > 0)
                    {
                        promptNommer.Append((char)charInBuf[0]);
                        if (promptNommer.ToString().ToLower().EndsWith(promptString))
                        {
                            consumeNextPrompt--;
                            promptNommer = new StringBuilder();
                            stdoutsofar = new StringBuilder();
                            stdoutProcessedCount = 0;
                        }
                    }
                    else
                    {
                        stdoutsofar.Append((char) charInBuf[0]);
                    }
                }

                // eek stack overflow here eventually?!
                gpSimProcess.StandardOutput.BaseStream.BeginRead(charInBuf, 0, 1, gpSimOutputDataRecieved, this);
            }
            catch (InvalidOperationException)
            {
                // This will happen once our Process has exited. I can't seem to see any way to check for this
                // other than just catching this exception :|
            }
        }

        private void waitForPrompt()
        {
            waitForSpecified(promptString.ToLower());
        }

        private void waitForSpecified(string toWaitFor)
        {
            while (true)
            {
                lock (stdOutLock)
                {
                    string soFar = stdoutsofar.ToString().ToLower();
                    stdoutProcessedCount = soFar.Length;

                    // Since we are waiting for our prompt, error out on any errors or unexpected
                    // occurances.
                    if (soFar.Contains(errorString.ToLower()))
                        throw new GPSimException();

                    if (soFar.Contains(breakpointCannotBeSetOnString.ToLower()))
                        throw new GPSimException();

                    // Otherwise, we should truncate the output and return
                    if (soFar.Contains(toWaitFor.ToLower()))
                    {
                        string trimmed = soFar.Substring( soFar.LastIndexOf(toWaitFor.ToLower()) + toWaitFor.Length );
                        stdoutProcessedCount = 0;
                        stdoutsofar = new StringBuilder(trimmed);

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
                lock (stdOutLock)
                {
                    string stdOut = stdoutsofar.ToString();
                    stdoutProcessedCount = stdOut.Length;
                    // Since more than one line may have been returned, we need to split the line and make
                    // sure we only nom up the first.
                    if (stdOut.Contains("\n"))
                    {
                        string[] lines = stdOut.Split(new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

                        if (lines.Length == 0)
                            continue;

                        string foo = stdoutsofar.ToString().Substring(lines[0].Length + 1);
                        stdoutsofar = new StringBuilder( foo );

                        stdoutProcessedCount = 0;
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
                lock (stdOutLock)
                {
                    if (stdoutsofar.Length > stdoutProcessedCount)
                    {
                        stdoutProcessedCount = stdoutsofar.Length;
                        return;
                    }
                }
                if (exitTime)
                    return;
                Thread.Sleep(0);
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

                breakpoints.Add(newBP.id, newBP);
            }
        }

        /// <summary>
        /// Load a file. Pass a .cod file, or previously specify the processor type.
        /// This will also set the chipType field.
        /// </summary>
        /// <param name="filename"></param>
        private void load(string filename)
        {
            string lineToExec = String.Format("load \"{0}\"", filename );
            writeLine(lineToExec);
            waitForPrompt();

            // Now, ask gpsim for the processor list, and read our processor type from it. If the load
            // fails, gpsim will not report this (!), but the processor list will then be empty.
            writeLine(cmdProcessor);

            waitForSpecified(processorListStart);
            // Now get the first element of our processor list.
            string processorLine = getNextLine();
            waitForPrompt();

            chipType = (chipType) Enum.Parse(typeof(chipType), processorLine);
        }

        private void writeLine(string toWrite)
        {
            Thread.Sleep(10);
            gpSimProcess.StandardInput.Flush();
            gpSimProcess.StandardInput.Write(toWrite);
            gpSimProcess.StandardInput.Write("\n");
            gpSimProcess.StandardInput.Flush();
        }

        public void run()
        {
            lock (this)
            {
                writeLine("run");
                //writeLine("stfu");
                waitForSpecified("running...\r\n");
            }
        }

        private bool handleBreakpointStop(StringBuilder linesSoFar)
        {
            bool handledBreakpoint = false;
            // We should see one or more 'Message:...' lines, which indicate which breakpoint 
            // was hit, as they return the 'message' we set when we made the breakpoint - so 
            // that'll be our breakpoint ID.
            // We are not guaranteed to have a line ending in \r\n, so omit any partial lines.
            string withoutPartials = linesSoFar.ToString().Substring(0, linesSoFar.ToString().LastIndexOf('\n'));
            if (withoutPartials.Length <= stdoutProcessedCount)
                return false;
            withoutPartials = withoutPartials.Substring(stdoutProcessedCount);
            int n = 0;
            breakpoint toCall = null;
            foreach (string line in withoutPartials.Split('\n'))
            {
                string lineIn = line.ToLower().Trim(new[] { '\r', '\n', ' ', '\t' });

                if (lineIn.StartsWith("message:"))
                {
                    string idStr = lineIn.Split(':')[1];
                    int id = Convert.ToInt32(idStr);

                    int start = linesSoFar.ToString().ToLower().IndexOf(bpHitString) + bpHitString.Length + 2 + idStr.Length;
                    string foo = linesSoFar.ToString().Substring(start);

                    lock (stdOutLock)   // fuck, too late, should've done this earlier!
                    {
                        if (stdoutsofar.ToString() != linesSoFar.ToString())
                            throw new Exception();
                        stdoutsofar = new StringBuilder(foo);
                        stdoutProcessedCount = 0;
                    }

                    if (breakpoints.ContainsKey(id))
                        toCall = breakpoints[id];
                    break;
                }

                n += line.Length + 1;
            }

            if (toCall != null)
            {
                doBreakpointHit(toCall);

                return true;
            }

            return false;
        }

        private void doBreakpointHit(breakpoint hitBP)
        {
            _eventHandler.BeginInvoke(breakpoints[hitBP.id].callback, new object[] {this, hitBP} );
        }

        public void doBreakin()
        {
            lock (this)
            {
                //writeLine(breakIn);
                waitForSpecified(promptString.ToLower() + "\r\n" + promptString.ToLower());
            }
        }

        public int readMemory(string symToRead)
        {
            lock (this)
            {
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
                    if (lineIn.Contains(promptString))
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

        public TypeToReturn readMemory<TypeToReturn>(string symToRead)
            where TypeToReturn : sfr, new()
        {
            lock (this)
            {
                int rawToRetrun = readMemory(symToRead);

                sfr toRet = new TypeToReturn();
                toRet.setUnderlyingValue(rawToRetrun);

                return (TypeToReturn)toRet;
            }
        }

        public void Dispose()
        {
            try
            {
                exitTime = true;
                gpSimProcess.Close();
                if (!gpSimProcess.HasExited)
                    gpSimProcess.Kill();
                gpSimProcess.WaitForExit(2000);
            }
            catch (InvalidOperationException)
            {
                // Swallow these - they may happen if the process exists before we finish our try.
            }
            gpSimProcess.Dispose();
        }
    }
}