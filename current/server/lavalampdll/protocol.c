
#include "protocol.h"
#include "comms.h"
#include "commands.h"
#include <stdio.h>
#include <windows.h>

void setrandomnewseq(datapkt_t* ofthis)
{
	if (RAND_MAX <0xFF )
	{
		printf("RAND_MAX on your box is less than 0xFF. This is not currently supported.\n");
		printf("Go code some workaround in setrandomnewseq().\n");
		exit(1);
	}

	ofthis->seqByte3 = ( (char)rand() & 0x000000FF ) ;
	ofthis->seqByte2 = ( (char)rand() & 0x000000FF ) ;
	ofthis->seqByte1 = ( (char)rand() & 0x000000FF ) ;
	ofthis->unused = 0x81;
}

void setseq(datapkt_t* ofthis, long toThis)
{
	ofthis->seqByte3 = ( (unsigned char)(toThis >> 16) & 0x000000FF) ;
	ofthis->seqByte2 = ( (unsigned char)(toThis >>  8) & 0x000000FF) ;
	ofthis->seqByte1 = ( (unsigned char)(toThis >>  0) & 0x000000FF) ;
	ofthis->unused   = 0;
}

long getseq(datapkt_t* ofthis)
{
	long toret = 0;
	toret |= (ofthis->seqByte3 << 16) & 0x00FF0000;
	toret |= (ofthis->seqByte2 <<  8) & 0x0000FF00;
	toret |= (ofthis->seqByte1 <<  0) & 0x000000FF;

	return toret;
}

generic_packet_response_t* sendPacketTimed(appConfig_t* myconfig, datapkt_t tosend)
{
	// todo: use method for measuing time that won't overflow, or handle the overflow
	long starttime,endtime, timetaken;
	generic_packet_response_t* response;

	starttime = timeGetTime();						// 
	response = sendPacketWithRetry(myconfig,tosend);			// do all the sending/reciving needed for this pkt
	endtime = timeGetTime();						// 

	timetaken = endtime-starttime;			// find total time taken
	response->totaltime = timetaken;

	return response;
}

generic_packet_response_t* sendPacketWithRetry(appConfig_t* myconfig, datapkt_t tosend)
{
	long retries = myconfig->retries;
	generic_packet_response_t* result;

	while( (retries-- >0) )
	{
		result = sendPacket(myconfig, tosend);
		if ( (result->errorcode != 00) && (result->errorcode < 0x20))
		{	
			// A protocol error has occured (ie, a failure talking to the node, as opposed to the mode reporting
			// an error), so we should retransmit.
			if (myconfig->verbose>1) 
			{ 
				printf("Communications failure %s! retry %d/%d\n", geterrormessage(result->errorcode), retries, myconfig->retries );  
				Sleep(500);
			}
		} else {
			// otherwise, the transmission was OK - we should return it.
			return result;
		}
	}
	// We've run out of retries. return erraneous packet for handling further up.
	printf("Communications failure %s! Out of retries (tried %d times)\n",geterrormessage(result->errorcode), myconfig->retries );  
	return result;
}

