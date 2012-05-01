
; Stuff below here are variables used by sensors, which are
; 'placed' at compile-time in bank 1.

placeinmemory macro base
	local memptr = base

(AUTOGEN_BEGIN_REPLICATING_BLOCK)

#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW	EQU memptr
memptr=memptr+1
SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_HIGH EQU memptr
memptr=memptr+1
SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME   		EQU memptr
memptr=memptr+1
SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET 		EQU memptr
memptr=memptr+1
SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED 			EQU memptr
memptr=memptr+1

	#endif

	#if (SENSOR_ID_TRIAC == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW	EQU memptr
memptr=memptr+1
SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME   		EQU memptr
memptr=memptr+1

	#endif
#endif

(AUTOGEN_END_REPLICATING_BLOCK)
;
	endm

; invoke our nice shiny macro to place memory at compile-time
	placeinmemory h'A0'
