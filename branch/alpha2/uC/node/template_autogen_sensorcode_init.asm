	#include "sensorcfg.h"
	#include "main.h"
	#include "protocol.h"

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
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

    bcf STATUS, RP0 ; bank 0
    bcf STATUS, RP1 ; bank 0

	return

	end