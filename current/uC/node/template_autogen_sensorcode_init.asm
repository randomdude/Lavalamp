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

		movlw 0x01
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED

		clrf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW
	#endif
	#if (SENSOR_ID_MULTILED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 8)
			#error SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT too great
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 7)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_8, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_8
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 6)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_7, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_7
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 5)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_6, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_6
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 4)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 3)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 2)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 1)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 0)
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		#endif
	#endif

	#if (SENSOR_ID_TRIAC == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TRIS, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN	
		; Set tristate for zero-crossing port, too
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ZC_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ZC_PIN	

		movlw 0x20
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
		clrf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW
	#endif

	#if (SENSOR_ID_MULTILED_MULTIPLEX == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_TRIS_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CONTROL_1_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CONTROL_1_PIN
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CONTROL_2_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CONTROL_2_PIN
	#endif
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

    bcf STATUS, RP0 ; bank 0
    bcf STATUS, RP1 ; bank 0

; Now initialise any output pins to their default states.

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
; SENSOR_(AUTOGEN_EVERY_SENSOR_ID)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	#if (SENSOR_ID_GENERIC_DIGITAL_OUT == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN	
	#endif
	#if (SENSOR_ID_MULTILED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 8)
			#error SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT too great
		#endif
		movlw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_DEFAULT
		movwf tmp1
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 7)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_8, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_8
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_8, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_8
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 6)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_7, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_7
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_7, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_7
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 5)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_6, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_6
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_6, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_6
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 4)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 3)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 2)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 1)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 0)
			rrf tmp1, f
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
			btfsc STATUS, C
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		#endif
	#endif

	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		; bank 1 for these vars!
    	bsf STATUS, RP0 ; bank 1

		movlw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_DEFAULT
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET

	    bcf STATUS, RP0 ; bank 0
	#endif

#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	return

	end