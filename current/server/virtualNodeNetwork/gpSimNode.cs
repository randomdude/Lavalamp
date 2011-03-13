using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace virtualNodeNetwork
{
    public class gpSimNode : virtualNodeBase
    {
        private gpSim simulator;
        private static string hexFile = @"C:\c0adz\lavalamp\current\uC\node\lavalamp";

        public gpSimNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors) : base(newId, newName, newSensors)
        {
            throw new NotImplementedException();
        }

        public gpSimNode(int newId, string newName) :base(newId, newName)
        {
            simulator = new gpSim(hexFile, chipType.p16f627);
            lock (simulator)
            {
                simulator.addWriteBreakpoint("txreg", onByteWritten);

                ParameterizedThreadStart nodeThread = simulatorRunner;
                simulator.run();
            }
        }

        private int packetCount = 0;
        private byte[] packet = new byte[networkPacket.lengthInBytes];
        private void onByteWritten(breakpoint hit)
        {
            if (hit.type != bpHitType.write)
                return;

            packet[packetCount++] = hit.value;

            if (packetCount > networkPacket.lengthInBytes)
                onSendPacket(new networkPacket(packet));
        }

        private void simulatorRunner(object obj)
        {
            lock (simulator)
            {
                simulator.run();
            }
        }

        public void processByte(byte[] byteRead)
        {
            
        }
    }

    public class breakpoint
    {
        public byte value;
        public string location;
        public bpHitType type;

        public breakpoint(string trimmedLineIn)
        {
            trimmedLineIn = trimmedLineIn.Trim();

            // Parse the line. It should be in a format similar to
            // [Read|Wrote]: 0x0045 [to|from] txreg[(0x0019)] [was 0x0054]
            string[] words = trimmedLineIn.Split(' ');
            string dataWritten = words[1];
            if (words[2].ToLower() != "to" &&
                words[2].ToLower() != "from")
                throw new GPSimException();

            if (words[0].ToLower() == "read:")
                type = bpHitType.read;
            else if (words[0].ToLower() == "wrote:")
                type = bpHitType.write;
            else
                throw new GPSimException();

            location = words[3];

            // Trim any bracketed info from the location
            if (location.Contains("("))
                location = location.Split('(')[0];

            // parse the '0x1234' written value
            if (dataWritten[0] != '0' ||
                dataWritten[1] != 'x'   )
                throw new GPSimException();

            // The top byte should be zero.
            if (dataWritten[2] != '0' ||
                dataWritten[3] != '0')
                throw new GPSimException();

            // OK. Yoink the bottom byte.
            string byteToParse = string.Format("{0}{1}", dataWritten[4], dataWritten[5]);

            value = byte.Parse(byteToParse);
        }
    }

    public class GPSimException : Exception {}

    public enum bpHitType
    {
        read, write
    }

    public enum chipType
    {
        p16f627
    }

}