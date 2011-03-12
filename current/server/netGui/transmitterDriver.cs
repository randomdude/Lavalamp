using System;
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
		private enum errorcode_enum { errcode_none,	errcode_timeout,errcode_crypto,	errcode_internal,errcode_portstate, 
									  errcode_sensor_not_found=0x20, errcode_sensor_wrong_type };
		
		private struct cmdResponseGeneric_t
		{
			errorcode_enum errorcode;
			Int32 response;
			Int32 totaltime;
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

		private struct appConfig_t
		{
			byte nodeid;
			byte sensorid;
			CharPtr portname;
			bool useEncryption;
			UInt32[] key;
			int verbose;
			HANDLE hnd;
			bool machineoutput;
			Int32 com_timeout;
			bool assume_synced;
			Int32 retries;
			bool isSerialPort;
			bool injectFaultInvalidResponse;
		}
		
		[DllImport("lavalampdll.dll",EntryPoint="initPort")]
		private static extern bool initPort(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="closePort")]
		private static extern void closePort(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="closePort")]
		private static extern bool isPortOpen(appConfig_t config);

		[DllImport("lavalampdll.dll",EntryPoint="syncNetwork")]
		private static extern void syncNetwork(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdPing")]
		private static extern cmdResponseGeneric_t  cmdPing(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdReload")]
		private static extern cmdResponseGeneric_t*  cmdReload(appConfig_t config);
		
		[DllImport("lavalampdll.dll",EntryPoint="cmdIdentify")]
		private static extern  cmdResponseIdentify_t cmdIdentify(rppConfig_t config);
		
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
		
		#endregion
		
		appConfig_t appConfigData;
			
		public transmitterDriver (string strPortName, bool useEncryption, byte[] key)
		{

		}
		
	}
}

