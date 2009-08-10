
#ifndef LAVALAMP_H
#define LAVALAMP_H

#include <windows.h>

// Force linkage as C 
#ifdef __cplusplus
extern "C"
{
#endif

#define CMDIDENTIFY_LONGEST_NODE_NAME 0x20

typedef struct datapkt_t
{
	long seqnum;
	unsigned char nodeid;
	unsigned char byte6;
	unsigned char byte7;
	unsigned char byte8;
} datapkt_t;

// error codes below 0x20 are reserved for the low-level comms subsystem. Above is errors returned from the PIC
// Note that errors returned from the PIC are command-dependant and thus need parsing at the PC-side, depending
// which command they are returned from.
enum errorcode_enum { errcode_none,errcode_timeout,errcode_crypto,errcode_internal, errcode_sensor_not_found=0x20, errcode_sensor_wrong_type };	


// Each cmd_* returns one of these.
typedef struct cmdResponseGeneric
{
	long response;
	enum errorcode_enum errorcode;
	long totaltime;
}cmdResponseGeneric_t;
typedef struct cmdResponseIdentify
{
	char response[CMDIDENTIFY_LONGEST_NODE_NAME];
	enum errorcode_enum errorcode;
	long totaltime;
}cmdResponseIdentify_t;

typedef struct appConfig_t
{
	// this struct holds all the stuff that the user can configure. Its a lot easier to pass
	// about than a load of individual fields.
	unsigned char nodeid;
	unsigned char sensorid;
	char* portname;
	unsigned long key[4];
	int verbose;
	HANDLE hnd;
	BOOL machineoutput;
	long com_timeout;
	BOOL assume_synced;
} appConfig_t ;

BOOL initPort(appConfig_t*);
BOOL closePort(appConfig_t*);
BOOL isPortOpen(appConfig_t* myappconfig);

cmdResponseIdentify_t* cmdIdentify(appConfig_t*);
cmdResponseGeneric_t*  cmdPing(appConfig_t* myconfig);
cmdResponseGeneric_t*  cmdCountSensors(appConfig_t*);
cmdResponseGeneric_t*  cmdGetGenericDigitalSensor(appConfig_t* myconfig);
cmdResponseGeneric_t*  cmdSetGenericDigitalSensor(appConfig_t* myconfig, BOOL tothis);

void cmd_free(void* stuff);

char* geterrormessage(int errorcode);

// End force linkage as C 
#ifdef __cplusplus
}
#endif

#endif