
#include <stdio.h>
#include <windows.h>
#include "../shared/lavalamp.h"

#include "main.h"

#define RESPONSE_PERIOD 10

BOOL defaultkey=TRUE;
int operation=OPERATION_CMD_PING;

appConfig_t* getdefaultconfig()
{
	appConfig_t* myconfig=(appConfig_t*)malloc(sizeof(appConfig_t));

	myconfig->nodeid = 0x01;
	myconfig->sensorid = 0x01;
	myconfig->portname = "COM3";
	myconfig->key[0] = (long)0x00112233;
	myconfig->key[1] = (long)0x44556677;
	myconfig->key[2] = (long)0x8899aabb;
	myconfig->key[3] = (long)0xccddeeff;
	myconfig->verbose = 0x00;
	myconfig->hnd = INVALID_HANDLE_VALUE;
	myconfig->machineoutput = FALSE;
	myconfig->com_timeout = 1000;
	myconfig->assume_synced = FALSE;
	myconfig->retries = 1;	// todo: add this as a commandline parameter

	return myconfig;
}

unsigned char hexchartoint(char hexchar)
{
	if (hexchar<':')
		return hexchar-'0';
	hexchar|=0x20;	// shift to lowercase
	return (hexchar-'a')+10;
}

long hextolong(char* hexstring)
{
	long output=0;
	unsigned long index;
	unsigned long bytecount=0;
	for (index=(unsigned long)strlen(hexstring); index>0; index--)
	{
		output+=( hexchartoint(hexstring[index-1]) * (1<<(bytecount*4)) );
		bytecount++;
	}

	return output;
}


unsigned long parseargtolong(char* arg)
{
	unsigned long resp = 0x00;
	BOOL ishex=FALSE;

	// Lop off any leading 0x
	if (arg[0]=='0' && 
		( arg[1]=='x' || arg[1]=='X') )
	{
		arg+=2;
		ishex=TRUE;
	}

	if (ishex)
	{
		resp = hextolong(arg);
	} else {
		resp = atoi(arg);
	}
	return resp;
}

void parsecmdline(int argc, char* argv[], appConfig_t* myappconfig)
{
	int myargc=1;
	int n;

	while(myargc<argc)
	{
		if (argv[myargc][0]=='-' || argv[myargc][0]=='/' )
		{
			switch(argv[myargc][1])
			{
			case 'o':
					if (myargc==argc)
						doUsage();
					else
						sensorval =  (unsigned long)parseargtolong(argv[++myargc]);
					break;
			case 'm':
					myappconfig->machineoutput=TRUE;
					break;
			case 'a':
					myappconfig->assume_synced=TRUE;
					break;
			case 'k':
					defaultkey=FALSE;
					break;
			case 'v':
					n=1;
					while(argv[myargc][n++]=='v')
						myappconfig->verbose++;
					break;
			case 'h':
					doUsage();
			case 'p':
				if (myargc==argc)	// -p was last argument
					doUsage();
				else
					myappconfig->portname=argv[++myargc];
				break;
			case 'n':
				if (myargc==argc)
					doUsage();
				else
					myappconfig->nodeid=(unsigned char)parseargtolong(argv[++myargc]);
				break;
			case 's':
				if (myargc==argc)
					doUsage();
				else
					myappconfig->sensorid=(unsigned char)parseargtolong(argv[++myargc]);
				break;
			case 'c':
				if (myargc==argc)
					doUsage();
				else
				{
					myargc++;
					if (_stricmp(argv[myargc], "identify")==0)
						operation = OPERATION_CMD_IDENT;
					else if (_stricmp(argv[myargc], "ping")==0)
						operation = OPERATION_CMD_PING;
					else if (_stricmp(argv[myargc], "get_sensor_count")==0)
						operation = OPERATION_CMD_GET_SENSOR_COUNT ;
					else if (_stricmp(argv[myargc], "get_sensor_generic_digital")==0)
						operation = OPERATION_CMD_GET_GENERIC_SENSOR ;
					else if (_stricmp(argv[myargc], "set_sensor_generic")==0)
						operation = OPERATION_CMD_SET_GENERIC_SENSOR ;
					else if (_stricmp(argv[myargc], "set_sensor_pwm")==0)
						operation = OPERATION_CMD_SET_GENERIC_SENSOR ;
					else if (_stricmp(argv[myargc], "get_sensor_type")==0)
						operation = OPERATION_CMD_GET_SENSOR_TYPE ;
					else if (_stricmp(argv[myargc], "set_sensor_fade_speed")==0)
						operation = OPERATION_CMD_SET_SENSOR_FADE_SPEED ;
					else if (_stricmp(argv[myargc], "set_p")==0)
						operation = OPERATION_CMD_SET_P;
					else if (_stricmp(argv[myargc], "set_id")==0)
						operation = OPERATION_CMD_SET_ID;
					else
					{
						printf("Unrecognised command '%s'\n", argv[myargc]);
						doUsage();
					}
				}
				break;
			default:
					doUsage();
			}
		} else {
			doUsage();
		}
		myargc++;
	}
}

