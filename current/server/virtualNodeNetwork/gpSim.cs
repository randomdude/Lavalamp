using System;
using System.Collections.Generic;
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
        // The keystroke we use to pause PIC execution
        private static readonly string breakIn = ((char)0x03).ToString();

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

        public gpSim(string filename)
        {
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
                    stdoutsofar.Append((char)charInBuf[0]);

                    // If we see a 'Message:...' line, this indicates that a breakpoint was hit. 
                    if (stdoutsofar.ToString().ToLower().Contains(bpHitString) )
                        handleBreakpointStop(stdoutsofar);
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
                        stdoutProcessedCount = trimmed.Length;
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
                waitForNewData();

                lock (stdOutLock)
                {
                    string stdOut = stdoutsofar.ToString();
                    // Since more than one line may have been returned, we need to split the line and make
                    // sure we only nom up the first.
                    if (stdOut.Contains("\n"))
                    {
                        string[] lines = stdOut.Split(new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

                        if (lines.Length == 0)
                            stdoutsofar = new StringBuilder();
                        else
                            stdoutsofar = new StringBuilder( stdoutsofar.ToString().Substring( lines[0].Length ) );

                        stdoutProcessedCount = 0;
                        return lines[0].TrimEnd(new [] {'\r', '\n' } );
                    }
                }
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

        private void handleBreakpointStop(StringBuilder linesSoFar)
        {
            // We should see one or more 'Message:...' lines, which indicate which breakpoint 
            // was hit, as they return the 'message' we set when we made the breakpoint - so 
            // that'll be our breakpoint ID.
            // We are not guaranteed to have a line ending in \r\n, so omit any partial lines.
            string withoutPartials = linesSoFar.ToString().Substring(0, linesSoFar.ToString().LastIndexOf('\n'));
            int n = 0;
            foreach (string line in withoutPartials.Split('\n'))
            {
                string lineIn = line.ToLower().Trim(new[] { '\r', '\n', ' ', '\t' });

                if (lineIn.StartsWith("message:"))
                {
                    int id = Convert.ToInt32(lineIn.Split(':')[1]);

                    if (breakpoints.ContainsKey(id))
                        doBreakpointHit(breakpoints[id]);

                    stdoutProcessedCount = n + line.Length;
                }

                n += line.Length + 1;
            }
        }

        private void doBreakpointHit(breakpoint hitBP)
        {
            breakpoints[hitBP.id].callback.Invoke(this, hitBP);            
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

                while (true)
                {
                    string lineIn = gpSimProcess.StandardOutput.ReadLine();
                    if (lineIn == null)
                        continue;

                    // The prompt, as our string is echoed back to us
                    if (lineIn.StartsWith(promptString))
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
                writeLine("quit");

                gpSimProcess.Close();
                if (!gpSimProcess.HasExited)
                    gpSimProcess.Kill();
                gpSimProcess.WaitForExit();
            }
            catch (InvalidOperationException)
            {
                // Swallow these - they may happen if the process exists before we finish our try.
            }
            gpSimProcess.Dispose();
        }
    }
}