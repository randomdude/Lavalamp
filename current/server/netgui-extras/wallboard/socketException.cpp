#include "StdAfx.h"
#include "socketException.h"


socketException::socketException(const char* socket)
{
	if (socket == NULL)
		this->_what = "Could not open Socket";
	else 
		this->_what = socket;
}

const char* socketException::what() const
{
	return _what;
}

socketException::~socketException(void)
{
}
