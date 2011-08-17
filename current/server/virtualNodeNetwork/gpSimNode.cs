using System;
using System.Collections.Generic;
using System.Threading;
using virtualNodeNetwork.PICStuff;

namespace virtualNodeNetwork.PICStuff
{
    public abstract class sfr
    {
        public abstract void setUnderlyingValue(int raw);
    }

    public class sfrPIR1 : sfr
    {
        public static string name = "pir1";

        public sfrPIR1Bits val;

        public override void setUnderlyingValue(int raw)
        {
            val = (sfrPIR1Bits) raw;
        }

        public bool hasBit(sfrPIR1Bits toCheck)
        {
            return (((int)val) | ((int)(toCheck)) ) != 0;
        }
    }

    public class sfrTXSTA : sfr
    {
        public static string name = "txsta";

        public sfrTXSTABits val;

        public override void setUnderlyingValue(int raw)
        {
            val = (sfrTXSTABits)raw;
        }

        public bool hasBit(sfrTXSTABits toCheck)
        {
            return (((int)val) | ((int)(toCheck))) == 0 ? false : true;
        }
    }

    public class sfrRCSTA : sfr
    {
        public static string name = "rcsta";

        public sfrRCSTABits val;
        
        /// <summary>
        /// Is 9-bit reception enabled?
        /// </summary>
        public int byteSize
        {
            get { return hasBit(sfrRCSTABits.RX9) ? 9 : 8; }
        }

        public override void setUnderlyingValue(int raw)
        {
            val = (sfrRCSTABits)raw;
        }

        public bool hasBit(sfrRCSTABits toCheck)
        {
            return (((int)val) | ((int)(toCheck))) == 0 ? false : true;
        }
    }

    public class sfrPIE1 : sfr
    {
        public static string name = "pie1";

        public sfrPIE1Bits val;

        public override void setUnderlyingValue(int raw)
        {
            val = (sfrPIE1Bits)raw;
        }

        public bool hasBit(sfrPIE1Bits toCheck)
        {
            return (((int)val) | ((int)(toCheck))) == 0 ? false : true;
        }
    }

    public class sfrTXREG : sfr
    {
        public static string name = "txreg";

        public byte val;

        public override void setUnderlyingValue(int raw)
        {
            if (raw > 0xFF)
                throw new ArgumentException();

            val = (byte) raw;
        }
    }

    public enum sfrPIR1Bits
    {
        TMR1IF = 0x01,
        TMR2IF = 0x02,
        CCP1IF = 0x04,
        unused = 0x08,
        TXIF = 0x10,
        RCIF = 0x20,
        CMIF = 0x40,
        EEIF = 0x80
    }

    public enum sfrTXSTABits
    {
        TX9D = 0x01,
        TRMT = 0x02,
        BRGH = 0x04,
        Unused = 0x08,
        SYNC = 0x10,
        TXEN = 0x20,
        TX9 = 0x40,
        CSRC = 0x80
    }

    public enum sfrRCSTABits
    {
        SPEN = 0x80,
        RX9 = 0x40, 
        SREN = 0x20, 
        CREN = 0x10, 
        ADEN = 0x08, 
        FERR = 0x04, 
        OERR = 0x02, 
        RX9D = 0x01
    }

    public enum sfrPIE1Bits
    { 
        EEIE = 0x80,
        CMIE = 0x40,
        /// <summary>
        /// UART Rx Interrupt enable
        /// </summary>
        RCIE = 0x20,
        TXIE = 0x10,
        unused = 0x08,
        CCP1IE = 0x04,
        TMR2IE = 0x02,
        TMR1IE = 0x01
    }

}

namespace virtualNodeNetwork
{
    public class gpSimNode : virtualNodeBase
    {
        private gpSim simulator;
        private static string hexFile = @"C:\c0adz\lavalamp\current\uC\node\lavalamp";

        public gpSimNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors) 
            : base(newId, newName, newSensors)
        {
            throw new NotImplementedException();
        }

        public gpSimNode(int newId, string newName) :base(newId, newName)
        {
            simulator = new gpSim(hexFile, chipType.p16f627);
            lock (simulator)
            {
                simulator.addWriteBreakpoint(sfrTXREG.name, onByteWritten);

                ParameterizedThreadStart nodeThreadStart = simulatorRunner;
                Thread nodeThread = new Thread(nodeThreadStart);
                nodeThread.Name = "GPSimNode thread, node id " + newId + " name '" + newName + "'";
                nodeThread.Start();
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
            simulator.run();
        }

        public void processByte(byte[] byteRead)
        {
            // Send this byte to the PIC.
            int n = 0;
            while (n < byteRead.Length )
            {
                lock (simulator)
                {
                    simulator.doBreakin();

                    // Check that the PIC has enabled the UART
                    sfrRCSTA rcsta = simulator.readMemory<sfrRCSTA>(sfrRCSTA.name);
                    if (!rcsta.hasBit(sfrRCSTABits.SPEN))
                    {
                        // The UART is disabled. We could drop bytes, but since we should never
                        // send data to a PIC unable to recieve it, this is an error condition.
                        throw new GPSimException();
                    }
                    // Check for some unsupported scenarios
                    if ( rcsta.byteSize != 8 ||
                        !rcsta.hasBit(sfrRCSTABits.CREN) )
                    {
                        throw new NotSupportedException();
                    }
                    sfrTXSTA txsta = simulator.readMemory<sfrTXSTA>(sfrTXSTA.name);
                    if (txsta.hasBit(sfrTXSTABits.SYNC))
                        throw new NotSupportedException();
                    sfrPIE1 pie1 = simulator.readMemory<sfrPIE1>(sfrPIE1.name);
                    if (pie1.hasBit(sfrPIE1Bits.RCIE))
                        throw new NotSupportedException();

                }
            }
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