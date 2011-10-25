

	radix	hex

	#include "main.h"
	#include "tea.h"
	#include "../shared/init.h"
	#include "../shared/manchesteruart.h"
	#include "../shared/manchesteruart-lowlevel.h"
	#include "idletimer.h"
	#include "sensorcfg.h"

	errorlevel  -302  

	CODE

sendpacket:
	GLOBAL sendpacket

#ifdef COMMLINK_HWUART
	goto sendpackethwuart
#endif

#ifdef COMMLINK_SWMANCHESTER
	goto sendpackethwuart
#endif

#ifdef COMMLINK_SWMANCHESTER
sendpacketmanchester:
	; Sent a packet of data out via the software manchester
	; UART.

	; Since we're doing a lot of bit-banging here, disable
	; interrupts.
	call disableidetimer

	clrf packet4	; address to base station
	movlw h'aa'
	movwf packet3	; packet3 is currently unused
	; Now encrypt it	
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call encrypt

	; We send a load of transitions in order that
	; the other end sees our signal properly. First
	; some really fast (mhz-ish) transitions then
	; some sensibly-long ones.

	movlw LEADIN_CYCLES
	movwf bitcnt
idlebits2:
	call sendidlesymbol

	decfsz bitcnt, f
	goto idlebits2

	; send start symbol
	call sendstartsymbol

	; send the real data
	movfw packet0
	call sendasmanchester
	movfw packet1
	call sendasmanchester
	movfw packet2
	call sendasmanchester
	movfw packet3
	call sendasmanchester
	movfw packet4
	call sendasmanchester
	movfw packet5
	call sendasmanchester
	movfw packet6
	call sendasmanchester
	movfw packet7
	call sendasmanchester

	; leave that RF transmitter disabled! 
	bcf RF_TX_PORT, RF_TX_PIN

	call enableidetimer

	return
#endif

#ifdef COMMLINK_HWUART
sendpackethwuart:
; This crypts a packet,and sends a packet out through the hardware UART.
	GLOBAL sendpackethwuart

#ifndef TEST_TRANSMISSION
	; Address to transmitter
	movlw 0x22
	movwf packet4
#endif

	; Now encrypt it	
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call encrypt

	movfw packet0
	call sendbytehwuart
	movfw packet1
	call sendbytehwuart
	movfw packet2
	call sendbytehwuart
	movfw packet3
	call sendbytehwuart
	movfw packet4
	call sendbytehwuart
	movfw packet5
	call sendbytehwuart
	movfw packet6
	call sendbytehwuart
	movfw packet7
	call sendbytehwuart
	
	; Flash a pin so our testbench knows that IO is over.

;	bsf PORTA, 2
;	bcf PORTA, 2

	return

sendbytehwuart:
	global sendbytehwuart
	; Send 'w' to the HW UART.
waituntilidle1:
	btfss PIR1, TXIF	; are we idle?
	goto waituntilidle1	; wait for idleness!
	movwf TXREG
	bsf STATUS, RP0  ; page 1
	bsf TXSTA, TXEN
	bcf STATUS, RP0  ; page 0
waituntilidle2:			; we wait for our packet to finish sending before returning
	btfss PIR1, TXIF	; are we idle?
	goto waituntilidle2	; wait for idleness!

	return
#endif

waitforpacket:
	GLOBAL waitforpacket
#ifdef COMMLINK_HWUART
	goto waitforpackethwuart
#endif
#ifdef COMMLINK_SWMANCHESTER
	goto recasmanchester
#endif

#ifdef COMMLINK_HWUART
resetsyncbytesandwaitforpackethwuart
	movlw 0x08
	movwf syncbytes

	; reset to 'no connections'
	; TODO: Move?
	bcf state, STATUS_BIT_CRYPTOSTATE
waitforpackethwuart:
	global waitforpackethwuart
	; Await a packet of data from the hardwre UART. If we see
	; eight consecutive 0xAA bytes - a sync packet - we reset
	; to the start of the buffer.

	call waitforbytehwuart
	movwf packet0

	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet1
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet2
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet3
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet4
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet5
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet6
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	call waitforbytehwuart
	movwf packet7
	movf syncbytes, f
	btfsc STATUS, Z			; have we seen enough sync bytes?
	goto onSyncPacket		; Yup. loop up.

	return

onSyncPacket:
	; Eight consecutive sync bytes have been recieved. We should
	; restart our read, and pulse the debug pin if one is set.
#ifdef DEBUG_PULSE_ON_SYNC
	bsf DEBUG_PULSE_ON_SYNC_PORT, DEBUG_PULSE_ON_SYNC_PIN
	bcf DEBUG_PULSE_ON_SYNC_PORT, DEBUG_PULSE_ON_SYNC_PIN
#endif

	goto resetsyncbytesandwaitforpackethwuart

waitforbytehwuart:
	btfsc PIR1, RCIF		; data waiting?
	goto okgotbyte

	; Check for terrible things
;	btfsc RCSTA, FERR
;	goto ohshit
	btfsc RCSTA, OERR
	goto ohshit

	goto waitforbytehwuart

okgotbyte:
	movfw RCREG	

	; If it's a sync byte, dec our sync counter (which starts at 8).
	xorlw 0xAA
	btfsc STATUS, Z
	goto isSyncByte

	; Not a sync byte. Clear our sync byte counter.
	; We do this like this to avoid using a extra byte of memory.
	clrf syncbytes
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f
	incf syncbytes, f

	; W contains the recieved byte XOR 0xAA, so restore it to our recieved byte.
	xorlw 0xAA

	return

isSyncByte:
	decf syncbytes, f

	; W contains the recieved byte XOR 0xAA, so restore it to our recieved byte.
	xorlw 0xAA

	return

ohshit:
;	bsf PORTA, 1
;	bcf PORTA, 1
	goto ohshit

#endif
	end
