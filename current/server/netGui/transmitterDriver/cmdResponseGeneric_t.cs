using System;
using System.Runtime.InteropServices;

namespace netGui
{
    public class disposableCommand : pinvokeWrapper, IDisposable
    {
        /// <summary>
        /// An IntPtr to the raw data which the dll returned
        /// </summary>
        private readonly IntPtr unmanagedPointer;

        protected disposableCommand(IntPtr rawData)
        {
            unmanagedPointer = rawData;            
        }

        public void Dispose()
        {
            cmd_free(unmanagedPointer);
        }  
    }

    public class cmdResponseGeneric_t : disposableCommand, ICmdResponse
    {
        public errorcode_enum errorcode
        {
            get { return responsePacket.errorcode; }
            set { responsePacket.errorcode = value; }
        }

        public Int32 totaltime
        {
            get { return responsePacket.totaltime; }
            set { responsePacket.totaltime = value; }
        }
        
        public Int32 response
        {
            get { return responsePacket.response; }
            set { responsePacket.response = value; }
        }

        /// <summary>
        /// The interop-parsed data which the dll returned
        /// </summary>
        private cmdResponseGeneric_t_unsafe responsePacket;

        public cmdResponseGeneric_t(IntPtr rawResponse)
            : base(rawResponse)
        {
            responsePacket = (cmdResponseGeneric_t_unsafe)Marshal.PtrToStructure(rawResponse, typeof(cmdResponseGeneric_t_unsafe));
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct cmdResponseGeneric_t_unsafe
        {
            public errorcode_enum errorcode;
            public Int32 response;
            public Int32 totaltime;
        }

    }

    public class cmdResponseIdentify_t : disposableCommand, ICmdResponse
    {
        public errorcode_enum errorcode
        {
            get { return responsePacket.errorcode; }
        }

        public Int32 totaltime
        {
            get { return responsePacket.totaltime; }
        }

        public byte[] response
        {
            get { return responsePacket.response;  }
        }

        private cmdResponseIdentify_t_unsafe responsePacket;

        public cmdResponseIdentify_t(IntPtr rawResponse)
            : base(rawResponse)
        {
            responsePacket = (cmdResponseIdentify_t_unsafe)Marshal.PtrToStructure(rawResponse, typeof(cmdResponseIdentify_t_unsafe));
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct cmdResponseIdentify_t_unsafe
        {
            [MarshalAs(UnmanagedType.U4)]
            public readonly errorcode_enum errorcode;

            // The driver limits this field to be a max of 0x21 bytes.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType  = UnmanagedType.U1, SizeConst = 0x21)]
            public readonly byte[] response;

            [MarshalAs(UnmanagedType.U4)]
            public readonly Int32 totaltime;
        }
    }

    public interface ICmdResponse
    {
        errorcode_enum errorcode { get; }
        Int32 totaltime { get; }
    }
    
    public struct cmdResponseGetSensorType_t
    {
        errorcode_enum errorcode;
        Int32 type;
        IntPtr FriendlyType;
        Int32 totaltime;
    };
}