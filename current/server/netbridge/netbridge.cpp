// This is the main DLL file.

#include "stdafx.h"
#include "netbridge.h"

namespace netbridge
{
	public enum class sensorTypeEnum
	{
		unknown = 0x00,
		generic_digital_in = 0x01,
		generic_digital_out = 0x02,
		pwm_out = 0x03,
		triac_out = 0x04
	};

	public ref class sensorType
	{
	public:
		System::String ^ FriendlyType;
		Int16 id;
		sensorTypeEnum enumeratedType;
	};

	public ref class transmitterDriver : System::IDisposable
	{
		private:
			System::Threading::Mutex ^ serialLock;
			void throwerror(long errcode)
			{
				switch( errcode )
				{
					case 0x01:
						throw gcnew commsTimeoutException();
						break;
					case 0x02:
						throw gcnew commsCryptoException();
						break;
					case 0x03:
						throw gcnew commsInternalException();
						break;
					case 0x04:
						throw gcnew commsPortStateException();
						break;

					case 0x20:
						throw gcnew sensorNotFoundException();
						break;
					case 0x21:
						throw gcnew sensorWrongTypeException();
						break;

					default:
						throw gcnew InternalErrorException();
				}
			}

		public: 

			transmitterDriver(System::String ^strPortName, bool useEncryption, array<System::Byte> ^ key)
			{
				if (strPortName->IsNullOrEmpty(strPortName))
					throw gcnew badPortException;

				// Convert String into a char* so we can send it to the driver
				System::Text::ASCIIEncoding ^myEncoding = gcnew System::Text::ASCIIEncoding();
				int byteCount = myEncoding->GetByteCount(strPortName) ;

				myseshdata.portname = (char*)malloc(byteCount+1);

				// Getting data from a System::String is tricky. Is there a better way to do it than this?
				// ERK, this is coming straight of a unicode char in to an ascii! fixme!
				for( int i = 0; i< byteCount; i++)
					myseshdata.portname[i] =  (char)strPortName->ToCharArray()[i];
				myseshdata.portname[byteCount] = 0;

				// Pass through encryption setting
				myseshdata.useEncryption = useEncryption;

				// Now, open the port.
				if (!initPort(&myseshdata))
					throw gcnew cantOpenPortException();

				// we lock the port at this level.
				serialLock = gcnew System::Threading::Mutex();

				// Save the key
				if (useEncryption)
				{
					if (key->Length != 16) throw gcnew ArgumentException("Key is of an invalid length");

					myseshdata.key[0] =	(key[ 3] <<  0 ) |
										(key[ 2] <<  8 ) | 
										(key[ 1] << 16 ) | 
										(key[ 0] << 24 )   ;
					myseshdata.key[1] =	(key[ 7] <<  0 ) |
										(key[ 6] <<  8 ) | 
										(key[ 5] << 16 ) | 
										(key[ 4] << 24 )   ;
					myseshdata.key[2] =	(key[11] <<  0 ) |
										(key[10] <<  8 ) | 
										(key[ 9] << 16 ) | 
										(key[ 8] << 24 )   ;
					myseshdata.key[3] =	(key[15] <<  0 ) |
										(key[14] <<  8 ) | 
										(key[13] << 16 ) | 
										(key[12] << 24 )   ;
				}
				else
				{
					memset(myseshdata.key, 0x00, 16 );
				}

				myseshdata.assume_synced = FALSE;
				myseshdata.verbose = 0;
				myseshdata.machineoutput = FALSE;
				myseshdata.com_timeout = 5;
				myseshdata.retries = 1;
			}

			bool portOpen()
			{
				if ( TRUE == isPortOpen(&myseshdata) )
					return true;
				else
					return false;
			}

			void setInjectFaultInvalidResponse(System::Boolean ^ newVal)
			{
				myseshdata.injectFaultInvalidResponse = ((System::Boolean)newVal);
			}

			~transmitterDriver()
			{
				this->!transmitterDriver();
			}

			!transmitterDriver()
			{
				if (portOpen())
				{
					closePort(&myseshdata);
				}
			}


			System::String ^ doIdentify(Int16 nodeId)
			{
				serialLock->WaitOne();
				cmdResponseIdentify_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdIdentify(&myseshdata);

				if (genericResponse->errorcode==errcode_none)
				{
					genericResponse->response[0x1f]=0;
					System::String ^ toreturn = gcnew String(genericResponse->response);
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return toreturn;
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
				return gcnew System::String("");	// Dummy return value - code can never get to this point
			}

			Int16 doGetSensorCount(Int16 nodeId)
			{
				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;

				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse=cmdCountSensors(&myseshdata);

				if (genericResponse->errorcode==errcode_none)
				{
					long sensorCount = genericResponse->response;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return (Int16)sensorCount;
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
				return 0;	// Dummy return value - code can never get to this point
			}

			void doPing(Int16 nodeId)
			{
				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdPing(&myseshdata);

				if (genericResponse->errorcode==errcode_none)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return;
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}

			void doSyncNetwork()
			{
				serialLock->WaitOne();
				syncNetwork(&myseshdata);
				serialLock->ReleaseMutex();
			}

			// This observes a sensorType and uses the relevant function to obtain a weakly-typed result.
			Object ^ doGetValue(sensorType ^ thisSensorType,  Int16 nodeId, Int16 sensorId)
			{ 
				switch(thisSensorType->enumeratedType )
				{
				case sensorTypeEnum::generic_digital_in:
					return doGetGenericDigitalIn(nodeId, sensorId);
					break;
				default:
					throw gcnew cantHandleSensorException();
				}
			}

			bool doGetGenericDigitalIn(Int16 nodeId, Int16 sensorId)
			{
				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				myseshdata.sensorid = (unsigned char)sensorId;
				genericResponse = cmdGetGenericDigitalSensor(&myseshdata);

				if (genericResponse->errorcode==errcode_none)
				{
					bool toReturn;
					if (genericResponse->response == 0x00)
						toReturn = false;
					else
						toReturn = true;

					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return toReturn;
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
				return false;	// Dummy return value - code can never get to this point
			}

			void doSetGenericOut(Int16 nodeId, System::Int16 toThis, Int16 sensorId)
			{
				if (nodeId>255 || toThis > 255 || sensorId > 255)
					throw gcnew ArgumentException();

				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				myseshdata.sensorid = (unsigned char)sensorId;
				genericResponse = cmdSetGenericDigitalSensor(&myseshdata, (unsigned char)toThis);

				if (genericResponse->errorcode==errcode_none)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}
			sensorType ^ doGetSensorType(Int16 nodeId, Int16 sensorId)
			{
				serialLock->WaitOne();
				cmdResponseGetSensorType_t* sensorTypeResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				myseshdata.sensorid = (unsigned char)sensorId;
				sensorTypeResponse = cmdGetSensorType(&myseshdata);

				if (sensorTypeResponse->errorcode==errcode_none)
				{
					sensorType ^ toReturn = gcnew sensorType();
					toReturn->id = (short)sensorTypeResponse->type;
					toReturn->FriendlyType = gcnew System::String(sensorTypeResponse->FriendlyType );
					// Todo: move this switch to somewhere in the driver
					// TODO: Remove this switch entirely! It's just another hurdle for people that add sensors.
					switch (sensorTypeResponse->type)
					{
					case 0x01:
						toReturn->enumeratedType = sensorTypeEnum::generic_digital_in;
						break;
					case 0x02:
						toReturn->enumeratedType = sensorTypeEnum::generic_digital_out;
						break;
					case 0x03:
						toReturn->enumeratedType = sensorTypeEnum::pwm_out;
						break;
					case 0x04:
						toReturn->enumeratedType = sensorTypeEnum::triac_out;
						break;
					default:
						toReturn->enumeratedType = sensorTypeEnum::unknown;
						break;
					}

					cmd_free(sensorTypeResponse);
					serialLock->ReleaseMutex();
					return toReturn;
				}
				else
				{
					long errcode = sensorTypeResponse->errorcode;
					cmd_free(sensorTypeResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}

			void doSetPWMSpeed(Int16 nodeId, Int16 speed, Int16 sensorId)
			{
				if (nodeId>255 || speed > 255 || sensorId > 255)
					throw gcnew ArgumentException();

				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				myseshdata.sensorid = (unsigned char)sensorId;
				genericResponse = cmdSetSensorFadeSpeed(&myseshdata, (unsigned char)speed );

				if (genericResponse->errorcode==errcode_none)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}
			void doSetNodeP(Int16 nodeId,  array<System::Byte> ^ newP)
			{
				if (nodeId > 255 || newP->Length != 3)
					throw gcnew ArgumentException();

				// We send two commands to set the high and low end of P.
				serialLock->WaitOne();

				cmdResponseGeneric_t* genericResponse;	
				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdSetP(&myseshdata, newP[0], newP[1], false );
				if (genericResponse->errorcode!=errcode_crypto)	// We expect this to fail.
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throw gcnew InternalErrorException();
				}

				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdSetP(&myseshdata, newP[2], 0x00, true );
				if (genericResponse->errorcode==errcode_crypto)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return;
				}
				else
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throw gcnew InternalErrorException();
				}

			}
			void doSetNodeId(Int16 nodeId, Int16 newNodeId)
			{
				if (nodeId>255 || newNodeId > 255 )
					throw gcnew ArgumentException();

				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdSetNodeId(&myseshdata, (unsigned char)newNodeId );

				if (genericResponse->errorcode==errcode_none)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}
			void doFlashReRead(Int16 nodeId)
			{
				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				myseshdata.nodeid = (unsigned char)nodeId;
				genericResponse = cmdReload(&myseshdata);

				if (genericResponse->errorcode==errcode_none)
				{
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					return;
				}
				else
				{
					long errcode = genericResponse->errorcode;
					cmd_free(genericResponse);
					serialLock->ReleaseMutex();
					throwerror(errcode);
				}
			}

			void doSetNodeKey(Int16 nodeId, array<System::Byte> ^ key)
			{
				if (nodeId>255 || key->Length != 16 )
					throw gcnew ArgumentException();

				serialLock->WaitOne();
				cmdResponseGeneric_t* genericResponse;	
	
				for(int n=0; n<key->Length; n++)
				{
					myseshdata.nodeid = (unsigned char)nodeId;
					genericResponse = cmdSetNodeKeyByte(&myseshdata, n, (unsigned char)key[n] );

					if (genericResponse->errorcode!=errcode_none)
					{
						long errcode = genericResponse->errorcode;
						cmd_free(genericResponse);
						serialLock->ReleaseMutex();
						throwerror(errcode);
					}
				}

				cmd_free(genericResponse);
				serialLock->ReleaseMutex();
			}
	};
}