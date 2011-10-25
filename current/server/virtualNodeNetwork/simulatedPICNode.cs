using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using virtualNodeNetwork.PICStuff;

namespace virtualNodeNetwork.PICStuff
{
    public abstract class sfr
    {
        public int rawVal;

        public bool hasBit(int toCheckIndex)
        {
            return ((rawVal) | ((int)(toCheckIndex))) != 0;
        }

        public virtual void setUnderlyingValue(int raw)
        {
            if (raw > 0xFF)
                throw new ArgumentException();

            rawVal = raw;
        }
    }

    public class sfrPIR1 : sfr
    {
        public static string name = "pir1";

        public sfrPIR1Bits val;

        public override void setUnderlyingValue(int raw)
        {
            rawVal = raw;
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
            rawVal = raw;
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
            rawVal = raw;
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
            rawVal = raw;
            val = (sfrPIE1Bits)raw;
        }

        public bool hasBit(sfrPIE1Bits toCheck)
        {
            return (((int)val) | ((int)(toCheck))) == 0 ? false : true;
        }
    }

    public class sfrTXREG : sfr { public static string name = "txreg"; }
    public class sfrRCREG : sfr { public static string name = "rcreg"; }
    public class sfrPORTB : sfr { public static string name = "portb"; }

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
        private readonly gpSim _simulator;

        public simulatedPICNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors) 
            : base(newId, newName, newSensors)
        {
            throw new NotImplementedException();
        }

        public simulatedPICNode(int newId, string newName, Form eventForm, string objectFile) :base(newId, newName)
        {
            // Make our new simulator..
            _simulator = new gpSim(objectFile, eventForm );

            // And add breakpoints on things which are important - rs232 IO, and the debug pins.
            lock (_simulator)
            {
                _simulator.addWriteBreakpoint(sfrTXREG.name, onByteWritten);
                _simulator.addWriteBreakpoint(sfrPORTB.name, onPortBChange);
                _simulator.run();
            }
        }

        private void onPortBChange(gpSim sender, breakpoint hit)
        {
            // OK, a pin has changed in PORTB. We should examine portb and discover if any interesting bits
            // are now set.
            sfrPORTB portb = sender.readMemory<sfrPORTB>(sfrPORTB.name);
            if (portb.hasBit(0))
            {
                // A sync packet is being reported by the debug interface. Fire the appropriate event.
                syncPacket();
            }
        }

        private int _packetCount = 0;
        private readonly byte[] _packet = new byte[networkPacket.lengthInBytes];
        private void onByteWritten(gpSim sender, breakpoint hit)
        {
            _packet[_packetCount++] = (byte) sender.readMemory( hit.location );

            if (_packetCount > networkPacket.lengthInBytes)
                onSendPacket(new networkPacket(_packet));
        }

        public void processByte(byte[] byteRead)
        {
            // Send this byte to the PIC.
            int n = 0;
            while (n < byteRead.Length )
            {
                lock (_simulator)
                {
                    _simulator.doBreakin();

                    // Check that the PIC has enabled the UART
                    sfrRCSTA rcsta = _simulator.readMemory<sfrRCSTA>(sfrRCSTA.name);
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
                    sfrTXSTA txsta = _simulator.readMemory<sfrTXSTA>(sfrTXSTA.name);
                    if (txsta.hasBit(sfrTXSTABits.SYNC))
                        throw new NotSupportedException();
                    sfrPIE1 pie1 = _simulator.readMemory<sfrPIE1>(sfrPIE1.name);
                    if (pie1.hasBit(sfrPIE1Bits.RCIE))
                        throw new NotSupportedException();

                    // Otherwise, it's all good. We simulate reception thus:
                    // 1: Set RCREG to the incoming byte
                    // 2: Set PIR1.RCIF.
                    _simulator.writeMemory(sfrRCREG.name, byteRead[n]);
                    _simulator.writeMemory(sfrPIR1.name, (int) sfrPIR1Bits.TXIF, 1);
                }
                n++;
            }
        }
    }

    public class breakpoint
    {
        public string location;
        public readonly int id;
        private static int _count = 0;
        public gpSim.breakpointHandler callback;

        public breakpoint()
        {
            id = (_count++);
        }
    }

    public class GPSimException : Exception {
        public GPSimException() { }
        public GPSimException(string whyso) : base(whyso) { }
    }

    /// <summary>
    /// This class translates between a range of almost-but-not-quite-the-same PIC names.
    /// </summary>
    public class chipType
    {
        public underlyingType_t underlyingType;

        public chipType(underlyingType_t newType)
        {
            underlyingType = newType;
        }

        public enum underlyingType_t
        {
            pic16f628
        }

        /// <summary>
        /// Return, eg, "p16f628".
        /// </summary>
        public string toGPSimStyle
        {
            get
            {
                switch(underlyingType)
                {
                    case underlyingType_t.pic16f628:
                        return "p16f628";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static chipType parse(string processorLine)
        {
            // We parse the input under a variety of different sources, returning when one seems
            // to match. We just use a large switch() for now. It may be better to parse this
            // algorithmically in the future.
            switch(processorLine)
            {
                case "16f628":
                case "p16f628":
                case "pic16f628":
                    return new chipType(underlyingType_t.pic16f628);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return underlyingType.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals((chipType)obj);
        }

// ReSharper disable InconsistentNaming
        private bool Equals(chipType other)
// ReSharper restore InconsistentNaming
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.underlyingType, underlyingType);
        }

        public override int GetHashCode()
        {
            return underlyingType.GetHashCode();
        }
    }

}