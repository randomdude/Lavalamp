
#include "commands.h"
#include <stdio.h>
#include "../shared/lavalamp.h"

// Place custom 'command' calls in here. 

validated_datapkt_t* cmd_generic(appconfig_t* myconfig)
{
	datapkt_t tosend;
	datapkt_t* response=(datapkt_t*)malloc(sizeof(datapkt_t));

	// assemble the request packet to send to the chip
	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_IDENT;
	tosend.byte7 = 0x00;
	tosend.byte8 = 0x00;

	// send it
	BOOL success = sendpacket(tosend, response, myconfig->hnd, myconfig->key, myconfig->verbose);

	validated_datapkt_t* toreturn=(validated_datapkt_t*)malloc(sizeof(validated_datapkt_t));

	if (success) // only copy it if it was a valid response
		memcpy(&(toreturn->response), response, sizeof(datapkt_t));

	toreturn->isvalid = success;
	return toreturn;
}

BOOL cmd_ping(appconfig_t* myconfig)
{
	if (myconfig->verbose>0) printf("Executing CMD_PING\n");
	datapkt_t tosend;
	datapkt_t response;

	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_PING;
	tosend.byte7 = 0x00;
	tosend.byte8 = 0x00;

	return sendpacket(tosend, &response, myconfig->hnd, myconfig->key, myconfig->verbose);
}

char* cmd_identify(appconfig_t* myconfig)
{
	if (myconfig->verbose>0) printf("Executing CMD_IDENT\n");
	datapkt_t tosend;
	datapkt_t response;
	char nodename[CMD_IDENTIFY_LONGEST_NODE_NAME +1];
	memset(nodename, 0x00, CMD_IDENTIFY_LONGEST_NODE_NAME +1);

	for (int pktsgot=0; pktsgot<CMD_IDENTIFY_LONGEST_NODE_NAME /4; pktsgot++)
	{
		// assemble the request packet to send to the chip
		setnewseq(&tosend);
		tosend.nodeid = myconfig->nodeid;
		tosend.byte6 = CMD_IDENT;
		tosend.byte7 = pktsgot;
		tosend.byte8 = 0x00;

		// send it
		BOOL success = sendpacket(tosend, &response, myconfig->hnd, myconfig->key, myconfig->verbose);

		if (!success)
		{
			return NULL;
		}
		memcpy(&nodename[pktsgot*4], &(response.nodeid), 4);
		if (response.nodeid==00 || response.byte6==00 || response.byte7==00 || response.byte8==00)
			break;
	}

	char* toreturn = (char*)malloc((pktsgot*4)+1);
	strcpy(toreturn,nodename);

	return toreturn;
}
