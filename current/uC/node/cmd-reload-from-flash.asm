
	#include "sensorcfg.h"
	#include "eeprom.h"
	#include "main.h"

	errorlevel  -302  

PROG CODE

	; This command sets a flag indicating that, after this command, the
	; node should re-load key information from flash.

do_cmd_reload_from_flash:
	global do_cmd_reload_from_flash

	bsf state, STATUS_BIT_NEEDRELOAD

	clrf packet5
	clrf packet6
	clrf packet7

	return
	
	end