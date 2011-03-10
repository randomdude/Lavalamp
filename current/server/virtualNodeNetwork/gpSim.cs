using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace virtualNodeNetwork
{
    public class gpSim
    {
        public const string gpSimBinary = @"C:\Program Files (x86)\gpsim\bin\gpsim.exe";

        private const string promptString = @"**gpsim> ";
        private const string errorString = @"***error: ";
        private const string bpHitString = @"hit a breakpoint!";
        private Process gpsim;
        private StringBuilder stdoutsofar = new StringBuilder();
        private chipType chipType;

        public delegate void breakpointHandler(breakpoint hit);
        private readonly Dictionary<string, breakpointHandler> breakpointCallbacks = new Dictionary<string, breakpointHandler>();

        public gpSim(string filename, chipType newChipType)
        {
            chipType = newChipType;

            // Start up the executable. 
            ProcessStartInfo info = new ProcessStartInfo(gpSimBinary);
            info.Arguments = " -i ";    // command-line mode
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            gpsim = Process.Start(info);

            // Now wait for our prompt to be returned.
            waitForPrompt();

            setChip(chipType);
            load(filename + ".hex");
            load(filename + ".cod");
            writeLine("symbol");
        }

        private void waitForPrompt()
        {
            while (true)
            {
                char[] byteIn = new char[1];

                if (0 == gpsim.StandardOutput.Read(byteIn, 0, 1))
                    continue;

                stdoutsofar.Append(byteIn);

                if (stdoutsofar.ToString().ToLower().EndsWith(errorString))
                    throw new GPSimException();

                if (stdoutsofar.ToString().ToLower().EndsWith(promptString))
                    break;
            }
        }

        public void addWriteBreakpoint(string breakpiontLocation, breakpointHandler onByteWritten)
        {
            writeLine("br w " + chipType + "." + breakpiontLocation);
            waitForPrompt();

            breakpointCallbacks.Add(breakpiontLocation, onByteWritten);
        }

        private void load(string hexFile)
        {
            writeLine("load \"" + hexFile.ToString() + "\"");
            waitForPrompt();            
        }

        private void setChip(chipType toSetTo)
        {
            writeLine("processor "  + toSetTo.ToString());
            waitForPrompt();
        }

        private void writeLine(string toWrite)
        {
            gpsim.StandardInput.Write(toWrite);
            gpsim.StandardInput.Write('\n');
            gpsim.StandardInput.Flush();
        }

        public void run()
        {
            writeLine("run");

            while (true)
            {
                // Wait for the simulator to stop, then read a line from the console to see why we've stopped.
                string lineIn = gpsim.StandardOutput.ReadLine();
                if (lineIn == null)
                    continue;

                if (lineIn.ToLower().StartsWith(bpHitString))
                    handleBreakpointStop();
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
    }
}