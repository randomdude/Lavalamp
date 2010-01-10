
	#include "main.h"
	#include "sensorcfg.h"
	#include "processnamedcmd.asm"
	#include "protocol.h"
	#include "memoryplacement.h"


	errorlevel  -302  

	CODE
do_cmd_set_sensor
	GLOBAL do_cmd_set_sensor

	; This command takes one byte of sensor ID, in packet7,
	; and one byte of state data in packet6.The sensor will
	; respond with 0x00000000 on success, or report the
	; error in packet7 on failure.
	;
	; It is expected that output sensors add to this command
	; rather than create a new command.
	;
	; The following sensors respond to this command:
	;	SENSOR_GENERIC_DIGITAL_OUT - takes one bit, bit 0 of packet6
	; 	SENSOR_PWM_LED - takes one bytes, packet6, specifying the brightness to
	;					 fade to.
	; Now, work out what is selected. 

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	movfw packet7		; ID
	xorlw (AUTOGEN_EVERY_SENSOR_ID)
	btfsc STATUS,Z
	goto cmd_set_sensor_(AUTOGEN_EVERY_SENSOR_ID)	; sensor 1 is selected
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	; don't know this sensor! respond with a failure indicator.
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_NODE_NOT_FOUND
	movwf packet7
	return

;
; If you are creating a handler for a new sensor type, add a test here in the
; form of
;  #if (SENSOR_ID_<your identifier> == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
;    (driver code for sensor (AUTOGEN_EVERY_SENSOR_ID))
;  #endif

; SENSOR_GENERIC_DIGITAL_OUT
(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
cmd_set_sensor_(AUTOGEN_EVERY_SENSOR_ID):
	#if (SENSOR_ID_GENERIC_DIGITAL_OUT == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		btfss packet6, 0
		goto cmd_set_(AUTOGEN_EVERY_SENSOR_ID)_is_clear
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
		goto cmd_set_return_ok
cmd_set_(AUTOGEN_EVERY_SENSOR_ID)_is_clear:
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
		goto cmd_set_return_ok
	#else

		; SENSOR_PWM_LED
		#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
			movfw packet6
			bsf STATUS, RP0	; bank 1
			movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET
			bcf STATUS, RP0	; bank 0
			goto cmd_set_return_ok
		#else
			; SENSOR_TRIAC
			#if (SENSOR_ID_TRIAC == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

				; Take our byte of data and scale it from 0-255 to 0-32
				bcf STATUS, C
				rrf packet6, f
				bcf STATUS, C
				rrf packet6, w
				
				bsf STATUS, RP0	; bank 1
				movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
				bcf STATUS, RP0	; bank 0
				goto cmd_set_return_ok
	
				; Add your own handlers here, in an #if..#else block. encompass the following goto in the else.
				goto cmd_set_return_wrong_type
			#endif ; sensor_triac
		#endif	; sensor_pwm_led
	
	#endif
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

cmd_set_return_wrong_type:

	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_NODE_WRONG_TYPE
	movwf packet7
	return

cmd_set_return_ok:
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SENSOR_NONE
	movwf packet7
	return

	end