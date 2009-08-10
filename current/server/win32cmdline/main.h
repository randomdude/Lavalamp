
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
#define OPERATION_CMD_GET_SENSOR_TYPE		0x06
#define OPERATION_CMD_SET_SENSOR_FADE_SPEED	0x07
#define OPERATION_CMD_SET_P					0x08
#define OPERATION_CMD_SET_ID				0x09

unsigned long sensorval;	// global to store target sensor value, could do with moving to something neater

#endif