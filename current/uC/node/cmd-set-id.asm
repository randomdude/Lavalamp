
	#include "sensorcfg.h"
	#include "eeprom.h"
	#include "main.h"

	errorlevel  -302  

PROG CODE

	; This command sets the ID of the node in EEPROM and in memory. Note
	; that the new node ID is used immediately.
	; Only input is one byte of node ID in packet7.

do_cmd_set_id:
	global do_cmd_set_id

	movfw packet7
	movwf nodeid

	call saveid

	clrf packet5
	clrf packet6
	clrf packet7

	return

	end