// You'll want to see the README from the PIC project for docs here!
// Todo: clean this up a bit
generic_packet_response_t* sendPacket(appConfig_t* myconfig, datapkt_t tosend)
{
	generic_packet_response_t* wrappedresponse = (generic_packet_response_t*)malloc(sizeof(generic_packet_response_t));
	long p=0;
	datapkt_t crypted;
	long s;
	BOOL timeout;
	long elapsed=0;
	datapkt_t* response = (datapkt_t*)malloc(sizeof(datapkt_t));

	wrappedresponse->totaltime = 0;

	// Is the serial port open?
	if (!isPortOpen(myconfig))
	{
		wrappedresponse->errorcode = errcode_portstate;
		return wrappedresponse;
	}

	// First - send a challenge to the pic, with random payload data.
	setseq( &crypted, getseq(&tosend) );
	crypted.nodeid = tosend.nodeid;
	crypted.byte6 = rand();
	crypted.byte7 = rand(); // todo: make portable to other vals of RAND_MAX
	crypted.byte8 = rand();

	if (myconfig->verbose>2) printf("Challenged node 0x%08lx\n", getseq(&tosend) );

	if (myconfig->verbose>1) { printf("Packet to send (plain  ):\n"); dumppacket(&crypted); }
	if (myconfig->useEncryption)
		encipher(0x20, (unsigned long*)(&crypted), myconfig->key , myconfig->verbose);
	if (myconfig->verbose>2) { printf("Packet to send (crypted):\n"); dumppacket(&crypted); }

	s = sendwithtimeout(myconfig, (char*)(&crypted), sizeof(crypted), &timeout);

	if (!s ||  timeout )
	{
		if (myconfig->verbose>0)
			printf("Unable to send data - GetLastError 0x%lx. Tmeout status %s\n", GetLastError(), timeout?"TRUE":"FALSE");
		if (timeout)
			wrappedresponse->errorcode=errcode_timeout;
		else
			wrappedresponse->errorcode=errcode_internal;
		return wrappedresponse;
	}

	if (myconfig->verbose>0) printf("waiting for response..");
	s = readwithtimeout(myconfig, (char*)response, sizeof(datapkt_t), &timeout);
	if (myconfig->verbose>0) printf("..ok\n");

	if (!s || timeout )
	{
		if (myconfig->verbose>0)
			printf("Unable to ReadFile - GLE 0x%lx, timeout status %s\n", GetLastError(), timeout?"TRUE":"FALSE");
		wrappedresponse->errorcode=errcode_timeout;
		return wrappedresponse;
	}

	if (myconfig->verbose>2) { printf("Packet recieved (crypted):\n"); dumppacket(response); }
	if (myconfig->useEncryption)
		decipher(0x20, (unsigned long*)(response), myconfig->key, myconfig->verbose);
	if (myconfig->verbose>1) { printf("Packet recieved (plain  ):\n"); dumppacket(response); }

	// We have a challenge from the PIC here. It has supplied n+1 and given us P.
	if ( getseq(response) != getseq(&tosend)+1 )
	{
		if (myconfig->verbose>1) printf("Packet is not valid\n");
		wrappedresponse->errorcode=errcode_crypto;
		return wrappedresponse;
	}

	p = 0;
	p |= response->byte6 << 16;
	p |= response->byte7 <<  8;
	p |= response->byte8 <<  0;
	if (myconfig->verbose>2) printf("Node challenged us 0x%08lx\n", p);

	// respond with p+1 and command payload. 
	// We can also inject a fault here, by responding with an incorrect P, so do that if needed.
	if (myconfig->injectFaultInvalidResponse)
		p++;	// Use an incorrect P, as we are simulating a fault

	setseq( &crypted , p+1 );
	crypted.nodeid = tosend.nodeid;
	crypted.byte6 = tosend.byte6;
	crypted.byte7 = tosend.byte7;
	crypted.byte8 = tosend.byte8;
	if (myconfig->verbose>1) { printf("Packet to send (plain  ):\n"); dumppacket(&crypted); }
	if (myconfig->useEncryption)
		encipher(0x20, (unsigned long*)(&crypted), myconfig->key, myconfig->verbose);
	if (myconfig->verbose>2) { printf("Packet to send (crypted):\n"); dumppacket(&crypted); }

	s = sendwithtimeout(myconfig, (char*)(&crypted), sizeof(crypted), &timeout);

	if (!s ||  timeout )
	{
		if (myconfig->verbose>0)
			printf("Unable to send data - GetLastError 0x%lx. Timeout status %s\n", GetLastError(), timeout?"TRUE":"FALSE");
		if (timeout)
			wrappedresponse->errorcode=errcode_timeout;
		else
			wrappedresponse->errorcode=errcode_internal;
		return wrappedresponse;
	}

	if (myconfig->verbose>0) printf("waiting for response..");
	s = readwithtimeout(myconfig, (char*)response, sizeof(datapkt_t), &timeout);
	if (myconfig->verbose>0) printf("..ok\n");

	if (!s || timeout )
	{
		if (myconfig->verbose>0)
			printf("Unable to ReadFile - GLE 0x%lx, timeout status %s\n", GetLastError(), timeout?"TRUE":"FALSE");
		if (timeout)
			wrappedresponse->errorcode=errcode_timeout;
		else
			wrappedresponse->errorcode=errcode_internal;
		return wrappedresponse;
	}

	if (myconfig->verbose>2) { printf("Packet received (crypted):\n"); dumppacket(response); }
	if (myconfig->useEncryption)
		decipher(0x20, (unsigned long*)(response), myconfig->key, myconfig->verbose);
	if (myconfig->verbose>1) { printf("Packet received (plain  ):\n"); dumppacket(response); }

	// This should be the response to our command (and p+2).
	if ( getseq(response) == p+2 )
	{
		if (myconfig->verbose>1) printf("Packet is valid\n");
		wrappedresponse->response.nodeid = response->nodeid;
		wrappedresponse->response.byte6 = response->byte6;
		wrappedresponse->response.byte7 = response->byte7;
		wrappedresponse->response.byte8 = response->byte8;
		wrappedresponse->errorcode=errcode_none;
	} else {
		if (myconfig->verbose>1) printf("Packet is not valid\n");
		wrappedresponse->errorcode=errcode_crypto;
	}
	return wrappedresponse;

}

