
#include "commands.h"
#include "protocol.h"
#include <stdio.h>
#include "../shared/lavalamp.h"

// Place custom 'command' calls in here. 

cmdResponseGeneric_t*  cmdGetGenericDigitalSensor(appConfig_t* myconfig)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* toreturn;

	// gets the state of a GENERIC_DIGITAL sensor.
	// returns state, any error code, and time of roundtrip.
	if (myconfig->verbose>1) 
		printf("Executing CMD_GET_SENSOR asking for sensor %d\n", myconfig->sensorid);
	else if (myconfig->verbose>0) 
		printf("Executing CMD_GET_SENSOR\n");

	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_GET_SENSOR;
	tosend.byte7 = 0x00;
	tosend.byte8 = myconfig->sensorid;

	packetresponse = sendPacketTimed(myconfig, tosend);

	toreturn = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (packetresponse->errorcode == errcode_none)
	{
		// if the node reported an error, the topmost bit of byte6  will
		// be set. Set the errorcode accordingly
		if ( (packetresponse->response.byte6 & 0x01 ) == 0x00 )
		{
			toreturn->errorcode = errcode_none;

			toreturn->response = 0;
			toreturn->response |= packetresponse->response.byte6;
			toreturn->response = toreturn->response << 8;
			toreturn->response |= packetresponse->response.byte7;
			toreturn->response = toreturn->response << 8;
			toreturn->response |= packetresponse->response.byte8;
		} else {
			if (packetresponse->response.byte8 == ERR_GET_SENSOR_NODE_NOT_FOUND)
				toreturn->errorcode = errcode_sensor_not_found;
			if (packetresponse->response.byte8 == ERR_GET_SENSOR_NODE_WRONG_TYPE)
				toreturn->errorcode = errcode_sensor_wrong_type;
		}
		toreturn->totaltime = packetresponse->totaltime;
	} else {
		// there was an error processing the packet
		toreturn->response  = 0x00;
		toreturn->totaltime = packetresponse->totaltime;
		toreturn->errorcode = packetresponse->errorcode;
	}


	return toreturn;
}

cmdResponseGeneric_t*  cmdSetGenericDigitalSensor(appConfig_t* myconfig, BOOL tothis)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* sensorcountresponse;

	// sets the state of a GENERIC_DIGITAL sensor.
	// returns an error code, and time of roundtrip.
	if (myconfig->verbose>1) 
		printf("Executing CMD_SET_SENSOR asking for sensor %d\n", myconfig->sensorid);
	else if (myconfig->verbose>0) 
		printf("Executing CMD_SET_SENSOR\n");

	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_SET_SENSOR;
	if (tothis)
		tosend.byte7 = 0x01;
	else
		tosend.byte7 = 0x00;
	tosend.byte8 = myconfig->sensorid;

	packetresponse = sendPacketTimed(myconfig, tosend);

	sensorcountresponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (packetresponse->errorcode == errcode_none)
	{
		// if the node reported an error, set the errorcode accordingly
		if ( packetresponse->response.byte8 == ERR_SET_SENSOR_NONE)
		{
			sensorcountresponse->errorcode = errcode_none;
		} else {
			if (packetresponse->response.byte8 == ERR_SET_SENSOR_NODE_NOT_FOUND)
				sensorcountresponse->errorcode = errcode_sensor_not_found;
			if (packetresponse->response.byte8 == ERR_SET_SENSOR_NODE_WRONG_TYPE)
				sensorcountresponse->errorcode = errcode_sensor_wrong_type;
		}
		sensorcountresponse->response  = 0x00;
		sensorcountresponse->totaltime = packetresponse->totaltime;
	} else {
		// there was an error handling the packet
		sensorcountresponse->response  = 0x00;
		sensorcountresponse->totaltime = packetresponse->totaltime;
		sensorcountresponse->errorcode = packetresponse->errorcode;
	}

	return sensorcountresponse;

}

cmdResponseGeneric_t*  cmdCountSensors(appConfig_t* myconfig)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* sensorcountresponse;
	
	// We send a CMD_GET_SENSOR to node 00
	if (myconfig->verbose>1) 
		printf("Executing CMD_GET_SENSOR asking for sensor 00\n");
	else if (myconfig->verbose>0) 
		printf("Executing CMD_GET_SENSOR \n");

	// Prepare our single packet
	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_GET_SENSOR;
	tosend.byte7 = 0x00;
	tosend.byte8 = 0x00;

	packetresponse = sendPacketTimed(myconfig, tosend);

	sensorcountresponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (packetresponse->errorcode==errcode_none)
	{
		sensorcountresponse->response = packetresponse->response.byte8;
		sensorcountresponse->totaltime= packetresponse->totaltime;
	}
	sensorcountresponse->errorcode = packetresponse->errorcode;
	return sensorcountresponse;
}

cmdResponseGeneric_t*  cmdPing(appConfig_t* myconfig)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* pingresponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (myconfig->verbose>0) printf("Executing cmdPing\n");

	// prepare packet
	setnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_PING;
	tosend.byte7 = 0x00;
	tosend.byte8 = 0x00;

	// send
	packetresponse = sendPacketTimed(myconfig, tosend);
	
	pingresponse->totaltime = packetresponse->totaltime;
	pingresponse->errorcode = packetresponse->errorcode;
	pingresponse->response = 0;
	pingresponse->response|=packetresponse->response.byte8;
	pingresponse->response=pingresponse->response<<8;
	pingresponse->response|=packetresponse->response.byte7;
	pingresponse->response=pingresponse->response<<8;
	pingresponse->response|=packetresponse->response.byte6;

	cmd_free(packetresponse);

	return pingresponse;
}

cmdResponseIdentify_t* cmdIdentify(appConfig_t* myconfig)
{
	generic_packet_response_t* packetresponse;
	cmdResponseIdentify_t* identresponse;
	datapkt_t tosend;
	int pktsgot;

	if (myconfig->verbose>0) printf("Executing CMD_IDENT\n");

	// init the response we pass back
	identresponse = (cmdResponseIdentify_t*)malloc(sizeof(cmdResponseIdentify_t));
	identresponse->totaltime = 0;

	for (pktsgot=0; pktsgot<CMDIDENTIFY_LONGEST_NODE_NAME /4; pktsgot++)
	{
		// assemble the request packet to send to the chip
		setnewseq(&tosend);
		tosend.nodeid = myconfig->nodeid;
		tosend.byte6 = CMD_IDENT;
		tosend.byte7 = pktsgot;
		tosend.byte8 = 0x00;

		// send it
		packetresponse = sendPacket(myconfig, tosend);

		if (packetresponse->errorcode!=errcode_none)
		{
			// bad stuff happened. pass it back up
			identresponse->errorcode = packetresponse->errorcode;
			return identresponse;
		}
		memcpy(&(identresponse->response[pktsgot*4]), &(packetresponse->response.nodeid), 4);

		if (packetresponse->response.nodeid==00 || 
			packetresponse->response.byte6==00  || 
			packetresponse->response.byte7==00  ||		// we break on any nulls.
			packetresponse->response.byte8==00    )
			break;

		identresponse->totaltime += packetresponse->totaltime;
	}

	identresponse->errorcode = errcode_none;
	return identresponse;
}


void cmd_free(void* stuff)
{
	free(stuff);
}