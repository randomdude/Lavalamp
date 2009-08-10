
	#include "main.h"

	errorlevel  -302  

	CODE
do_cmd_ping
	GLOBAL do_cmd_ping
	; Respond to a ping with some arbitrary data in the packet.
	movlw 0x42
	movwf packet4
	movlw 0x43
	movwf packet5
	movlw 0x44
	movwf packet6
	movlw 0x45
	movwf packet7

	return

end
