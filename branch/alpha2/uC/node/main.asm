

	radix	hex

	#include "main.h"
	#include "tea.h"
	#include "../shared/init.h"
	#include "protocol.h"
	#include "cmds.h"
	#include "maths.h"
	#include "testbench.h"
	#include "debugger.h"
	#include "../shared/swuart.h"
	#include "eeprom.h"

	errorlevel  -302  


; Enable this to 'insert' a debug packet, and disable the auth
; procedure. Handy for debugging through sensor procedures. 
; Don't define it in a real PIC!
#undefine SIM_DEBUG 

	__CONFIG _CP_OFF & _WDT_OFF & _HS_OSC & _LVP_OFF & _BODEN_OFF
STARTUP CODE
	goto start
	nop
	nop
	nop
	nop

PROG CODE

start 
	global start

	call init
	call debugger_init

	movlw DEBUGGER_INITTED
	call debugger_spitvalue;

#ifdef TESTBENCH
	goto testbench_main
#endif

	movlw DEBUGGER_AWAITING_FIRST_PACKET
	call debugger_spitvalue;

	clrf state	; set state to 'waiting for first packet' as opposed to 'waiting for transciever to auth'
waitfordata
	; Get a 64bit packet of data.
#ifndef SIM_DEBUG				; define only during simulation!
	call recasmanchester
#else
	; if we're debugging, and the devver has set SIM_DEBUG, we
	; should operate as if we recieved this encrypted packet.
	movlw 0x01
	movwf packet0
	movlw 0x1b
	movwf packet1
	movlw 0x81
	movwf packet2
	movlw 0xd2
	movwf packet3
	movlw 0xec
	movwf packet4
	movlw 0x2a
	movwf packet5
	movlw 0x86
	movwf packet6
	movlw 0x4d
	movwf packet7
#endif

	movlw DEBUGGER_GOT_CMD
	call debugger_spitvalue;

	; debugging - dump recieved packet
	;call sendpackethwuart

	; decrypt packet
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call decrypt
	;	debug code - echo decrypted packet back over RF
	;	call sendpacket

	; We need to check what state the system is in. We either need
	; to respond with [n+1 p] (see readme on auth) or verify p+1
	; and perform an action.
#ifndef SIM_DEBUG		; define only during simulation!
	movfw state
	btfsc STATUS, Z
	goto sendp
#endif
	goto processcmd

sendp:
	; Are you talking to me, punk?
	movfw packet4
	xorwf nodeid, W
	btfss STATUS, Z
	goto waitfordata	; Nope, not us. Wait for the next pkt.

	movlw DEBUGGER_GOT_MY_CMD
	call debugger_spitvalue;

	; Packet will be [ 0xNNNNNNNN ournodeID rnd rnd rnd ].
	; We must respond with [0xNNNNNNNN+1  0x00PPPPPP ].
	; See readme for details.
	movlw 0x01
	movwf arg4
	clrf arg3
	clrf arg2
	clrf arg1

	movfw packet0
	movwf arg5
	movfw packet1
	movwf arg6
	movfw packet2
	movwf arg7
	movfw packet3
	movwf arg8

	call addargs

	movfw arg1
	movwf packet0
	movfw arg2
	movwf packet1
	movfw arg3		; these are N+1
	movwf packet2
	movfw arg4
	movwf packet3

	movfw p1
	movwf packet7
	movfw p2
	movwf packet6
	movfw p3
	movwf packet5

	; address to transmitter
	clrf packet4
	
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call encrypt
	call sendpacket

	incf state, F			; update status
	goto resync 

processcmd:
	; First off, make sure the base station gave the correct P.

	movlw 0x01
	movwf arg4
	clrf arg3
	clrf arg2
	clrf arg1

	movfw p1
	movwf arg8
	movfw p2
	movwf arg7
	movfw p3
	movwf arg6
	clrf arg5
	call addargs

#ifndef SIM_DEBUG				
	movfw arg2
	xorwf packet1, W
	btfss STATUS, Z
	goto wrongP
	movfw arg3
	xorwf packet2, W
	btfss STATUS, Z
	goto wrongP
	movfw arg4
	xorwf packet3, W
	btfss STATUS, Z
	goto wrongP
#endif
	; Yes, it did. save the inc'ed p for use with the next packet,
	; and write it back to EEPROM, to make sure it's fresh at next
	; poweron.
	movfw arg4
	movwf p1
	movfw arg3
	movwf p2
	movfw arg2
	movwf p3
	; save to eeprom
	call savep

	; Now, we can go about processing the command given.
	call processpacketcmd;

	goto finishedChallenge

wrongP:

	; We got the wrong P? That's odd. 

	movlw DEBUGGER_GOT_WRONG_P
	call debugger_spitvalue;

	; Since there is no timeout on the 'state' of the system, it
	; is possible that a packet was dropped at some point and
	; what we actually recieved was a first-state packet and not
	; a second.. so let's try parsing it as that.

	goto sendp

finishedChallenge:

	clrf state				; reset to 'idle'
	goto resync 

processpacketcmd:
	; Right, aces. What command is it?
	movlw CMD_PING
	xorwf packet5, W
	btfsc STATUS, Z		; Is it a CMD_PING?
	call do_cmd_ping

	movlw CMD_IDENT
	xorwf packet5, W
	btfsc STATUS, Z
	call do_cmd_ident

	movlw CMD_GETSENSOR
	xorwf packet5, W
	btfsc STATUS, Z
	call do_cmd_get_sensor

	movlw CMD_SETSENSOR
	xorwf packet5, W
	btfsc STATUS, Z
	call do_cmd_set_sensor

	movlw CMD_GETSENSORTYPE
	xorwf packet5, W
	btfsc STATUS, Z
	call do_cmd_get_sensor_type

	; We need to respond with P+2, but since we already inc'ed it, we
	; can just send p+1.
	movlw 0x01
	movwf arg4
	clrf arg3
	clrf arg2
	clrf arg1

	clrf arg5
	movfw p3
	movwf arg6
	movfw p2
	movwf arg7
	movfw p1
	movwf arg8

	call addargs

	movfw arg4
	movwf packet3
	movfw arg3
	movwf packet2
	movfw arg2
	movwf packet1

	clrf packet4	; address to base station

	; Sorted. Now crypt it	
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call encrypt
	; and then send it.
	call sendpacket

	return

resync
	;Here, we wait for a stable idle seq.
	bcf PORTB,0 
	bsf PORTB,0 
	call awaitidlesymbol
	bcf PORTB,0 
	bsf PORTB,0 
	goto waitfordata	; wait for the next packet.

	end
