using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using virtualNodeNetwork.PICStuff;

namespace virtualNodeNetwork
{
    public class gpSim
    {
        private const string gpSimBinary = @"C:\Program Files (x86)\gpsim\bin\gpsim.exe";

        private const string promptString = @"**gpsim> ";
        private const string errorString = @"***error: ";
        private const string breakpointCannotBeSetOnString = @"break cannot be set on";
        private const string bpHitString = @"hit a breakpoint!";
        private static readonly string breakIn = ((char)0x03).ToString();
        private Process gpsim;
        private StringBuilder stdoutsofar = new StringBuilder();
        private int stdoutSizeSoFar = 0;
        private object stdOutLock = new object();
        private readonly chipType chipType;

        public delegate void breakpointHandler(breakpoint hit);
        private readonly Dictionary<string, breakpointHandler> breakpointCallbacks = new Dictionary<string, breakpointHandler>();

        byte[] charInBuf = new byte[1];

        public gpSim(string filename, chipType newChipType)
        {
            chipType = newChipType;

            // Start up the executable. 
            ProcessStartInfo info = new ProcessStartInfo(gpSimBinary);
            info.Arguments = " -i ";    // command-line mode
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            gpsim = new Process();
            gpsim.StartInfo = info;
            gpsim.Start();
            // FIXME - won't we miss data from gpsim if it's scheduled to run before our read?
            gpsim.StandardOutput.BaseStream.BeginRead(charInBuf, 0, 1, gpSimOutputDataRecieved, this);

            // Now wait for our prompt to be returned.
            waitForPrompt(0);

            setChip(chipType);
            load(filename + ".hex");
            load(filename + ".cod");
        }

        private void gpSimOutputDataRecieved(IAsyncResult ar)
        {
            if (gpsim.StandardOutput.BaseStream.EndRead(ar) == 0)
                return;

            lock (stdOutLock)
            {
                stdoutsofar.Append((char)charInBuf[0]);
                stdoutSizeSoFar += 1;

                if (stdoutsofar.ToString().ToLower().StartsWith(bpHitString))
                {
                    handleBreakpointStop();
                }
            }
            gpsim.StandardOutput.BaseStream.BeginRead(charInBuf, 0, 1, gpSimOutputDataRecieved, this);
        }

        private void waitForPrompt(int stdoutSizeInitial)
        {
            waitForPrompt(stdoutSizeInitial, promptString.ToLower());
        }

        private void waitForPrompt(int stdoutSizeInitial, string toWaitFor)
        {
            while (true)
            {
                while (true)
                {
                    lock (stdOutLock)
                    {
                        // Wait for data to be appended to stdOut
                        int soFar = stdoutSizeSoFar;
                        if (stdoutSizeInitial < soFar)
                        {
                            stdoutSizeInitial = soFar;
                            break;
                        }
                    }
                    Thread.Sleep(0);
                }

                lock (stdOutLock)
                {
                    string soFar =  stdoutsofar.ToString();

                    //foreach(string thisLine in soFar.Split('\n'))
                    {
                        if (soFar.ToLower().Contains(errorString.ToLower()))
                            throw new GPSimException();

                        if (soFar.ToString().ToLower().Contains(breakpointCannotBeSetOnString.ToLower()))
                            throw new GPSimException();

                        if (soFar.ToString().ToLower().Contains(toWaitFor))
                            return;
                    }

                    // Will miss multi-line stuff when the final line isn't finished yet
                    if (soFar.EndsWith("\n"))
                    {
                        stdoutSizeSoFar = 0;
                        stdoutSizeInitial = 0;
                        stdoutsofar = new StringBuilder();
                    }
                }
            }
        }

        public void addWriteBreakpoint(string locationSymbol, breakpointHandler onByteWritten)
        {
            lock (this)
            {
                int stdoutSizeInitial = stdoutSizeSoFar;
                writeLine("br w " + chipType + "." + locationSymbol);
                waitForPrompt(stdoutSizeInitial);

                breakpointCallbacks.Add(locationSymbol, onByteWritten);
            }
        }

        private void load(string hexFile)
        {
            int stdoutSizeInitial = stdoutSizeSoFar;
            writeLine("load \"" + hexFile.ToString() + "\"");
            waitForPrompt(stdoutSizeInitial);            
        }

        private void setChip(chipType toSetTo)
        {
            int stdoutSizeInitial = stdoutSizeSoFar;
            writeLine("processor " + toSetTo.ToString());
            waitForPrompt(stdoutSizeInitial);
        }

        private void writeLine(string toWrite)
        {
            gpsim.StandardInput.Write(toWrite);
            gpsim.StandardInput.Write('\n');
            gpsim.StandardInput.Flush();
        }

        public void run()
        {
            lock (this)
            {
                int stdoutSizeInitial = stdoutSizeSoFar;
                Thread.Sleep(10*1000);
                writeLine("run");
                //writeLine("stfu");
                waitForPrompt(stdoutSizeInitial, "running...");
            }
        }

        private void handleBreakpointStop()
        {
            // Read from stdIn until we get to the prompt. If we see 'read' or 'wrote', parse the line to see
            // which addresses were read or written.
            while (true)
            {
                string lineIn = gpsim.StandardOutput.ReadLine();
                if (lineIn == null)
                    continue;

                string trimmedLineIn = lineIn.ToLower().Trim();
                if (trimmedLineIn.StartsWith("read") ||
                    trimmedLineIn.StartsWith("wrote")   )
                {
                    breakpoint hitbp = new breakpoint(trimmedLineIn);

                    if (breakpointCallbacks.ContainsKey(hitbp.location))
                        breakpointCallbacks[hitbp.location].Invoke(hitbp);
                }
            } 
        }

        public void doBreakin()
        {
            lock (this)
            {
                int stdoutSizeInitial = stdoutSizeSoFar;
                //writeLine(breakIn);
                waitForPrompt(stdoutSizeInitial, promptString.ToLower() + "\r\n" + promptString.ToLower());
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
                    string lineIn = gpsim.StandardOutput.ReadLine();
                    if (lineIn == null)
                        continue;

                    // The prompt, as our string is echoed back to us
                    if (lineIn.StartsWith(promptString))
                        continue;

                    // OK, this should be our line.
                    string[] chunks = lineIn.Split('=');

                    if (chunks.Length != 3)
                        throw new GPSimException();

                    string hexNum = chunks[2].Trim();

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

                return (TypeToReturn) toRet;
            }
        }
    }
}