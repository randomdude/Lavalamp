using System;
using System.Text;

namespace netGui
{
	public class transmitterDriver : pinvokeWrapper, ITransmitter
	{
        appConfig_t myseshdata;

        /// <summary>
        /// We lock on this before performing any interop.
        /// </summary>
	    private readonly object serialLock = new Object();
			
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
            lock (serialLock)
            {
                myseshdata.injectFaultInvalidResponse = newVal;
            }
	    }

	    public void doSyncNetwork()
	    {
            lock (serialLock)
            {
                syncNetwork(ref myseshdata);
            }
	    }

	    public void Dispose()
	    {
            // TODO: This seems a lot of work to do in the destructor (and on the GC thread). Is this the
            // best way of doing things?
            if (portOpen())
            {
                // TODO: it _is_ safe to lock on this in the GC thread, right? I think so.
                lock (serialLock)
                {
                    closePort(ref myseshdata);
                }
            }
	    }

	    public bool portOpen()
	    {
            lock (serialLock)
            {
                return isPortOpen(myseshdata);
            }
	    }

	    public string doIdentify(short nodeId)
	    {
            lock (serialLock)
            {
                myseshdata.nodeid = (byte) nodeId;

                using (cmdResponseIdentify_t genericResponse = cmdIdentify(ref myseshdata))
                {
                    if (genericResponse.errorcode != errorcode_enum.errcode_none)
                        throwerror(genericResponse.errorcode);

                    ASCIIEncoding enc = new ASCIIEncoding();
                    string convertedWithNulls = enc.GetString(genericResponse.response);

                    // Careful here - the string returned by enc.GetString may contain nulls. Make sure
                    // we return a null-less string.
                    if (convertedWithNulls.Contains("\0"))
                        return (convertedWithNulls.Split(new[] { '\0' })[0]);
                    else
                        return convertedWithNulls;
                }
            }
	    }

	    public void doPing(short nodeId)
	    {
            lock (serialLock)
            {
                myseshdata.nodeid = (byte) nodeId;

                using (cmdResponseGeneric_t genericResponse = cmdPing(ref myseshdata))
                {
                    if (genericResponse.errorcode != errorcode_enum.errcode_none)
                        throwerror(genericResponse.errorcode);
                }
            }
	    }

        public short doGetSensorCount(short nodeId)
        {
            lock (serialLock)
            {
                myseshdata.nodeid = (byte)nodeId;

                using (cmdResponseGeneric_t genericResponse = cmdCountSensors(ref myseshdata))
                {
                    if (genericResponse.errorcode != errorcode_enum.errcode_none)
                        throwerror(genericResponse.errorcode);

                    return (short) genericResponse.response;
                }
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

	    public sensorType doGetSensorType(short nodeId, short sensorId)
	    {
            lock (serialLock)
            {
                myseshdata.nodeid = (byte)nodeId;

                using (cmdResponseGetSensorType_t response = cmdGetSensorType(myseshdata))
                {
                    if (response.errorcode != errorcode_enum.errcode_none)
                        throwerror(response.errorcode);

                    return new sensorType( response.type );
                }
            }
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