void reporterror(int errorcode)
{
	printf("Error %d - '%s'\n",errorcode, geterrormessage(errorcode));
}
void readkey(appConfig_t* myappconfig)
{
	char inputbuf[100];
	BOOL keepgoing=TRUE;
	int count =0;
	char charin;
	char buffer[9];

	while(keepgoing)
	{
		charin = getchar();
		if (charin=='x' || charin=='X')		// Assume it's as in '0x1234' and thus we should start recieving after the x
		{
			count=0;
		}
		else if ( (charin>'/' && charin<':') || 
			 (charin>'@' && charin<'[') ||
			 (charin>'`' && charin<'{')   ) 
		{
			inputbuf[count++] = charin;
		}
		else if ( (charin==0x00) || 
			 (charin==0x0d) || 
			 (charin==0x0a) || 
			 (count>32) )
		{
			keepgoing=FALSE;
		}
	}
	inputbuf[count+1]=0;
	if (count!=32)
	{
		printf("Key must be specified as 32 contigous hex digits, eg.\n");
		printf("00112233445566778899aabbccddeeff\n");
		exit(1);
	}

	// For each key[..] element, pick 8 bytes of hex
	strncpy_s(buffer, 9, &inputbuf[ 0], 8); buffer[8] = 0;
	myappconfig->key[0] = hextolong(buffer);
	strncpy_s(buffer, 9, &inputbuf[ 8], 8); buffer[8] = 0;
	myappconfig->key[1] = hextolong(buffer);
	strncpy_s(buffer, 9, &inputbuf[16], 8); buffer[8] = 0;
	myappconfig->key[2] = hextolong(buffer);
	strncpy_s(buffer, 9, &inputbuf[24], 8); buffer[8] = 0;
	myappconfig->key[3] = hextolong(buffer);

	if (myappconfig->verbose>1)
	{
		printf("parsed key:\n");
		printf("[0] 0x%08lx\n[1] 0x%08lx\n[2] 0x%08lx\n[3] 0x%08lx\n", myappconfig->key[0],myappconfig->key[1],myappconfig->key[2],myappconfig->key[3] );
	}
}

void do_cmd_ident(appConfig_t* myappconfig)
{
	cmdResponseIdentify_t* identifyResponse;
	identifyResponse = cmdIdentify(myappconfig);
	if (myappconfig->machineoutput)
	{
		if (identifyResponse->errorcode=errcode_none)
			printf("%d|%d|%s", identifyResponse->errorcode, identifyResponse->totaltime, identifyResponse->response);
		else
			printf("%d|%d|%s", identifyResponse->errorcode, 0, 0);
	} else {
		if (identifyResponse->errorcode==errcode_none)
		{
			printf("Node reported its name as '%s'\n", identifyResponse->response);
		}
		else
		{
			reporterror(identifyResponse->errorcode);
		}
	}
	cmd_free(identifyResponse);
}

