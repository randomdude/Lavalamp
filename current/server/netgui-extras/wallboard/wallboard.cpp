#include "stdafx.h"

#include "wallboard.h"

HANDLE connectWallboard(const char * port)
{
	DCB mydcb;
	HANDLE hndport;
	size_t portLen = strlen(port) + 1;
	wchar_t* wcport = new wchar_t[portLen];
	size_t charactersEncoded;
	COMMTIMEOUTS timeout;
	mbstowcs_s(&charactersEncoded, wcport,portLen, port, portLen -1 );
	assert(charactersEncoded == portLen);
	// Open serial port
	hndport = CreateFile(wcport, GENERIC_READ | GENERIC_WRITE ,NULL, NULL,OPEN_EXISTING, 0, NULL);
	if (hndport==INVALID_HANDLE_VALUE) 
		return 0;
	// set port config - 9600, 7e1
	if (!GetCommState(hndport, &mydcb)) 
		return 0;
	mydcb.BaudRate = 9600;
	mydcb.ByteSize = 7;
	mydcb.Parity = EVENPARITY;
	mydcb.StopBits = ONESTOPBIT	;
	if (!SetCommState(hndport, &mydcb)) 
		return 0;
	//set port timeout
	timeout.ReadIntervalTimeout = 500;
	timeout.ReadTotalTimeoutMultiplier = 1000;
	timeout.ReadTotalTimeoutConstant = 1000;
	timeout.WriteTotalTimeoutMultiplier = 10;
	timeout.WriteTotalTimeoutConstant = 100;
	if (!SetCommTimeouts(hndport,&timeout))
		return 0;

	return hndport;
}

void closeWallboard(HANDLE hndport)
{
	CloseHandle(hndport);
}

wallboardErrorState checkWallboardErrorState(HANDLE hndport)
{
	// Protocol format - 
	// 		   <NUL> <NUL> <NUL> <NUL> <NUL> 
	// SOH character - 0x01
	// Type code     - 'Z'  (all types of wallboard)
	// Address field - "??" (all stations)
	// Start of Text - 0x02
	// Command code  - 'F' special function
	// Special function code (* get - error state )
	// EOT	         - 0x04
	char message[255]={ 00,00,00,00,00,	\
						01,				\
						'Z',			\
						'?','?',		\
						 02,		\
						'F',			\
						'*',
						0x04	};
	bool completePacket = false;
	DWORD done;
	WriteFile(hndport, message, 255, &done, NULL);

	if(!readPacket(hndport,message))
		return TimeOutError;

	if (message[6] != '*')
		OutputDebugStringA(message);

	if (message[7] & IllegalCommand)
		return IllegalCommand;
	if (message[7] & ChecksumError) 
		return ChecksumError;
	if (message[7] & BufferOverflow) 
		return BufferOverflow;
	if (message[7] & SerialTimeout) 
		return SerialTimeout;
	if (message[7]  & BaudrateError)
		return BaudrateError;
	if (message[7]  & ParityError)
		return ParityError;

	return None;

}

void resetWallboard(HANDLE hndPort)
{
	// Protocol format - 
	// 		   <NUL> <NUL> <NUL> <NUL> <NUL> 
	// SOH character - 0x01
	// Type code     - 'Z'  (all types of wallboard)
	// Address field - "??" (all stations)
	// Start of Text - 0x02
	// Command code  - 'F'  
	// Special function code ( , reset )
	// EOT	         - 0x04
	char message[255]={ 00,00,00,00,00,	\
						01,				\
						'Z',			\
						'?','?',		\
						 02,		\
						'F',			\
						',',
						0x04	};

	DWORD done;
	WriteFile(hndPort, message, 255, &done, NULL);
}

