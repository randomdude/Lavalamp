using System;
using System.Runtime.InteropServices;
using System.Threading;
using HANDLE = System.IntPtr;
using CharPtr = System.IntPtr;

namespace netGui
{
	public class transmitterDriver : ITransmitter
	{
		#region "pinvoke declarations"
	 	/* 	error codes below 0x20 are reserved for the low-level comms subsystem. Above is errors returned from the PIC
		 *	Note that errors returned from the PIC are command-dependant and thus need parsing at the PC-side, depending
		 *	which command they are returned from.
		 */

	    public enum errorcode_enum { errcode_none,	errcode_timeout,errcode_crypto,	errcode_internal,errcode_portstate, 
									  errcode_sensor_not_found=0x20, errcode_sensor_wrong_type };

        public class cmdResponseGeneric_t :IDisposable
        {
            public errorcode_enum errorcode
            {
                get { return responsePacket.errorcode; }
                set { responsePacket.errorcode = value; }
            }

            public Int32 response
            {
                get { return responsePacket.response;  }
                set { responsePacket.response = value; }
            }

            public Int32 totaltime
            {
                get { return responsePacket.totaltime; }
                set { responsePacket.totaltime = value; }
            }

            /// <summary>
            /// An IntPtr to the raw data which the dll returned
            /// </summary>
            private readonly IntPtr unmanagedPointer;

            /// <summary>
            /// The interop-parsed data which the dll returned
            /// </summary>
            private cmdResponseGeneric_t_unsafe responsePacket;

            public cmdResponseGeneric_t(IntPtr rawResponse)
            {
                // Save our raw pointer so that we can free it later on.
                unmanagedPointer = rawResponse;

                responsePacket = (cmdResponseGeneric_t_unsafe)Marshal.PtrToStructure(unmanagedPointer, typeof(cmdResponseGeneric_t_unsafe));
            }

            public void Dispose()
            {
                cmd_free(unmanagedPointer);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
		private struct cmdResponseGeneric_t_unsafe
		{
		    public errorcode_enum errorcode;
            public Int32 response;
            public Int32 totaltime;
		}

		private struct cmdResponseIdentify_t
		{
			errorcode_enum errorcode;
			IntPtr response; //max size 0x20;
			Int32 totaltime;
		}
		
		private struct cmdResponseGetSensorType_t
		{
			errorcode_enum errorcode;
			Int32 type;
			CharPtr FriendlyType;
			Int32 totaltime;
		};

        [StructLayout(LayoutKind.Sequential)]
        public struct appConfig_t_ptr
        {
            private readonly IntPtr unsafeHandle;

            public appConfig_t appConfig
            {
                get { 
                    return (appConfig_t) Marshal.PtrToStructure(unsafeHandle, typeof(appConfig_t)); 
                }
            }
        }

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

	    public enum sensorTypeEnum
	    {
		    unknown = 0x00,
		    generic_digital_in = 0x01,
		    generic_digital_out = 0x02,
		    pwm_out = 0x03,
		    triac_out = 0x04
	    }

        public class sensorType
        {
            public String FriendlyType;
            Int16 id;
            public sensorTypeEnum enumeratedType;
        }

        #region exceptions
        public class badPortException			: Exception { }
        public class cantOpenPortException		: Exception { }
        public class cantClosePortException	    : Exception { }
        public class InternalErrorException	    : Exception { }
        public class commsException			    : Exception { }
        public class commsCryptoException		: commsException { };
        public class commsTimeoutException		: commsException { };
        public class commsPortStateException	: commsException { };
        public class commsInternalException	    : commsException { };
        public class userCancelledException	    : Exception { }
        public class cantHandleSensorException  : Exception { }
        public class sensorException			: Exception { }
        public class sensorNotFoundException	: sensorException { };
        public class sensorWrongTypeException	: sensorException { };
        #endregion

        [DllImport("lavalampdll.dll", EntryPoint = "initPort")]
        private static extern bool initPort(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "getTestConfig")]
        private static extern appConfig_t_ptr _getTestConfig();
        public static appConfig_t getTestConfig()
        {
            return _getTestConfig().appConfig;
        }

	    [DllImport("lavalampdll.dll",EntryPoint="closePort")]
		private static extern void closePort(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="closePort")]
		private static extern bool isPortOpen(appConfig_t config);

