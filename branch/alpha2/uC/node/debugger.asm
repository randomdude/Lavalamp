
	#include "main.h"
	#include "../shared/init.h"

	errorlevel  -302  

; Just a tiny little system for throwing diagnostic messages,
; can be quite useful in a live PIC (since I haven't got an 
; ICE yet).

; enable and configure here.
#undefine SW_DEBUGGER
#define DEBUGPORT PORTB
#define DEBUGTRIS TRISB
#define DEBUGPIN_DATA  0
#define DEBUGPIN_CLOCK 3

page0 CODE

debugger_init
	global debugger_init

#ifdef SW_DEBUGGER
	; set tristate on debugger output pins, and clear them.

	bsf STATUS, RP0 ; bank 1

	bcf DEBUGTRIS, DEBUGPIN_DATA	
	bcf DEBUGTRIS, DEBUGPIN_CLOCK

	bcf STATUS, RP0	; bank 0

	bcf DEBUGPORT, DEBUGPIN_DATA
	bcf DEBUGPORT, DEBUGPIN_CLOCK
#endif
	return

debugger_spitvalue
	global debugger_spitvalue
#ifdef SW_DEBUGGER
	movwf dbg1 
	movlw 0x08
	movwf dbg2

	bcf DEBUGPORT, DEBUGPIN_CLOCK
	call delay
	bsf DEBUGPORT, DEBUGPIN_DATA
	call delay
	bsf DEBUGPORT, DEBUGPIN_CLOCK
	call delay

nextbit:
	call delay
	bsf DEBUGPORT, DEBUGPIN_CLOCK

	btfsc dbg1, 0
	goto setdebugpin
	goto cleardebugpin

setdebugpin
	bsf DEBUGPORT, DEBUGPIN_DATA
	goto debugpindone
cleardebugpin
	bcf DEBUGPORT, DEBUGPIN_DATA
debugpindone:

	bcf DEBUGPORT, DEBUGPIN_CLOCK

	rrf dbg1, F
	decfsz dbg2, F
	goto nextbit

	bsf DEBUGPORT, DEBUGPIN_CLOCK
	call delay

	return

delay
	nop
	nop
	return

	movlw 0x02
	movwf dbg3
delay_wait
	decf tmp3, F
	btfss STATUS, Z
	goto delay_wait
#endif
	return

	end