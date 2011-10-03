	
	; Don't warn about case sensitivity
	errorlevel  -302  

	#include "p16f628.inc"

	; Set our config word
	__CONFIG _CP_OFF & _WDT_OFF & _HS_OSC & _LVP_OFF & _BODEN_OFF 

;code

;org 0

	goto start
	nop
	nop
	nop
	nop

; And here's where it all begins.
start 

; Set pin 0 of porta to output, and emit a square wave from it.

	bcf STATUS, RP1	 
	bsf STATUS, RP0  	; page 1

	clrf TRISB			; Set tristate

	bcf STATUS, RP0		; Bank 0

flashLoop

	; Do not use bcf/bsf here, since we want our simulator to 
	; report a single write, not a read-modify-write combo.
	movlw 0x01
	movwf PORTB
	clrf PORTB

	goto flashLoop


end