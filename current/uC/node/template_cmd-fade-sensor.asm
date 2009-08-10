	#include "sensorcfg.h"
	#include "main.h"
	#include "protocol.h"
	#include "memoryplacement.h"

	errorlevel  -302  

	CODE

do_cmd_set_sensor_fade_speed:
	global do_cmd_set_sensor_fade_speed

	; This command takes one byte of sensor ID, in packet7,
	; and one byte of state data in packet6.The sensor will
	; respond with 0x00000000 on success, or report the
	; error in packet7 on failure.
	;
	; This command takes a byte of 'delay', which is applied
	; to an PWM LED to govern how fast it 'fades' to its set
	; point.

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
; SENSOR_(AUTOGEN_EVERY_SENSOR_ID)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	; this sensor?
	movfw packet7
	xorlw (AUTOGEN_EVERY_SENSOR_ID)
	btfss STATUS, Z
	goto notthissensor_(AUTOGEN_EVERY_SENSOR_ID)

	; Yup, this sensor. Save the data we're interested in.
	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		movfw packet6
		bsf STATUS, RP0 ; bank 1
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED
		bcf STATUS, RP0 ; bank 0
		goto returnOK
	#else 
		goto wrongType
	#endif
notthissensor_(AUTOGEN_EVERY_SENSOR_ID):
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_FADE_NOT_FOUND
	movwf packet7
	return

wrongType:
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_FADE_WRONG_TYPE
	movwf packet7
	return

returnOK:
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SENSOR_NONE
	movwf packet7
	return

	end