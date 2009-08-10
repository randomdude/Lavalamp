
	#include "main.h"
	#include "debugger.h"

	errorlevel  -302  

	CODE
do_cmd_ping
	GLOBAL do_cmd_ping

	movlw DEBUGGER_GOT_CMD_PING
	call debugger_spitvalue;

	; Respond to a ping with some arbitrary data in the packet.
	clrf packet4
	movlw 0x41
	movwf packet5
	movlw 0x42
	movwf packet6
	movlw 0x43
	movwf packet7

	return

	end
