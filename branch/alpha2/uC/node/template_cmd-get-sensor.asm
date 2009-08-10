
	#include "main.h"
	#include "sensorcfg.h"
	#include "processnamedcmd.asm"
	#include "debugger.h"
	#include "protocol.h"

	errorlevel  -302  

	CODE
do_cmd_get_sensor
	GLOBAL do_cmd_get_sensor
	; This command takes one byte  of input, being an
	; index to a sensor (eg. kettle temperature).This
	; is in packet7.

	; Upon an error, the topmost bit of packet5 will be set
	; and the error code will be placed in packet7. Success
	; results in the topmost bit being cleared and data
	; being returned in packet5 (bits 1-7) to packet7
	; A special case is sensor 0, which will output a
	; count of present sensors, in packet7.

	; Configure using the following:
	;define SENSOR_3_PRESENT		(mandatory)
	;define SENSOR_3_PORT PORTB		(port)
	;define SENSOR_3_TRIS TRISB		(tristate)
	;define SENSOR_3_PIN 1			(pin of both above)
	;define SENSOR_3_IS_INVERTING	(set for an inverting input
	;define SENSOR_3_TYPE SENSOR_ID_GENERIC_DIGITAL_IN	(mandatory)

	movlw DEBUGGER_GOT_CMD_GETSENSOR
	call debugger_spitvalue;

	; first, handle any requests for sensor 00
	movfw packet7
	btfsc STATUS, Z
	goto tellsensorcount
	
	; This block will be replicated once for every sensor, each with (AUTOGEN_EVERY_SENSOR_ID) replaced
   ; with an increasing sensor ID number. 
(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT

	movfw packet7
	xorlw (AUTOGEN_EVERY_SENSOR_ID)
	btfss STATUS, Z
	goto notthissensor_(AUTOGEN_EVERY_SENSOR_ID)

	#if (SENSOR_ID_GENERIC_DIGITAL_IN == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

		clrf packet7
	#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_IS_INVERTING
		btfss SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
	#else
		btfsc SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
	#endif
		bsf packet7, 0
		goto cmd_get_return_ok
	#else
		goto cmd_get_return_wrong_type
	#endif
notthissensor_(AUTOGEN_EVERY_SENSOR_ID)
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	; If we get here, we don't know this sensor (none of the above goto's have been taken). Respond with
	; a failure code.
	movlw DEBUGGER_UNKNOWN_SENSOR
	call debugger_spitvalue;

	clrf packet4	
	clrf packet5
	clrf packet6

	bsf packet5, 0	; signal error
	movlw ERR_GET_SENSOR_NODE_NOT_FOUND	; set error code
	movwf packet7
	return
	
cmd_get_return_wrong_type:
	clrf packet4	
	clrf packet5
	clrf packet6
	bsf packet5, 0	; signal error
	movlw ERR_GET_SENSOR_NODE_WRONG_TYPE	; set error code
	movwf packet7
	return

cmd_get_return_ok:
	clrf packet4
	clrf packet5
	clrf packet6
;	bcf packet4, 0
	return

tellsensorcount:
	movlw SENSOR_COUNT
	movwf packet7
	clrf packet6
	clrf packet5
	clrf packet4
	return

	end
