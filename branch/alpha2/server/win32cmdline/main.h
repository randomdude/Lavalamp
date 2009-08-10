
#ifndef MAIN_H
#define MAIN_H


appConfig_t* getdefaultconfig();
unsigned char hexchartoint(char hexchar);
long hextolong(char* hexstring);
unsigned long parseargtolong(char* arg);
void parsecmdline(int argc, char* argv[], appConfig_t* myappconfig);
void readkey(appConfig_t* myappconfig);
void doUsage();

#define OPERATION_CMD_PING					0x01
#define OPERATION_CMD_IDENT					0x02
#define OPERATION_CMD_GET_SENSOR_COUNT		0x03
#define OPERATION_CMD_GET_GENERIC_SENSOR	0x04
#define OPERATION_CMD_SET_GENERIC_SENSOR	0x05

long sensorval;	// global to store target sensor value, could do with moving to something neater

#endif