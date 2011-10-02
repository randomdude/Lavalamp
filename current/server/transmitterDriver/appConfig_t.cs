using System;
using System.Runtime.InteropServices;

namespace transmitterDriver
{
    [StructLayout(LayoutKind.Sequential)]
    public struct appConfig_t
    {
        public byte nodeid;
        public byte sensorid;

        private IntPtr unsafePortName;
        public string portName
        {
            get { return Marshal.PtrToStringAnsi(unsafePortName); }
            set
            {
                // FIXME - This will leak, surely? StringToHGlobalAnsi will allocate unmanaged memory.
                unsafePortName = Marshal.StringToHGlobalAnsi(value);
            }
        }

        public bool useEncryption;

        public Int32 key1;
        public Int32 key2;
        public Int32 key3;
        public Int32 key4;

        public int verbose;
        public IntPtr hnd;
        public bool machineoutput;
        public int com_timeout;
        public bool assume_synced;
        public int retries;
        public bool isSerialPort;
        public bool injectFaultInvalidResponse;
    }

}