using System;

namespace netGui
{
    public class badPortException : Exception { }
    public class cantOpenPortException : Exception { }
    public class cantClosePortException : Exception { }
    public class InternalErrorException : Exception { }
    public class commsException : Exception { }
    public class commsCryptoException : commsException { };
    public class commsTimeoutException : commsException { };
    public class commsPortStateException : commsException { };
    public class commsInternalException : commsException { };
    public class userCancelledException : Exception { }
    public class cantHandleSensorException : Exception { }
    public class sensorException : Exception { }
    public class sensorNotFoundException : sensorException { };
    public class sensorWrongTypeException : sensorException { };
}
