
#include "commands.h"
#include "protocol.h"
#include <stdio.h>
#include "../shared/lavalamp.h"

// Place custom 'command' calls in here. 



cmdResponseGeneric_t*  cmdSetSensorFadeSpeed(appConfig_t* myconfig, unsigned char tothis)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* sensorcountresponse;

	// sets the fade speed of a PWM sensor
	// returns an error code, and time of roundtrip.
	if (myconfig->verbose>1) 
		printf("Executing CMD_SET_SENSOR_FADE asking for sensor %d\n", myconfig->sensorid);
	else if (myconfig->verbose>0) 
		printf("Executing CMD_SET_SENSOR_FADE\n");

	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_SET_SENSOR_FADE ;
	tosend.byte7 = tothis;
	tosend.byte8 = myconfig->sensorid;

	packetresponse = sendPacketTimed(myconfig, tosend);

	sensorcountresponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	sensorcountresponse->response  = 0x00;
	sensorcountresponse->totaltime = packetresponse->totaltime;
	sensorcountresponse->errorcode = errcode_none;

	return sensorcountresponse;
}

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

	setrandomnewseq(&tosend);
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

cmdResponseGeneric_t*  cmdSetGenericDigitalSensor(appConfig_t* myconfig, unsigned char tothis)
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

	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_SET_SENSOR;
	tosend.byte7 = tothis;
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
	setrandomnewseq(&tosend);
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
	setrandomnewseq(&tosend);
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

cmdResponseGeneric_t*  cmdSetNodeKeyByte(appConfig_t* myconfig, unsigned char index, unsigned char newVal)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* noderesponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (myconfig->verbose>0) printf("Executing cmdSetNodeKeyByte, byte %d val 0x%02X\n", index, newVal );

	// prepare packet
	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_SET_KEY_BYTE;
	tosend.byte7 = index;
	tosend.byte8 = newVal;

	// send
	packetresponse = sendPacketTimed(myconfig, tosend);
	
	noderesponse->totaltime = packetresponse->totaltime;
	noderesponse->errorcode = packetresponse->errorcode;
	noderesponse->response = 0;

	cmd_free(packetresponse);

	return noderesponse;
}

cmdResponseGeneric_t*  cmdReload(appConfig_t* myconfig)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* noderesponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (myconfig->verbose>0) printf("Executing cmdSetNodeId\n");

	// prepare packet
	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_RELOAD_FROM_FLASH;
	tosend.byte7 = 0x00;
	tosend.byte8 = 0x00;

	// send
	packetresponse = sendPacketTimed(myconfig, tosend);
	
	noderesponse->totaltime = packetresponse->totaltime;
	noderesponse->errorcode = packetresponse->errorcode;
	noderesponse->response = 0;

	cmd_free(packetresponse);

	return noderesponse;
}

cmdResponseGeneric_t*  cmdSetNodeId(appConfig_t* myconfig, unsigned char tothis)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* noderesponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (myconfig->verbose>0) printf("Executing cmdSetNodeId\n");

	// prepare packet
	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_SET_NODE_ID;
	tosend.byte7 = 0x00;
	tosend.byte8 = tothis;

	// send
	packetresponse = sendPacketTimed(myconfig, tosend);
	
	noderesponse->totaltime = packetresponse->totaltime;
	noderesponse->errorcode = packetresponse->errorcode;
	noderesponse->response = 0;

	cmd_free(packetresponse);

	return noderesponse;
}

cmdResponseGeneric_t*  cmdSetP(appConfig_t* myconfig, byte byte1, byte byte2, BOOL isHigh)
{
	datapkt_t tosend;
	generic_packet_response_t* packetresponse;
	cmdResponseGeneric_t* pingresponse = (cmdResponseGeneric_t*)malloc(sizeof(cmdResponseGeneric_t));

	if (myconfig->verbose>0) printf("Executing cmdPing\n");

	// prepare packet
	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	if (!isHigh)
	{
		tosend.byte6 = CMD_SET_P_LOW;
		tosend.byte7 = byte1;
		tosend.byte8 = byte2;
	} else {
		tosend.byte6 = CMD_SET_P_HIGH;
		tosend.byte7 = 0x00;
		tosend.byte8 = byte1;
	}

	// send
	packetresponse = sendPacketTimed(myconfig, tosend);
	
	pingresponse->totaltime = packetresponse->totaltime;
	pingresponse->errorcode = packetresponse->errorcode;
	pingresponse->response = 0;

	cmd_free(packetresponse);

	return pingresponse;
}

cmdResponseGetSensorType_t* cmdGetSensorType(appConfig_t* myconfig)
{
	cmdResponseGetSensorType_t* toreturn = malloc(sizeof(cmdResponseGetSensorType_t));
	generic_packet_response_t*  response ;
	datapkt_t tosend;

	if (myconfig->verbose>0) printf("Executing CMD_GET_SENSOR_TYPE on sensor 0x%lx \n", myconfig->sensorid);

	// prepare packet
	setrandomnewseq(&tosend);
	tosend.nodeid = myconfig->nodeid;
	tosend.byte6 = CMD_GET_SENSOR_TYPE;
	tosend.byte7 = 0x00;
	tosend.byte8 = myconfig->sensorid;

	// send
	response = sendPacketTimed(myconfig, tosend);

	if (response->errorcode == errcode_none)
	{
		// if the node reported an error, the topmost bit of byte5  will
		// be set. Set the errorcode accordingly
		if ( (response->response.byte6 & 0x01 ) == 0x00 )
		{
			toreturn->errorcode = errcode_none;
			toreturn->type = response->response.byte8;
			switch (toreturn->type)
			{
			case 0x01:
				toreturn->FriendlyType = "Generic digital input";
				break;
			case 0x02:
				toreturn->FriendlyType = "Generic digital output";
				break;
			case 0x03:
				toreturn->FriendlyType = "LED dimmer control";
				break;
			default:
				toreturn->FriendlyType = "(unknown)";
				break;
			}
		} else {
			if (response->response.byte8 == ERR_GET_SENSOR_TYPE_SENSOR_NOT_FOUND)
				toreturn->errorcode = errcode_sensor_not_found;
		}
		toreturn->totaltime = response->totaltime;
	} else {
		// there was an error processing the packet
		toreturn->totaltime = response->totaltime;
		toreturn->errorcode = response->errorcode;
	}

	return toreturn;
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
		setrandomnewseq(&tosend);
		tosend.nodeid = myconfig->nodeid;
		tosend.byte6 = CMD_IDENT;
		tosend.byte7 = pktsgot;
		tosend.byte8 = 0x00;

		// send it
		packetresponse = sendPacketWithRetry(myconfig, tosend);

		if (packetresponse->errorcode!=errcode_none)
		{
			// bad stuff happened. pass it back up
			identresponse->errorcode = packetresponse->errorcode;
			return identresponse;
		}
		if (myconfig->verbose>1) printf("bytes %c %c %c \n", packetresponse->response.byte6, packetresponse->response.byte7, packetresponse->response.byte8);

		memcpy(&(identresponse->response[pktsgot*3]), &(packetresponse->response.byte6), 3);

		if (packetresponse->response.byte6==00  || 
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