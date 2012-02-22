#pragma once
#include <stdexcept>

class socketException
{
	public:
		socketException(const char* socket);
		~socketException(void);
		const char* what() const;
	private:
		const char* _what;
};

