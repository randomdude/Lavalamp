#include "comms.h"

#include <termios.h>
#include <stdio.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/signal.h>
#include <sys/types.h>

#define BAUDRATE B9600
#define _POSIX_SOURCE 1 /* POSIX compliant source */

long readwithtimeout( appConfig_t* myconfig, char* data, long datalen, BOOL* didTimeout )
{
    int bytesRead = 0;
    fd_set input_devices;
    struct timeval timeout;
    //if we want to support more ports/devices add them with FD_SET to input_devices here
    FD_ZERO(&input_devices);
    FD_SET(myconfig->hnd,&input_devices);
    timeout.tv_sec = myconfig->com_timeout;
    while (bytesRead < datalen)
    {
        int error = select(myconfig->hnd +1,&input_devices,NULL,NULL,&timeout);
        //check for errors or time out here
        if (error < 0)
        {
             printf("Failed");
             return 0;
        }
        else if (error == 0)
        {
            *didTimeout = TRUE;
            return 0;
        }
        //we has intput from our devices reading...
        if (FD_ISSET(myconfig->hnd,&input_devices))
             bytesRead += read(myconfig->hnd,data,datalen -bytesRead);
    }
    return bytesRead;
}

long sendwithtimeout( appConfig_t* myconfig, char* data, long datalen, BOOL* didTimeout )
{
    if (myconfig->verbose>2)
	{
		printf("sending raw bytes ");
		int n;
		for (n=0; n<datalen; n++)
			printf(" 0x%02X ", (unsigned char)(data[n]));
		printf("\n");
	}
	int success = write(myconfig->hnd,data, datalen);
	if (success < 0)
        {
            printf("sending failed");
            return 0;
        }
	return 1;

}

int initPort(appConfig_t* myconfig)
{
        int res;
        struct termios options;
        struct sigaction saio;           /* definition of signal action */
        char buf[255];
        myconfig->hnd = open(myconfig->portname, O_RDWR | O_NOCTTY | O_NONBLOCK);
        if (myconfig->hnd <0) {perror(myconfig->portname); exit(-1); }
        fcntl(myconfig->hnd,O_NONBLOCK);
        tcgetattr(myconfig->hnd, &options);
        //speed
        cfsetispeed(&options,BAUDRATE);
        cfsetospeed(&options, BAUDRATE);
        //no parity
        options.c_cflag &= ~PARENB;
        options.c_cflag &= ~CSTOPB;
        options.c_cflag &= ~CSIZE;
        options.c_cflag |= CS8;
        //set for raw input/output
        options.c_lflag &= ~(ICANON | ECHO | ECHOE | ISIG);
        options.c_oflag &= ~OPOST;
        tcsetattr(myconfig->hnd,TCSANOW,&options);
        syncNode(myconfig);
        return TRUE;

}

void closePort(appConfig_t* myappconfig)
{
    close(myappconfig->hnd);
    myappconfig->hnd = INVALID_HANDLE_VALUE;
}
