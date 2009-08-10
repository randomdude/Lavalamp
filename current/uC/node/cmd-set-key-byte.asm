
	#include "sensorcfg.h"
	#include "eeprom.h"
	#include "main.h"

	errorlevel  -302  

PROG CODE

	; This command sets a byte of key in EEPROM. The value won't be
	; used until you issue a command to re-read the contents of flash.
	; Specify the target byte index in packet6, and the new byte in 
	; packet7.

	; todo - verify value was written correctly (?)

do_cmd_set_key_byte:
	global do_cmd_set_key_byte

	movfw packet6
	andlw 0x0F			; only allow 16 bytes to be written!
	bsf STATUS, RP0 	; bank 1
	addlw EEPROM_KEY
	movwf EEADR
	bcf STATUS, RP0 	; bank0

	movfw packet7
	call writeabyte

	clrf packet5
	clrf packet6
	clrf packet7

	return

	end