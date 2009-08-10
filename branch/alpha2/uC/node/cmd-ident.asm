
	#include "main.h"

	errorlevel  -302  

	CODE
do_cmd_ident
	GLOBAL do_cmd_ident
	; This command returns four bytes of user-friendly ASCII
	; identification string. It takes one argument in byte 7
	; which is the word to respond with. Byte 8 is reserved,
	; set to 0. Strings are null-terminated.
	; Example, for a node named 'POWER-SOCKET':
	; <base> xx xx xx xx xx CMD_IDENT 00 00 
	; <node> xx xx xx xx xx P O W E
	; <base> xx xx xx xx xx CMD_IDENT 01 00 
	; <node> xx xx xx xx xx R - S O
	; <base> xx xx xx xx xx CMD_IDENT 02 00 
	; <node> xx xx xx xx xx C K E T
	; <base> xx xx xx xx xx CMD_IDENT 03 00 
	; <node> xx xx xx xx xx 00 00 00 00
	; It is the master station's responsibility to request
	; proper ranges. You may want to note that, for this 
	; reason,the contents of EEPROM are not secure against
	; an authenticated base station.

	movfw packet6		; load location
	movwf tmp1
	bcf STATUS, C
	RLF tmp1, F
	RLF tmp1, W		; multiply by 4

	clrf packet4
	clrf packet5
	clrf packet6
	clrf packet7		; clear packet ready for response
	
    bcf STATUS, RP0 ; bank 2
    bsf STATUS, RP1 ; bank 2

	addlw H'15'		; Start of friendly name in EEPROM
	movwf EEADR
	decf EEADR, F
	
	call readbyte
	movwf packet4
	btfsc STATUS, Z
	goto donethat

	call readbyte
	movwf packet5
	btfsc STATUS, Z
	goto donethat

	call readbyte
	movwf packet6
	btfsc STATUS, Z
	goto donethat

	call readbyte
	movwf packet7

donethat:
    bcf STATUS, RP0 ; bank 0
    bcf STATUS, RP1 ; bank 0

	return

readbyte:
	incf EEADR, F;
	bsf STATUS, RP0 ; hit bank 4
	bsf EECON1, RD
	bcf STATUS, RP0 ; back to bank 2
	movf EEDATA, W
	return

	end
