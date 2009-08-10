// netbridge.h

#pragma once

#include "../shared/lavalamp.h"

#using <mscorlib.dll>
using namespace System;

namespace netbridge 
{
	public ref struct badPortException			: public  System::Exception { };
    public ref struct cantOpenPortException		: public  System::Exception { };
	public ref struct cantClosePortException	: public  System::Exception { };
	public ref struct InternalErrorException	: public  System::Exception { };
	public ref struct commsException			: public  System::Exception { };
	public ref struct commsCryptoException		: public  commsException { };
	public ref struct commsTimeoutException		: public  commsException { };
	public ref struct commsPortStateException	: public  commsException { };
	public ref struct commsInternalException	: public  commsException { };
	public ref struct userCancelledException	: public  System::Exception { };
	public ref struct cantHandleSensorException : public  System::Exception { };
	public ref struct sensorException			: public  System::Exception { };
	public ref struct sensorNotFoundException	: public  sensorException { };
	public ref struct sensorWrongTypeException	: public  sensorException { };


	appConfig_t myseshdata;

}