		[DllImport("lavalampdll.dll",EntryPoint="syncNetwork")]
		private static extern void syncNetwork(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdPing")]
		private static extern IntPtr _cmdPing(ref appConfig_t config);

        private static cmdResponseGeneric_t cmdPing(ref appConfig_t config)
        {
            return new cmdResponseGeneric_t(_cmdPing(ref config));
        }
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdReload")]
		private static extern cmdResponseGeneric_t cmdReload(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdIdentify")]
		private static extern  cmdResponseIdentify_t cmdIdentify(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdCountSensors")]
		private static extern cmdResponseGeneric_t cmdCountSensors(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdGetGenericDigitalSensor")]
		private static extern cmdResponseGeneric_t  cmdGetGenericDigitalSensor(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdSetGenericDigitalSensor")]
		private static extern cmdResponseGeneric_t cmdSetGenericDigitalSensor( appConfig_t config, IntPtr tothis);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdSetSensorFadeSpeed")]
		private static extern cmdResponseGeneric_t cmdSetSensorFadeSpeed(appConfig_t config, byte tothis);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdGetSensorType")]
		private static extern cmdResponseGetSensorType_t cmdGetSensorType(appConfig_t config);		
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdSetP")]
		private static extern cmdResponseGeneric_t cmdSetP(appConfig_t config, byte byte1, byte byte2, bool isHigh);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdSetNodeId")]
		private static extern cmdResponseGeneric_t cmdSetNodeId(appConfig_t config, byte tothis);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdSetNodeKeyByte")]
		private static extern cmdResponseGeneric_t cmdSetNodeKeyByte(appConfig_t config, byte index, byte newVal);
		
		[DllImport("lavalampdll.dll",EntryPoint="geterrormessage")]
		private static extern CharPtr getErrorMessage(int errorcode);

        [DllImport("lavalampdll.dll", EntryPoint = "cmd_free")]
        private static extern void cmd_free(IntPtr stuff);
        
        #endregion

        appConfig_t myseshdata;
	    private System.Threading.Mutex serialLock;
			
		public transmitterDriver (string strPortName, bool useEncryption, byte[] key)
        {
			if (string.IsNullOrEmpty(strPortName))
				throw new badPortException();

            myseshdata.portName = strPortName;

			// Pass through encryption setting
            myseshdata.useEncryption = useEncryption;

			// Now, open the port.
			if (!initPort(ref myseshdata))
				throw new cantOpenPortException();

            // we lock the port at this level.
            serialLock= new Mutex();

            // Save the key
            if (useEncryption)
            {
                if (key.Length != 16) throw new ArgumentException("Key is of an invalid length");

                myseshdata.key1 =	(key[ 3] <<  0 ) |
                                    (key[ 2] <<  8 ) | 
                                    (key[ 1] << 16 ) | 
                                    (key[ 0] << 24 )   ;
                myseshdata.key2 =	(key[ 7] <<  0 ) |
                                    (key[ 6] <<  8 ) | 
                                    (key[ 5] << 16 ) | 
                                    (key[ 4] << 24 )   ;
                myseshdata.key3 =	(key[11] <<  0 ) |
                                    (key[10] <<  8 ) | 
                                    (key[ 9] << 16 ) | 
                                    (key[ 8] << 24 )   ;
                myseshdata.key4 =	(key[15] <<  0 ) |
                                    (key[14] <<  8 ) | 
                                    (key[13] << 16 ) | 
                                    (key[12] << 24 )   ;
            }

            myseshdata.assume_synced = false;
            myseshdata.verbose = 0;
            myseshdata.machineoutput = false;
            myseshdata.com_timeout = 5;
            myseshdata.retries = 1;        
        }

	    private static void throwerror(errorcode_enum errcode)
		{
		    switch (errcode)
		    {
		        case errorcode_enum.errcode_none:
		            return;
		        case errorcode_enum.errcode_timeout:
		            throw new commsTimeoutException();
		        case errorcode_enum.errcode_crypto:
		            throw new commsCryptoException();
		        case errorcode_enum.errcode_portstate:
		            throw new commsPortStateException();
		        case errorcode_enum.errcode_sensor_not_found:
		            throw new sensorNotFoundException();
		        case errorcode_enum.errcode_sensor_wrong_type:
		            throw new sensorWrongTypeException();

                case errorcode_enum.errcode_internal:
                default:
		            throw new InternalErrorException();
		    }
		}

	    public void setInjectFaultInvalidResponse(bool newVal)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSyncNetwork()
        {
            throw new NotImplementedException();
        }

	    public void Dispose()
	    {
	        throw new NotImplementedException();
	    }

	    public bool portOpen()
	    {
	        throw new NotImplementedException();
	    }

	    public string doIdentify(short nodeId)
	    {
	        throw new NotImplementedException();
	    }

	    public short doGetSensorCount(short nodeId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doPing(short nodeId)
	    {
            serialLock.WaitOne();
            try
	        {
                myseshdata.nodeid = (byte)nodeId;

                using (cmdResponseGeneric_t genericResponse = cmdPing(ref myseshdata))
                {
                    if (genericResponse.errorcode != errorcode_enum.errcode_none)
                        throwerror(genericResponse.errorcode);
                }
	        }
	        finally 
	        {
                serialLock.ReleaseMutex();
            }
	    }

	    public object doGetValue(sensorType thisSensorType, short nodeId, short sensorId)
	    {
	        throw new NotImplementedException();
	    }

	    public bool doGetGenericDigitalIn(short nodeId, short sensorId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSetGenericOut(short nodeId, short toThis, short sensorId)
	    {
	        throw new NotImplementedException();
	    }

	    sensorType ITransmitter.doGetSensorType(short nodeId, short sensorId)
	    {
	        return doGetSensorType(nodeId, sensorId);
	    }

	    public sensorType doGetSensorType(short nodeId, short sensorId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSetPWMSpeed(short nodeId, short speed, short sensorId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSetNodeP(short nodeId, byte[] newP)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSetNodeId(short nodeId, short newNodeId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doFlashReRead(short nodeId)
	    {
	        throw new NotImplementedException();
	    }

	    public void doSetNodeKey(short nodeId, byte[] key)
	    {
	        throw new NotImplementedException();
	    }
	}
}

