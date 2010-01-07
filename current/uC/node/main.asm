

	radix	hex

	#include "main.h"
	#include "sensorcfg.h"
	#include "tea.h"
	#include "../shared/init.h"
	#include "protocol.h"
	#include "cmds.h"
	#include "maths.h"
	#include "eeprom.h"
	#include "idletimer.h"
	#include "tests.h"

	errorlevel  -302  

	__CONFIG _CP_OFF & _WDT_OFF & _HS_OSC & _LVP_OFF & _BODEN_OFF


#ifndef COMMLINK_HWUART 
#ifndef COMMLINK_SWMANCHESTER
error "Define either COMMLINK_HWUART or COMMLINK_SWMANCHESTER"
#endif
#endif

#ifdef COMMLINK_HWUART 
#ifdef COMMLINK_SWMANCHESTER
error "Do not define both COMMLINK_HWUART and COMMLINK_SWMANCHESTER"
#endif
#endif
	
	page CODE
	ORG 0x00
  	goto start
	nop
	nop
	nop
	goto idletimer

PROG CODE

start 
	global start

	call init
	;call dotests

	; set state to 'waiting for first packet' as opposed to 'waiting for transciever to auth'
	bcf state, STATUS_BIT_CRYPTOSTATE
waitfordata
	; Get a 64bit packet of data.
	call waitforpacket
#ifdef lol
	bsf state, STATUS_BIT_CRYPTOSTATE
	movlw 0xd6
	movwf packet0
	movlw 0x82
	movwf packet1
	movlw 0xac
	movwf packet2
	movlw 0x62
	movwf packet3
	movlw 0xa4
	movwf packet4
	movlw 0xa4
	movwf packet5
	movlw 0xb3
	movwf packet6
	movlw 0xeb
	movwf packet7

	movlw 0xd3
	movwf packet0
	movlw 0xe0
	movwf packet1
	movlw 0xa3
	movwf packet2
	movlw 0xf9
	movwf packet3
	movlw 0x5d
	movwf packet4
	movlw 0x58
	movwf packet5
	movlw 0x9a
	movwf packet6
	movlw 0x87
	movwf packet7
#endif
	; decrypt packet
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call decrypt

	;call sendpacket

	; Is the packet addressed to our node?
	movfw packet4
	xorwf nodeid, W
	btfss STATUS, Z
	goto waitfordata	; Nope, not us. Wait for the next pkt.

	; We need to check what state the system is in. We either need
	; to respond with [n+1 p] (see readme on auth) or verify p+1
	; and perform an action.
	btfsc state, STATUS_BIT_CRYPTOSTATE
	goto processcmd

	; We're in the unauthenticated phase of the protocol. Respond in bits 0 to 2
	; and challenge the server in bits 5 through 7.
sendp:
	; Packet will be       [ 0xNNNNNN   (8bit unused) ournodeID rnd rnd rnd ].
	; We must respond with [ 0xNNNNNN+1 (8bit unused)  0x00      PP PP  PP ].
	; See readme for details.
	movlw 0x01
	movwf arg4
	clrf arg3
	clrf arg2
	clrf arg1

	movfw packet0
	movwf arg6
	movfw packet1
	movwf arg7
	movfw packet2
	movwf arg8

	call addargs

	movfw arg2
	movwf packet0
	movfw arg3
	movwf packet1
	movfw arg4		; these are N+1
	movwf packet2

	movfw p1
	movwf packet7
	movfw p2
	movwf packet6
	movfw p3
	movwf packet5

	call sendpacket

	bsf state, STATUS_BIT_CRYPTOSTATE			; update status, 
	goto resync 

processcmd:
	; Did the base station authenticate properly? is P correct?

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

	movfw arg2
	xorwf packet0, W
	btfss STATUS, Z
	goto wrongP

	movfw arg3
	xorwf packet1, W
	btfss STATUS, Z
	goto wrongP

	movfw arg4
	xorwf packet2, W
	btfss STATUS, Z
	goto wrongP

	; Yes, it did. save the inc'ed p for use with the next packet,
	; and write it back to EEPROM, to make sure it's fresh at next
	; poweron.
	movfw arg4
	movwf p1
	movfw arg3
	movwf p2
	movfw arg2
	movwf p3
	; we save these values to eeprom after the command is processed
	; to allow the command to modify them -ie, the command to set P

	; Now, we can go about processing the command given.
	call processpacketcmd;

	; save p to eeprom
	call savep

	; OK, this command has finished processing. Finish up any crypto if neccesary, then
	; wait for the next packet.
	goto finishedChallenge

wrongP:
	goto waitfordata;
	; We got the wrong P? That's odd. 
	; Since there is no timeout on the 'state' of the system, it
	; is possible that a packet was dropped at some point and
	; what we actually recieved was a first-state packet and not
	; a second.. so let's try parsing it as that.
	goto sendp

finishedChallenge:
	; reset to 'idle'
	bcf state, STATUS_BIT_CRYPTOSTATE
	goto resync 

processpacketcmd:
	; Right. We have a command, and any authentication neccesary has been performed.
	; What command is it?
	movlw CMD_PING
	xorwf packet5, W
	btfss STATUS, Z		; Is it a CMD_PING?
	goto notcmdping
	call do_cmd_ping
	goto cmddone

notcmdping:
	movlw CMD_IDENT
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdident
	call do_cmd_ident
	goto cmddone

notcmdident:
	movlw CMD_GETSENSOR
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdgetsensor
	call do_cmd_get_sensor
	goto cmddone

notcmdgetsensor:
	movlw CMD_SETSENSOR
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetsensor
	call do_cmd_set_sensor
	goto cmddone

notcmdsetsensor:
	movlw CMD_GETSENSORTYPE
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetsensortype
	call do_cmd_get_sensor_type
	goto cmddone

notcmdsetsensortype:
	movlw CMD_SETPLOW
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetplo
	call do_cmd_set_p_low
	goto cmddone

notcmdsetplo:
	movlw CMD_SETPHIGH
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetphigh
	call do_cmd_set_p_high
	goto cmddone

notcmdsetphigh:
	movlw CMD_SETID
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetid
	call do_cmd_set_id
	goto cmddone

notcmdsetid:
	movlw CMD_SET_KEY_BYTE
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetkeybyte
	call do_cmd_set_key_byte
	goto cmddone

notcmdsetkeybyte:
	movlw CMD_RELOAD_FROM_FLASH
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdreload
	call do_cmd_reload_from_flash
	goto cmddone

notcmdreload:
	movlw CMD_SETSENSORFADESPEED
	xorwf packet5, W
	btfss STATUS, Z
	goto notcmdsetsensorfadespeed
	call do_cmd_set_sensor_fade_speed
	goto cmddone

notcmdsetsensorfadespeed:

	; No idea what this command was! Crap. Let's just ignore it.
	return

cmddone:
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
	movwf packet2
	movfw arg3
	movwf packet1
	movfw arg2
	movwf packet0

	; and then send it.
	call sendpacket

	return

resync

	; Command has ended. Do anything pending
	btfsc state, STATUS_BIT_NEEDRELOAD
	call init_from_eeprom
	bcf state, STATUS_BIT_NEEDRELOAD

#ifdef COMMLINK_SWMANCHESTER
	;Here, we wait for a stable idle seq.
	call awaitidlesymbol
#endif

	goto waitfordata

	end
