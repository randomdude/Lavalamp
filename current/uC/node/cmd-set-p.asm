
	#include "sensorcfg.h"
	#include "main.h"
	errorlevel  -302  

PROG CODE

	; These two commands, cmd_set_p_low and cmd_set_p_high, set bytes
	; 0-1 and 2 of 'p', the cryptographic seed stored in flash, respectively.
	; Please note that this functionality is mainly intended for debugging
	; the crypto algorithm, and testing under various circumstances, and is
	; a fairly 'advanced' function. 
	; Obviously, since two commands are used to set the value, P will change
	; between them (as p is incremented for each transaction). Isn't code fun?
	; Bytes are stored in packet6-packet7 for _low (bytes 0-1 of p) and packet7
	; for _high (byte 2 of p). 
	; Note that we do not flush this to eeprom here; this is done by the callee.

do_cmd_set_p_low:
	global do_cmd_set_p_low

	movfw packet6
	movwf p1
	movfw packet7
	movwf p2

	clrf packet5
	clrf packet6
	clrf packet7

	return

do_cmd_set_p_high:
	global do_cmd_set_p_high

	movfw packet7
	movwf p3

	clrf packet5
	clrf packet6
	clrf packet7

	return

end