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

	appConfig_t myseshdata;

}
