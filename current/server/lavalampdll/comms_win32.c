#include "comms.h"
#include "protocol.h"
#include <stdio.h>

long sendwithtimeout( appConfig_t* myconfig, char* data, long datalen, BOOL* didTimeout)
{
	unsigned long sentbytes;
	long s=0;
	int n=0;
	long elapsed = 0;
	OVERLAPPED myoverlap;
	didTimeout[0]=FALSE;

	memset(&myoverlap, 0x00, sizeof(OVERLAPPED));
	myoverlap.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
	if (0 == myoverlap.hEvent)
	{
		printf("Error creating overlap event - %d, GLE %d\n", (long)myoverlap.hEvent, (long)GetLastError());
		return FALSE;
	}

	if (myconfig->verbose>2)
	{
		printf("sending raw bytes ");
		for (n=0; n<datalen; n++)
			printf(" 0x%02X ", (unsigned char)(data[n]));
		printf("\n");
	}
	s = WriteFile(myconfig->hnd, data, datalen , &sentbytes, &myoverlap);

	if (s==0)
	{
		long gle = GetLastError();
		if (gle==ERROR_IO_PENDING)
		{
			if (myconfig->verbose>2) printf("waiting on asynch send\n");

			s=WaitForSingleObject(myoverlap.hEvent , INFINITE);

			if (0 != s )
			{
				if (myconfig->verbose>2) printf("timeout writing bytes - WaitForSingleObject %d, GLE %d\n", s, GetLastError() );
				didTimeout[0]=TRUE;
				CloseHandle(myoverlap.hEvent);
				return FALSE;
			}
			if (!GetOverlappedResult(myconfig->hnd, &myoverlap, &sentbytes, FALSE))
			{
				printf("GetOverlappedResult failed\n");
				CloseHandle(myoverlap.hEvent);
				return FALSE;
			}
		}
		else
		{
			if (myconfig->verbose>1) printf("Unable write data - WriteFile returned %d, gle %d\n", s, gle);
			CloseHandle(myoverlap.hEvent);
			return FALSE;
		}
	}
	if (myconfig->verbose>2) printf("Asynch IO complete, %d bytes sent\n", sentbytes);

	CloseHandle(myoverlap.hEvent);

	return TRUE;
}

long readwithtimeout( appConfig_t* myconfig, char* data, long datalen,  BOOL* didTimeout )
{
	long sofar=0;
	unsigned long sentbytes=0;
	long s=0;
	int n=0;
	long elapsed = 0;
	OVERLAPPED myoverlap;
	didTimeout[0]=FALSE;

	memset(&myoverlap, 0x00, sizeof(OVERLAPPED));
	myoverlap.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
  	if (0 == myoverlap.hEvent)
	{
		printf("Error creating overlap event - %d, GLE %d\n", (long)myoverlap.hEvent, GetLastError());
		return FALSE;
	}

	while(sofar<datalen)
	{
		s = ReadFile(myconfig->hnd, &data[sofar], datalen - sofar , &sentbytes, &myoverlap);

		if (s==FALSE)
		{
			long gle = GetLastError();
			if (gle==ERROR_IO_PENDING)
			{
				if (myconfig->verbose>2) printf("waiting on asynch read\n");

				if (0!=WaitForSingleObject(myoverlap.hEvent , 5000))
				{
					if (myconfig->verbose>2) printf("timeout read bytes - GLE %d\n", GetLastError() );
					didTimeout[0]=TRUE;
					CloseHandle(myoverlap.hEvent);
					return FALSE;
				}

				if (!GetOverlappedResult(myconfig->hnd, &myoverlap, &sentbytes, FALSE))
				{
					printf("GetOverlappedResult failed\n");
					CloseHandle(myoverlap.hEvent);
					return FALSE;
				}

				// At this point, the async read has completed.
				sofar+=sentbytes;
			} else {
				// Getlasterror returned something meaningful
				if (myconfig->verbose>1) printf("Unable to read data - ReadFile returned %d, GLE %d\n", s, gle);
				CloseHandle(myoverlap.hEvent);
				return FALSE;
			}
		} else {
			// No error occured in ReadFile.
			sofar+=sentbytes;
		}
		if (myconfig->verbose>1) printf("read %d bytes. %d total recieved of %d.\n",sentbytes, sofar, datalen);
	}
	if (myconfig->verbose>2) printf("Asynch IO complete, %d bytes read\n", sentbytes);

	if (myconfig->verbose>2)
	{
		printf("gotten raw bytes ");
		for (n=0; n<datalen; n++)
			printf(" 0x%02X ", (unsigned char)(data[n]));
		printf("\n");
	}

	CloseHandle(myoverlap.hEvent);
	return TRUE;
}



BOOL __cdecl initPort(appConfig_t* myconfig)
{
	DCB mydcb;

	LPSTR portpath = (LPSTR)malloc(strlen(myconfig->portname) + 10);
	wsprintf(portpath, "\\\\.\\%s", myconfig->portname);
	myconfig->hnd = CreateFile(portpath, GENERIC_READ|GENERIC_WRITE, (DWORD)NULL, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, (HANDLE)NULL);
	free (portpath);

	if (INVALID_HANDLE_VALUE == myconfig->hnd)
	{
		printf("Unable to open port '%s': error code %d\n", myconfig->portname, GetLastError() );
		return FALSE ;
	}

	// Only set the serial parameters if we are using a serial port, since we can also communicate over named 
	// pipes or suchlike.
	if (_strnicmp(myconfig->portname, "pipe\\", 5) == 0)
		myconfig->isSerialPort = FALSE;
	else
		myconfig->isSerialPort = TRUE;

	if (myconfig->isSerialPort)
	{
		if (!GetCommState(myconfig->hnd,&mydcb))
		{
			printf("Unable to getCommState: GLE returned 0x%lx\n", GetLastError());
			CloseHandle(myconfig->hnd);
			return FALSE ;
		}		

		mydcb.BaudRate = 9600;
		mydcb.fBinary = TRUE;
		mydcb.fParity = FALSE;
		mydcb.fOutxCtsFlow = FALSE;
		mydcb.fOutxDsrFlow = FALSE;
		mydcb.fDtrControl = DTR_CONTROL_DISABLE	;
		mydcb.fDsrSensitivity = FALSE;
		mydcb.fOutX = FALSE;
		mydcb.fInX = FALSE;
		mydcb.fNull = FALSE;
		mydcb.fRtsControl = RTS_CONTROL_DISABLE ;
		mydcb.fAbortOnError = FALSE;
		mydcb.ByteSize = 8;
		mydcb.Parity = NOPARITY;
		mydcb.StopBits = ONESTOPBIT;

		if (!SetCommState(myconfig->hnd, &mydcb))
		{
			printf("Unable to setCommState: GLE returned 0x%lx\n", GetLastError());
			CloseHandle(myconfig->hnd);
			return FALSE ;
		}
	}

	return(TRUE);
}

void closePort(appConfig_t* myappconfig)
{
	CloseHandle(myappconfig->hnd);
	myappconfig->hnd = INVALID_HANDLE_VALUE;
}
