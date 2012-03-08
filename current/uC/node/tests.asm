
	#include "main.h"
	#include "..\shared\manchesteruart.h"
	#include "..\shared\manchesteruart-lowlevel.h"

	errorlevel  -302  

	code

dotests:
	global dotests

#ifdef TESTBENCH_TEST_AWAITIDLESYMBOL
	; This test will raise PORTB, 7 when it 
	; detects an idle symbol. 

	bsf STATUS, RP0	; bank 1
	bcf TRISB, 7
	bsf TRISB, 6
	bcf STATUS, RP0	; bank 0

	bsf PORTB, 7	; signal that we're ready to start
	bcf PORTB, 7

doagain:
	bcf PORTB, 7
	call a_waitidlesymbol
	bsf PORTB, 7

waitack:
	btfss PORTB, 6
	goto waitack

	goto doagain
#endif

#ifdef TESTBENCH_TEST_AWAITSTARTSYMBOL
	; This test will raise PORTB, 7 when it 
	; detects a start symbol. 

	bsf STATUS, RP0	; bank 1
	bcf TRISB, 7
	bsf TRISB, 6
	bcf STATUS, RP0	; bank 0

	bsf PORTB, 7	; signal that we're ready to start
	bcf PORTB, 7

doagain:
	bcf PORTB, 7
	call awaitstartsymbol
	bsf PORTB, 7

waitack:
	btfss PORTB, 6
	goto waitack

	goto doagain
#endif

	return

	end