// speed is 0-4
// charset is 0-5
void sendWallMessage(HANDLE hndport, const char* sayit, char pos, char style,  char col, unsigned char special,  bool dumppkt)
/* displays message in sayit on all wallboards on port opened as port. Set special to 0xff for no special mode. 
   Note that speed and charset no longer work. */
{
	unsigned long done;
	long msglen;

	// first, send a string requesting that 'file A' is the maximum length (see protoacd.doc appendix C).
	// This only really needs doing once, but we do it on every message because I'm lazy.
#define textFileLenPacketLen 35
	char textFileLenPacket[textFileLenPacketLen] = 
	{
		00,00,00,00,00,	// 5 nulls are required by the message center to lock on to the baud rate
		0x01,	//	"Start of Header" character
		0x5a, 0x30, 0x30,	// "Z00" -  Unit Type Code/Address Field
		0x02,	// "Start of Text" character
		'E',	//Write Special Functions Command Code
		'$',	// Special Functions label for Memory Configuration (directory)
		'A', 	// File Label
		'A',	//TEXT file type
		'U',	//"Unlocked" keyboard status
		'0','4','0','0',	// TEXT file size in bytes (hexadecimal or 1024 decimal) 
		'F','F',			// TEXT file run start time ("FF" represents "always")
		'0','0',			// TEXT file run stop time (ignored when start time is "always")
		'A',				// File Label
		'B',				// STRING file type
		'L',				// "Locked" keyboard status
		'0','0','2','0',	// STRING file size in bytes (hexadecimal or 32 decimal)
		' ',' ',			// ignored
		' ',' ',			// ignored
		0x04				// "End of Transmission" character
	};
	BOOL s = WriteFile(hndport, textFileLenPacket, textFileLenPacketLen, &done, NULL);
	if (!s || done != textFileLenPacketLen)
	{
		// No error handling here.. oops.
		return;
	}


	#define MSG_POS 13
	#define MSG_STYLE 14
	long MSG_TEXT = 15;		// this changes if we're writing a SPECIAL to allow for the extra byte

	// Protocol format - 
	// 		   <NUL> <NUL> <NUL> <NUL> <NUL> 
	// SOH character - 0x01
	// Type code     - 'Z'  (all types of wallboard)
	// Address field - "??" (all stations)
	// Start of Text - 0x02
	// Command code  - 'A'  (Write text file)
	// Start of text file - 'A' 
	// File label	 - 'A' (the default file displayed on clear)
	// Escape code   - 0x1B
	// Display position
	// Mode code
	// (optional byte of 'special' set only when mode = 'n')
	// ASCII data ....
	// EOT	         - 0x04
#define msgPreludeLen 13
	char* message = (char*)malloc(50 + strlen(sayit));
	char msgPrelude[msgPreludeLen]={ 00,00,00,00,00,	\
									01,				\
									'Z',			\
									'?','?',		\
									 02,			\
									'A',			\
									'A',
									0x1b	};
	memcpy(message, msgPrelude, msgPreludeLen);

	message[MSG_POS]=pos;												// position specifier
	if (style>0) 
		message[MSG_STYLE]=style; 
	else 
		message[MSG_STYLE]=0x62;		

	if (special!=0xff)	
	{
		// theres a special specifier. set style to 'n' and enter the specifier, moving the rest of the packet along one
		message[MSG_STYLE  ]= 'n';
		message[MSG_STYLE+1]= special;
		++MSG_TEXT;
		
		if (special > 0x39) // there is a special graphic an additional mode is required use the selected mode 
		{
			message[MSG_STYLE + 2] = 0x1b;
			message[MSG_STYLE + 3] = pos;
			message[MSG_STYLE + 4] = style;
			MSG_TEXT += 3;
		}
	}

	msglen = strlen(sayit) + MSG_TEXT + 2 + 1;				// find total packet length - before text, text length, extra space for escape codes, and 1 EOT char
	sprintf_s(&message[MSG_TEXT], msglen, "..%s%c",  sayit, 0x04);	// leave space for escape codes which set charset, colour, and speed. Also add EOT char
	message[MSG_TEXT+0]=0x1C;						// CTRL-\, set colour
	message[MSG_TEXT+1]=col;

	if (dumppkt)
	{
		for (int n=0; n<msglen; n++)
		{
			printf(" %d.. '%c' 0x%x\n", n, message[n],message[n] );
		}
	}

	WriteFile(hndport, message, msglen, &done, NULL);
} 

bool readPacket(HANDLE hndport, char* packet)
{
	const DWORD TIMEOUT = 500;
	bool completePacket = false;;
	int syncBytes = 0;
	int dataRead = 0;
	DWORD totalDone = 0;
	while(!completePacket)
	{
		DWORD done;
		char read[20];
		if(!ReadFile(hndport, read, 20, &done, NULL) || done == 0)
			return false;
		totalDone += done;
		for(int i = 0; i <20;i++)
		{
			if (read[i] == 0)
			{
				++syncBytes;
			}

			if (read[i] != 0 && syncBytes > 20)
			{
				// 20 sync bytes minimum are required
				if (syncBytes < 20)
					throw new socketException("serial com is out of sync");
				packet[dataRead++] = read[i];
				//end of packet
				if (read[i] = 4)
					completePacket = true;
			}
			if (totalDone > TIMEOUT)
				return false;
		}
	}
	return true;
}
