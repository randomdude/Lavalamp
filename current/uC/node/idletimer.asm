

	radix	hex

	#include "main.h"
	#include "sensorcfg.h"
	#include "protocol.h"
	#include "memoryplacement.h"
	#include "idle-pwm.h"

	errorlevel  -302  

	CODE

	;This timer is called 'every so often' by the Manchester-decoding 
	; function, while it looks for the start of a transmission. Don't
	; block too long here (really!) - if it doesn't return within the
	; amount of time that one manchester half-bit is transmitted, the
	; node will start observing packet start sequences, where they do
	; not exist.

	#define idletimerenabled

idletimer:
	global idletimer

	; First off, save all our crap. Code taken from the datasheet.
	MOVWF W_TEMP ;	copy W to temp register,
				 ;	could be in either bank
	SWAPF STATUS,W  ;	swap status to be saved
					;	into W
	BCF STATUS,RP0  ;	change to bank 0 regardless
	BCF STATUS,RP1  ;	of current bank
	MOVWF STATUS_TEMP ;	save status to bank 0
					  ; register

	; And our actual interrupt service routines.
	; Jump to them - we can't call, in case we overflow the
	; stack
	goto dopwmsensors
endpwmsensors:
	global endpwmsensors
	goto dotriacsensors
endtriacsensors:
	global endtriacsensors

	bcf INTCON, T0IF
	bsf INTCON, T0IE
	movlw IDLETIMER_SPEED
	movwf TMR0

	; restore context
	SWAPF STATUS_TEMP,W ;	swap STATUS_TEMP register
						;	into W, sets bank to original
						;	state
	MOVWF STATUS ;move W into STATUS register
	SWAPF W_TEMP,F ;swap W_TEMP
	SWAPF W_TEMP,W ;swap W_TEMP into W

	retfie

disableidetimer:
	global disableidetimer

#ifndef idletimerenabled
	return
#endif

	bcf INTCON, GIE
	bcf INTCON, T0IE

	return

enableidetimer:
	global enableidetimer

#ifndef idletimerenabled
	return
#endif


	bsf STATUS, RP0	; trip to bank 1
	bcf OPTION_REG, T0CS
	bcf STATUS, RP0	; bank 0

	bsf INTCON, GIE
	bsf INTCON, T0IE

	movlw IDLETIMER_SPEED	
	movwf TMR0

	return

	end