void do_cmd_set_sensor_fade_speed(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse = cmdSetSensorFadeSpeed(myappconfig, (unsigned char)sensorval);
	if (myappconfig->machineoutput)
	{
		printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	} else {
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}

	cmd_free(genericResponse);
}
void do_cmd_get_sensor_type(appConfig_t* myappconfig)
{
	cmdResponseGetSensorType_t* genericResponse;

	genericResponse = cmdGetSensorType(myappconfig);
	if (myappconfig->machineoutput)
	{
		if (genericResponse->errorcode==errcode_none)
			printf("%d|%d|%d|%s", genericResponse->errorcode, genericResponse->totaltime, genericResponse->type, genericResponse->FriendlyType );
		else
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	} else {
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Sensor has type '%s' (%#04x)\n", genericResponse->FriendlyType, genericResponse->type );
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}

	cmd_free(genericResponse);
}

void do_cmd_ping(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse = cmdPing(myappconfig);
	if (myappconfig->machineoutput)
	{
		if (genericResponse->errorcode==errcode_none)
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, genericResponse->response);
		else
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	} else {
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Node responded as present\n");
			printf("returning raw data bytes %#08lX\n", genericResponse->response);
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}

	cmd_free(genericResponse);
}
void do_cmd_get_sensor_count(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse=cmdCountSensors(myappconfig);
	if (myappconfig->machineoutput)
	{
		if (genericResponse->errorcode==errcode_none)
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, genericResponse->response);
		else
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	}
	else
	{
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Node reported %d sensors present.\n", genericResponse->response);
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}
	cmd_free(genericResponse);
}

void do_cmd_set_generic_sensor(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse = cmdSetGenericDigitalSensor(myappconfig, (unsigned char)sensorval);
	if (myappconfig->machineoutput)
	{
		if (genericResponse->errorcode==errcode_none)
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, genericResponse->response);
		else
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	}
	else
	{
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Node set output successfully.\n");
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}
	cmd_free(genericResponse);
}

void do_cmd_set_id(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse = cmdSetNodeId(myappconfig, (unsigned char)sensorval);
	if (myappconfig->machineoutput)
	{
		printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	}
	else
	{
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Node set new ID successfully.\n");
			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}
	cmd_free(genericResponse);
}