void dumppacket(datapkt_t* mypacket)
{
	printf("* unused byte- 0x%02lx\n", mypacket->unused  );
	printf("* SEQ number - 0x%06lx\n", getseq ( mypacket ) );
	printf("* node ID    - 0x%02lx\n", mypacket->nodeid  );
	printf("* Data bytes - 0x%02hhx 0x%02hhx 0x%02hhx\n", mypacket->byte6, mypacket->byte7, mypacket->byte8);	
	//	printf(" (%c:%c:%c:%c)\n", mypacket->nodeid, mypacket->byte6, mypacket->byte7, mypacket->byte8);
}

void encipher(unsigned int num_rounds, unsigned long* v, unsigned long* k, int vebosity) 
{
	unsigned long  v0,v1,i, sum, delta;

	// By convention, we use most-significant-byte-first byte ordering when sending crypted data to the PIC.
	v[0] = revLongByteOrder(v[0]);
	v[1] = revLongByteOrder(v[1]);

	v0=v[0]; v1=v[1]; sum=0; delta=0x9E3779B9;
	for(i=0; i<num_rounds; i++) 
	{
		v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + k[sum & 3]);
		if (vebosity>4) printf("cycle %d v0 = 0x%lx ", i, v0);
		sum += delta;
		v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + k[(sum>>11) & 3]);
		if (vebosity>4) printf(" v1 = 0x%lx new sum 0x%lx\n", v1, sum);
	}
	v[0]=v0; v[1]=v1;

	// By convention, we use most-significant-byte-first byte ordering when sending crypted data to the PIC.
	v[1] = revLongByteOrder(v[1]);
	v[0] = revLongByteOrder(v[0]);
}
void decipher(unsigned int num_rounds, unsigned long* v, unsigned long* k, int vebosity) 
{
	unsigned long v0, v1, i, delta, sum;

	// By convention, we use most-significant-byte-first byte ordering when sending crypted data to the PIC.
	v[0] = revLongByteOrder(v[0]);
	v[1] = revLongByteOrder(v[1]);

	v0=v[0]; v1=v[1]; delta=0x9E3779B9; sum=delta*num_rounds;
	for(i=0; i<num_rounds; i++) 
	{
		v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + k[(sum>>11) & 3]);
		if (vebosity>4) printf("cycle %d v0 = 0x%lx ", i, v0);
		sum -= delta;
		v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + k[sum & 3]);
		if (vebosity>4) printf(" v1 = 0x%lx new sum 0x%lx\n", v1, sum);
	}
	v[0]=v0; v[1]=v1; 

	// By convention, we use most-significant-byte-first byte ordering when sending crypted data to the PIC.
	v[0] = revLongByteOrder(v[0]);
	v[1] = revLongByteOrder(v[1]);
}

long revLongByteOrder(long ofthis)
{
	unsigned char byte1 = (unsigned char)((ofthis & 0x000000FF)      );
	unsigned char byte2 = (unsigned char)((ofthis & 0x0000FF00) >> 8 );
	unsigned char byte3 = (unsigned char)((ofthis & 0x00FF0000) >> 16);
	unsigned char byte4 = (unsigned char)((ofthis & 0xFF000000) >> 24);

	unsigned long output = 0;

	output |= byte1;
	output=output << 8;
	output |= byte2;
	output=output << 8;
	output |= byte3;
	output=output << 8;
	output |= byte4;

	return output;
}

void revPacketByteOrder(datapkt_t* mypacket)
{
	unsigned char bytesin[8], bytesout[8];
	int n;

	memcpy(bytesin, mypacket, sizeof(datapkt_t));

	for (n=0; n<8; n++)
	{
		bytesout[n] = bytesin[7-n];
	}

	memcpy(mypacket, bytesout, sizeof(datapkt_t));	
}

char* geterrormessage(int errorcode)
{
	switch (errorcode)
	{
	case errcode_none:
			return "No error";
			break;
	case errcode_timeout:
			return "A timeout occured attempting to talk to node";
			break;
	case errcode_crypto:
			return "A cryptographic error occured attempting to talk to node";
			break;
	case errcode_internal:
			return "An internal error attempting to talk to node";
			break;
	case errcode_sensor_not_found:
			return "The node cannot find the specified sensor";
			break;
	case errcode_sensor_wrong_type:
			return "The specified sensor does not acept the specified command";
			break;
	default:
			return "An unrecognised error occured";
	}
}