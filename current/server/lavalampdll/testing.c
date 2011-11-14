#include "protocol.h"
#include "comms.h"
#include "commands.h"

#include <stdio.h>

// Create a new appConfig_t and fill it wtih some test values. This can be called from the other side of our
// p/invoke to verify that the p/invoke is operating correctly.
appConfig_t* getTestConfig()
{
	appConfig_t* toRet;

	toRet = (appConfig_t*)malloc(sizeof(appConfig_t));

	toRet->nodeid = 0x01;
	toRet->sensorid = 0x02;
	toRet->portname = "some port";
	toRet->useEncryption = TRUE;
	toRet->key[0] = 0x01020304;
	toRet->key[1] = 0x05060708;
	toRet->key[2] = 0x090a0b0c;
	toRet->key[3] = 0x0d0e0f00;
	toRet->verbose = 0x22;
	toRet->hnd = (HANDLE)0x10111213;
	toRet->machineoutput = FALSE;
	toRet->com_timeout = 0x14;
	toRet->assume_synced = TRUE;
	toRet->retries = 0x15;
	toRet->isSerialPort = FALSE;
	toRet->injectFaultInvalidResponse = TRUE;

	return toRet;
}

// Cause the network to be out of sync with the transmitter by sending a few garbage bytes.
// return TRUE on success.
BOOL injectFaultDesync(appConfig_t* myConfig)
{
	BOOL timeout;
	long s = sendwithtimeout(myConfig, (char*)&".", 1, &timeout);

	if (s == 0 || timeout)
	{
		printf("failed to desync network..");
		return FALSE;
	}

	return TRUE;
}