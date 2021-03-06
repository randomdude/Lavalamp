
#ifndef PROTOCOL_H
#define PROTOCOL_H

#include "../shared/lavalamp.h"

typedef struct generic_packet_response
{
	datapkt_t response;
	enum errorcode_enum errorcode;
	long totaltime;
}generic_packet_response_t;

void revPacketByteOrder(datapkt_t* );
long revLongByteOrder(long );
void encipher(unsigned int num_rounds, unsigned long* v, unsigned long* k, int verbosity );
void decipher(unsigned int num_rounds, unsigned long* v, unsigned long* k, int verbosity);
//BOOL generic_packet_response(datapkt_t tosend, HANDLE hnd, unsigned long* key, int verbosity);
void dumppacket(struct datapkt_t* mypacket);
void setrandomnewseq(datapkt_t* ofthis);
void setnewseq(struct datapkt_t* ofthis, long toThis);
generic_packet_response_t* sendPacket(appConfig_t* myconfig, datapkt_t tosend);
generic_packet_response_t* sendPacketWithRetry(appConfig_t* myconfig, datapkt_t tosend);
generic_packet_response_t* sendPacketTimed(appConfig_t* myconfig, datapkt_t tosend);
long sendpacketwithtimeout( appConfig_t* myconfig, long* packet, long datalen, BOOL* didTimeout);
long readpackettimeout( appConfig_t* myconfig, long* packet, long datalen, BOOL* didTimeout);

#endif
