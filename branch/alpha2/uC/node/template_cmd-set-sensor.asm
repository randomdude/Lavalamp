
	#include "main.h"
	#include "sensorcfg.h"
	#include "processnamedcmd.asm"
	#include "debugger.h"
	#include "protocol.h"

	errorlevel  -302  

	CODE
do_cmd_set_sensor
	GLOBAL do_cmd_set_sensor

	; This command takes one byte of sensor ID, in packet7,
	; and one byte of state data in packet6.The sensor will
	; respond with 0x00000000 on success, or report the
	; error in packet7 on failure.
	; It is expected that output sensors add to this command
	; rather than create a new command.

	movlw DEBUGGER_GOT_CMD_SETSENSOR
	call debugger_spitvalue;

	; Now, work out what is selected. 

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	movfw packet7
	xorlw h'(AUTOGEN_EVERY_SENSOR_ID)'
	btfsc STATUS,Z
	goto cmd_set_sensor_(AUTOGEN_EVERY_SENSOR_ID)	; sensor 1 is selected
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	movlw DEBUGGER_UNKNOWN_SENSOR
	call debugger_spitvalue;

	; don't know this sensor! respond with a failure indicator.
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_NODE_NOT_FOUND
	movwf packet7
	return

;
; If you are creating a new sensor type, add a test here in the form of
;  #if (SENSOR_ID_<your identifier> == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
;    (driver code for sensor (AUTOGEN_EVERY_SENSOR_ID))
;  #endif
;
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
		goto cmd_set_return_wrong_type
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
	movlw ERR_SET_SENSOR_NONE
	movwf packet7
	return

	end