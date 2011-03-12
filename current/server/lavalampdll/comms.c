
#include "comms.h"
#include "protocol.h"

#include <stdio.h>


BOOL isPortOpen(appConfig_t* myappconfig)
{
	return (!(INVALID_HANDLE_VALUE == myappconfig->hnd));
}

void syncNetwork(appConfig_t* myconfig)
{
	// sync up with the transmitter

	int syncbytes=8;
	long s;
	char pkt = (char)0xAA;
	BOOL timeout=FALSE;
	char dummy=(char)0x8E;

	if (myconfig->assume_synced)
		return;

	if (myconfig->verbose>0) printf("syncing..");

	// send an initial non-sync character
	s = sendwithtimeout(myconfig, (char*)&".", 1, &timeout);
	if (timeout) printf("packet timed out!..");
	if (s==0) printf("packet send failed!..");

	// Since we only synch to the transmitter, over a wire, we just send 8 0xAA characters.
	while(syncbytes-->0)
	{
		s = sendwithtimeout(myconfig, &pkt, 1, &timeout);
		if (timeout) printf("packet timed out!..");
		if (s==0) printf("packet send failed!..");

		if (myconfig->verbose>0) printf("%d..", syncbytes);
	}
	if (myconfig->verbose>0) printf("OK.\n");
}


