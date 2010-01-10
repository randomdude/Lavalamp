
	radix	hex

	#include "main.h"
	#include "sensorcfg.h"
	#include "protocol.h"
	#include "idletimer.h"
	#include "memoryplacement.h"

	errorlevel  -302  

	CODE

dopwmsensors:
	global dopwmsensors

(AUTOGEN_BEGIN_REPLICATING_BLOCK)

#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

	bsf STATUS, RP0 ; bank 1

	; Inc timer_low. this is the value which is used to toggle the
	; output HIGH or LOW
	movlw 0x0A
	addwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW, f
	btfss STATUS, C
	goto dontdofade(AUTOGEN_EVERY_SENSOR_ID)	; Only proceed in fading the LED towards
												; TARGET if this is at 0, to give some delay.
												; Alternatively, if PWM_SPEED is zero, just go there immediately.
	movf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED, f
	btfsc STATUS, Z
	goto updateImmediately(AUTOGEN_EVERY_SENSOR_ID)

dofade(AUTOGEN_EVERY_SENSOR_ID):
	; update our rolling timer. is it time to fade slightly?
	decfsz SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_HIGH, f
	goto dontdofade(AUTOGEN_EVERY_SENSOR_ID)

	; Yup, time to fade. Reset the high portion of the rolling
	; timer first..
	movfw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_SPEED
	movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_HIGH

	; and then fade towards the target.
	movfw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
	subwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET, w
	btfsc STATUS, Z
	goto dontdofade(AUTOGEN_EVERY_SENSOR_ID)		; Target = volume

	btfss STATUS, C
	goto fadedown(AUTOGEN_EVERY_SENSOR_ID)

	incf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME, f		; fade up!
	goto dontdofade(AUTOGEN_EVERY_SENSOR_ID)
fadedown(AUTOGEN_EVERY_SENSOR_ID):
	decf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME, f		; fade down!
	goto dontdofade(AUTOGEN_EVERY_SENSOR_ID)
updateImmediately(AUTOGEN_EVERY_SENSOR_ID):
	movfw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET
	movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME

dontdofade(AUTOGEN_EVERY_SENSOR_ID):
	; perform PWM according to the value of TIMER_LOW.
	movfw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_ROLLING_TIMER_LOW
	subwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME, w
	btfss STATUS, C
	goto overlimit(AUTOGEN_EVERY_SENSOR_ID)

	bcf STATUS, RP0 ; bank 0
	bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
	goto retnow(AUTOGEN_EVERY_SENSOR_ID)

overlimit(AUTOGEN_EVERY_SENSOR_ID):
	nop		; This is here to preserve timing. Because the above btfss causes a pipeline stall,
	nop  	; we slow down the non-stalled branch with a couple nops.

	bcf STATUS, RP0 ; bank 0
	bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN

retnow(AUTOGEN_EVERY_SENSOR_ID):

	#endif
#endif

(AUTOGEN_END_REPLICATING_BLOCK)
;
	bcf STATUS, RP0 ; bank 0

	goto endpwmsensors

	end