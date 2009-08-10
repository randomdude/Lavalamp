	#include "sensorcfg.h"
	#include "main.h"
	#include "protocol.h"
	#include "memoryplacement.h"

	errorlevel  -302  

	CODE

; TRISTATES
autogen_sensors_init:
	global autogen_sensors_init

    bsf STATUS, RP0 ; bank 1
    bcf STATUS, RP1 ; bank 1

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
; SENSOR_(AUTOGEN_EVERY_SENSOR_ID)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	#if (SENSOR_ID_GENERIC_DIGITAL_IN == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TRIS, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN	
	#endif
	#if (SENSOR_ID_GENERIC_DIGITAL_OUT == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TRIS, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN	
	#endif
	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TRIS, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN	

		; Yes, we need to remain in bank 1
		movlw 0x01
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
		movlw 0xFF
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET
		movlw 0x10
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED

	#endif
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

    bcf STATUS, RP0 ; bank 0
    bcf STATUS, RP1 ; bank 0

	return

	end