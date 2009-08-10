
#ifndef COMMS_H
#define COMMS_H

#include "../shared/lavalamp.h"

long readwithtimeout( appConfig_t* myconfig, char* data, long datalen,  BOOL* didTimeout );
long sendwithtimeout( appConfig_t* myconfig, char* data, long datalen,  BOOL* didTimeout );


void sync(appConfig_t* myconfig);

#endif
