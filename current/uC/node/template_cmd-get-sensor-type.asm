
	#include "main.h"
	#include "sensorcfg.h"
	#include "processnamedcmd.asm"
	#include "protocol.h"

	errorlevel  -302  

	CODE
do_cmd_get_sensor_type
	GLOBAL do_cmd_get_sensor_type
	; This command takes one byte  of input, being an
	; index to a sensor. This is in packet7. It responds
	; with a number indicating the sensor type, eg,
	; a SENSOR_ID_GENERIC_DIGITAL_IN would return 1.
	; definitions are in protocol.h.

	; Upon an error, the topmost bit of packet5 will be set
	; and the error code will be placed in packet7. Success
	; results in the topmost bit of packet5 being cleared,
	; and data being returned in packet7.

	clrf packet6

	; Check the host hasn't requested a sensor index zero
	; as this is illegal
	movfw packet7
	btfsc STATUS, Z
	goto sensor_not_found
	; Check the host hasn't requested a sensor index greater
	; than our sensor count
	sublw SENSOR_COUNT	; packet7 - SENSOR_COUNT > 0?
	btfss STATUS, C
	goto sensor_not_found

	; Loop through every sensor, 
(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	movfw packet7
	xorlw (AUTOGEN_EVERY_SENSOR_ID)
	btfss STATUS,Z
	goto notthissensor_(AUTOGEN_EVERY_SENSOR_ID)
	; This sensor is being addressed
    movlw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE
	; Jump to the end of the autogenned code so we don't have to check every other
	; sensor, too.
	goto returnOK

notthissensor_(AUTOGEN_EVERY_SENSOR_ID):  
	; OK, skip to the next sensor.
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	;<-- Note that code will never get to here! 

returnOK:
	clrf packet5
	movwf packet7

	return

sensor_not_found:

	movlw 0x01
	movwf packet5
	movlw ERR_GET_SENSOR_TYPE_SNSR_NOT_FND
	movwf packet7

	return

	end