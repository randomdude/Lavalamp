// Winamp general purpose plug-in mini-SDK
// Copyright (C) 1997, Justin Frankel/Nullsoft

#include <windows.h>
#include <process.h>

#include "gen.h"
#include "resource.h"
#include "winampcmd.h"

int main() { return 0; }

BOOL WINAPI _DllMainCRTStartup(HANDLE hInst, ULONG ul_reason_for_call, LPVOID lpReserved)
{
	return TRUE;
}

void config();
void quit();
int init();
VOID WINAPI FileIOCompletionRoutine(DWORD  dwErrorCode,	// completion code 
									DWORD  dwNumberOfBytesTransfered,	// number of bytes transferred 
									LPOVERLAPPED  lpOverlapped 	// address of structure with I/O information  
								   );
DWORD WINAPI threadProc(LPVOID lpThreadParameter);
void doAction(unsigned char doThis);
BOOL initok;
HANDLE pipeHnd;
char* readBuffer;
#define READBUF_SIZE 100
OVERLAPPED overlapInfo;
DWORD threadHnd;

winampGeneralPurposePlugin plugin =
{
	GPPHDR_VER,
	"Lavalamp automation plugin",
	init,
	config,
	quit,
};
void config()
{
	// todo - use winamps hwnd
	MessageBox(NULL, "Nothing to configure!", "Error", MB_OK);
}

void quit()
{
	// todo - release any namedPipes and kill threads
}

int init()
{
	CreateThread(NULL, 0, &threadProc, NULL, 0, &threadHnd);

	return 0;
}

enum winampAction {
	winamp_start	=0x00, 
	winamp_previous	=0x01, 
	winamp_next		=0x02, 
	winamp_pause	=0x03, 
	winamp_stop		=0x04,
	winamp_play		=0x05 
};
 
DWORD WINAPI threadProc(LPVOID lpThreadParameter)
{
	long s;
	char* msg;
	long gle;
	unsigned char readBuffer;
	long bytesread;
	BOOL errorsoretry=FALSE;

	while (TRUE)
	{
		pipeHnd = CreateNamedPipe( "\\\\.\\pipe\\lavalamp winamp control", PIPE_ACCESS_DUPLEX , PIPE_TYPE_BYTE,  
			PIPE_UNLIMITED_INSTANCES, 100,100 , 0, NULL);
		if (!pipeHnd)
		{
			gle = GetLastError();
			if (!gle==997)	// ERROR_IO_PENDING
			{
				msg=(char*)LocalAlloc(LMEM_FIXED, 100);
				wsprintf(msg,  "Unable to create named pipe. GLE reported %d", gle );
				MessageBox(NULL, msg, "uh-oh", MB_OK);
				LocalFree(msg);
				errorsoretry=TRUE;
			}
		}

		if (!errorsoretry)
		{
			if (!ConnectNamedPipe(pipeHnd, NULL))	// this will block until a client connects
			{
				gle = GetLastError();
				if (!gle==997)	// ERROR_IO_PENDING
				{
					msg=(char*)LocalAlloc(LMEM_FIXED, 100);
					wsprintf(msg,  "Unable to connect named pipe. GLE reported %d", gle );
					MessageBox(NULL, msg, "uh-oh", MB_OK);
					LocalFree(msg);
					errorsoretry=TRUE;
				}
			}
		}

		if (!errorsoretry)
		{
			s=ReadFile(pipeHnd, &readBuffer, 1, &bytesread, NULL);
			if( (!s) || (bytesread!=1) )
			{
				gle=GetLastError();
				if (gle!=ERROR_PIPE_LISTENING)
				{
					msg=(char*)LocalAlloc(LMEM_FIXED, 100);
					wsprintf(msg,  "Unable to read from named pipe. Success %d, GLE %d, bytesRead %d.", s, gle , bytesread);
					MessageBox(NULL, msg, "uh-oh", MB_OK);
					LocalFree(msg);
					errorsoretry=TRUE;
				}
			}
		}

		if (!errorsoretry)
		{
			doAction(readBuffer);
		}

		CloseHandle(pipeHnd);
	}

	return 0;
}

void doAction(unsigned char doThis)
{
	HANDLE winamp=plugin.hwndParent;

	switch(doThis)
	{
		case winamp_previous:
			SendMessage(winamp, WM_COMMAND, WINAMP_BUTTON1, 0);
			break;
		case winamp_play:
			SendMessage(winamp, WM_COMMAND, WINAMP_BUTTON2, 0);
			break;
		case winamp_pause:
			SendMessage(winamp, WM_COMMAND, WINAMP_BUTTON3, 0);
			break;
		case winamp_stop:
			SendMessage(winamp, WM_COMMAND, WINAMP_BUTTON4, 0);
			break;
		case winamp_next:
			SendMessage(winamp, WM_COMMAND, WINAMP_BUTTON5, 0);
			break;
	}
}

__declspec( dllexport ) winampGeneralPurposePlugin * winampGetGeneralPurposePlugin()
{
	return &plugin;
}

