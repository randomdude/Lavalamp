
	#include "main.h"
	#include "eeprom.h"

	errorlevel  -302  

	CODE
do_cmd_ident
	GLOBAL do_cmd_ident
	; This command returns four bytes of user-friendly ASCII
	; identification string. It takes one argument in byte 7
	; which is the word to respond with. Byte 8 is reserved,
	; set to 0. Strings are null-terminated.
	; Example, for a node named 'POWERSOCKET':
	; <base> xx xx xx xx ourID CMD_IDENT 00 00 
	; <node> id xx xx xx 0x00   P O W
	; <base> xx xx xx xx ourID CMD_IDENT 01 00 
	; <node> id xx xx xx 0x00   E R S
	; <base> xx xx xx xx ourID CMD_IDENT 02 00 
	; <node> id xx xx xx 0x00   O C K 
	; <base> xx xx xx xx ourID CMD_IDENT 03 00 
	; <node> id xx xx xx 0x00   E T \0
	; It is the master station's responsibility to request
	; proper ranges. You may want to note that, for this 
	; reason,the contents of EEPROM are not secure against
	; an authenticated base station.

	movfw packet6		; load location
	movwf tmp1
	bcf STATUS, C
	RLF tmp1, W		;   multiply by 2
	addwf tmp1, W	; + add 1
					; = multiply by 3

	clrf packet5
	clrf packet6
	clrf packet7		; clear packet ready for response
	
    bsf STATUS, RP0 ; bank 1
    bcf STATUS, RP1 ; bank 1

	addlw H'14'		; Start of friendly name in EEPROM
	movwf EEADR
	decf EEADR, F
	
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

	return


	end