void do_cmd_set_p(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	char byte1 = sensorval & 0x000000FF;
	char byte2 = (sensorval & 0x0000FF00) >>  8;
	char byte3 = (unsigned char)( (sensorval & 0x00FF0000) >> 16 );

	printf("Changing P! This will cause errors and horribleness! This command should not return, but should throw a communications failure as the node attempts to use the new P!\n" );

	genericResponse=cmdSetP(myappconfig,byte1, byte2, FALSE );
	genericResponse=cmdSetP(myappconfig, byte3, 0x00, TRUE );

	if (myappconfig->machineoutput)
	{
		printf("%d|%d", genericResponse->errorcode, genericResponse->totaltime);
	}
	else
	{
		if (genericResponse->errorcode==errcode_none)
		{
			printf("Node reported OK, is NOT using new P\n" );

			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}
	cmd_free(genericResponse);
}

void do_cmd_get_generic_digital_sensor(appConfig_t* myappconfig)
{
	cmdResponseGeneric_t* genericResponse;

	genericResponse=cmdGetGenericDigitalSensor(myappconfig);
	if (myappconfig->machineoutput)
	{
		if (genericResponse->errorcode==errcode_none)
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, genericResponse->response);
		else
			printf("%d|%d|%d", genericResponse->errorcode, genericResponse->totaltime, 0);
	}
	else
	{
		if (genericResponse->errorcode==errcode_none)
		{
			if ( 0 == (genericResponse->response && 0x000000FF))
				printf("Node returned status OFF\n" );
			else
				printf("Node returned status ON\n" );

			printf("Total transaction time:%dmsec.\n", genericResponse->totaltime); 
		}
		else
		{
			reporterror(genericResponse->errorcode);
		}
	}
	cmd_free(genericResponse);
}
int main(int argc, char* argv[])
{
	appConfig_t* myappconfig = getdefaultconfig();	// get default config

	parsecmdline(argc, argv, myappconfig);			// modify config according to cmdline

	if (defaultkey==FALSE)
	{
		printf("please enter key as a sequence of hex bytes\n");
		readkey(myappconfig);
	}

	if (myappconfig->verbose>0)
		printf("Querying node %d \n", myappconfig->nodeid);
	
	if ( !initPort(myappconfig) )
	{
		printf("Unable to open port '%s'\n", myappconfig->portname);
		return FALSE;
	}

	// do whatever we need to do
	switch(operation)
	{
		case OPERATION_CMD_IDENT:
			do_cmd_ident(myappconfig);
			break;
		case OPERATION_CMD_GET_SENSOR_COUNT :
			do_cmd_get_sensor_count(myappconfig);
			break;
		case OPERATION_CMD_GET_GENERIC_SENSOR :
			do_cmd_get_generic_digital_sensor(myappconfig);
			break;
		case OPERATION_CMD_SET_GENERIC_SENSOR :
			do_cmd_set_generic_sensor(myappconfig);
			break;
		case OPERATION_CMD_PING:
			do_cmd_ping(myappconfig);
			break;
		case OPERATION_CMD_GET_SENSOR_TYPE:
			do_cmd_get_sensor_type(myappconfig);
			break;			
		case OPERATION_CMD_SET_SENSOR_FADE_SPEED:
			do_cmd_set_sensor_fade_speed(myappconfig);
			break;			
		case OPERATION_CMD_SET_P:
			do_cmd_set_p(myappconfig);
			break;			
		case OPERATION_CMD_SET_ID:
			do_cmd_set_id(myappconfig);
			break;			
	}

	// close the port.
	closePort(myappconfig);

	free(myappconfig);
	return TRUE;
}

void doUsage()
{
	printf("Command-line controller for home/industy automation system\n");
	printf("Copyright Alan Hammond, 2007\n\n");
	printf("Options:\n");
	printf(" -c <command>       : Specify which command to perform as below:\n");
	printf("    identify        : Query the node for its identification string (eg. 'desk \n%s", \
		   "                      light')\n");
	printf("    ping            : Ensure connectivity with node\n");
	printf("    get_sensor_count: Ask node how many sensors are attatched to it\n");
	printf("    get_sensor_type : Get type of a given sensor \n");
	printf("    get_sensor_generic_digital : Get state of a 'generic digital' sensor \n");
	printf("    set_sensor_generic : Set a 'generic' type sensor\n");
	printf("    get_sensor_fade_speed : Set fading speed of an PWM output\n");
	printf("    set_p			: Set crypto seed 'p' (note: advanced!)\n");
	printf("    set_id			: Set node ID\n");
	printf(" -s                 : Select sensor to operate on\n");
	printf(" -o                 : Select output payload of command, eg. value to set_sensor to, or new value to set node ID to\n");
	printf(" -a                 : Assume node is already sync'ed; don't bother sending 0xAA..AA sync packet\n");
	printf(" -k                 : Recieve key on stdin (otherwise use default key - \n%s" , \
		   "                      0x00112233..ccddeeff, not a good idea!\n");
	printf(" -m                 : 'Machine output' - output parsed command output\n");
	printf(" -n <id>            : Select node to talk to. can be hex (prefix with 0x)\n%s" , \
		   "                      or decimal.\n");
	printf(" -p <name>          : Select serial device to use\n");
	printf(" -v                 : Be verbose. Specify up to three times:\n");
	printf("                        Once for basic info\n");
	printf("                        Twice for protocol info\n");
	printf("                        Thrice for protocol info (unused)\n");
	printf("                        Four times for hardcore cryption innards\n");
	exit(1);
}