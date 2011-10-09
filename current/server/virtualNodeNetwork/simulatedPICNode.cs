using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
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
    /// <summary>
    /// A virtual node which is implemented by simulated PIC code.
    /// </summary>
    public class simulatedPICNode : virtualNodeBase
    {
        private readonly gpSim simulator;

        public simulatedPICNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors) 
            : base(newId, newName, newSensors)
        {
            throw new NotImplementedException();
        }

        public simulatedPICNode(int newId, string newName, Form eventForm, string objectFile) :base(newId, newName)
        {
            simulator = new gpSim(objectFile, eventForm);
            lock (simulator)
            {
                simulator.addWriteBreakpoint(sfrTXREG.name, onByteWritten);
                simulator.run();
            }
        }

        private int packetCount = 0;
        private readonly byte[] packet = new byte[networkPacket.lengthInBytes];
        private void onByteWritten(gpSim sender, breakpoint hit)
        {
            packet[packetCount++] = (byte) sender.readMemory( hit.location );

            if (packetCount > networkPacket.lengthInBytes)
                onSendPacket(new networkPacket(packet));
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
        public string location;
        public readonly int id;
        private static int count = 0;
        public gpSim.breakpointHandler callback;

        public breakpoint()
        {
            id = (count++);
        }
    }

    public class GPSimException : Exception {}

    public enum chipType
    {
        p16f628
    }

}