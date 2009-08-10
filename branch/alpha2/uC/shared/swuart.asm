
	#include "main.h"
;	#include "init.h"
#ifndef IS_TRANSMITTER
	#include "debugger.h"
#endif
	errorlevel  -302 

; Code to operate a software Manchester-encoded Rx/Tx module.
;
; Operates on pins RF_TX_PORT/RF_TX_PIN and RF_RX_PORT/RF_RX_PIN 
; and at a bit period set by BITPAUSEHI/BITPAUSELO.
; 
; For reference, one 'bit period' is half the time it takes
; to xmit or recieve one bit of 'real' non-manchester data.
;
; Manchester-encoded values are marked by the start sequence of
; 0110, or '10' non-Manchestered.

	; these twodefines will throw out some debug info
	#define debugging 
	#define debugoutput PORTB,0

	CODE

initswuart:
	global initswuart
	; initialise the software UART
	bsf STATUS, RP0 ; trip to bank 1
	bcf RF_TX_TRIS, RF_TX_PIN
	bsf RF_RX_TRIS, RF_RX_PIN
	bcf STATUS, RP0 ; trip back to bank 0
	return

awaitidlesymbol:
	global awaitidlesymbol

#ifdef debugging
	bsf debugoutput
#endif

	; Here we wait for a 010 sequence, where '1' is at least as
	; long as one manchester bit period.
waitstart3:
	btfss RF_RX_PORT, RF_RX_PIN
	goto waitstart3
	; time the wait for LOW
	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 

	movlw BITPAUSEHI
	movwf TMR1H
	movlw BITPAUSELO
	movwf TMR1L

	movfw TMR1L
	addlw 0xA0
	movwf TMR1L
	btfsc STATUS, C
	incf TMR1H,f 

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going

timebit2:
	btfsc RF_RX_PORT, RF_RX_PIN
	goto timebit2

#ifdef debugging
;	bcf debugoutput
;	bsf debugoutput
#endif

	bcf T1CON, TMR1ON		; stop timer
	; did that take less than one bit period?
	btfss PIR1, TMR1IF
	goto waitstart3

#ifdef debugging
	bcf debugoutput
	bsf debugoutput
	bcf debugoutput
	bsf debugoutput
#endif

	return

awaitstartsymbol:
	global awaitstartsymbol

	; Wait for our start sequence of 0110.
	; we do this by awaiting a falling edge, and then sampling at
	; every bit-period therafter.
	;
	; todo: make nonblocking, possibly switch to interrupt-driven
	; sampling? I'd rather keep interrupts free for the end-users
	; code than use them here if poss.

	; wait for first HIGH part of 0110
waitstart1:
#ifdef debugging
	bcf debugoutput
#endif
	btfss RF_RX_PORT, RF_RX_PIN
	goto waitstart1

	; time the wait for LOW

	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 

	movlw BITPAUSEHI
	movwf TMR1H
	movlw BITPAUSELO
	movwf TMR1L

	; If we have waited for more than two bit periods, we know we
	; have recieved the manchester-encoded sequence 01 (1001 'raw
	; on the wire'). 
	; 
	; Wire      : 0110
	; Manchester:  1 0
	;
	; so we multiply the bit timer by two, then compare this to the
	; wait for a rising edge. Because the bit timer is stored as 
	; (0xffff-delay), we do actually divide it by two, so then we
	; get a bigger delay value.
	; Note that we also subtract some 'safety time' from the delay
	; period,as this is the same value that the other station will
	; be using for a delay.

	bsf STATUS, C
	rlf TMR1H, f
	rlf TMR1L, f

	movfw TMR1L
	addlw 0x30
	movwf TMR1L
	btfsc STATUS, C
	incf TMR1H,f 
	
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going

timebit:
	btfsc RF_RX_PORT, RF_RX_PIN
	goto timebit

	bcf T1CON, TMR1ON		; stop timer
	; did that take less than two bit periods? If so, it
	; wans't a start-of-data marker.
	btfss PIR1, TMR1IF
	goto waitstart1

#ifdef debugging
	bsf debugoutput
	bcf debugoutput
	bsf debugoutput
	bcf debugoutput
	bsf debugoutput
	bcf debugoutput
#endif

	return

recasmanchester:
	; get data. We get 8 bytes.
	global recasmanchester
	
	; wait for idle symbol. When we recieve this, we can be reasonably
	; sure that we are listening to the node and not random noise.
	call awaitidlesymbol
	call awaitidlesymbol
	call awaitidlesymbol

	; wait until start symbol occurs
	call awaitstartsymbol
	
	movlw 0x08
	movwf tmp6			; collect 8 bytes

recnextbyte:
	movlw 0x08
	movwf bitcnt		; collect 8 bits per byte

nextbit
	; At this point, we are sync'ed to the midpoint of the
	; previous manchester codeword. Wait 1.5 codewords-til
	; in the first half of the new bit.
	call bitpause
	call halfbitpause

	; sample (and invert)!
	bcf STATUS, C
	btfss RF_RX_PORT, RF_RX_PIN	
	bsf STATUS, C

	rlf bytetosend, f

#ifdef debugging
	bsf debugoutput
	bcf debugoutput
	btfsc RF_RX_PORT, RF_RX_PIN	
	bsf debugoutput
#endif

	; To make culmative error a non-issue, wait until the
	; transition in the middle of the manchester codeword

	; sync with midpoint transition 
	movfw RF_RX_PORT  ; get current input state
	andlw 1<<RF_RX_PIN
	btfss STATUS, Z	  ; if currently high
	goto waituntillow
	goto waituntilhigh
; todo: optimise this
waituntillow
	movfw RF_RX_PORT 
	andlw 1<<RF_RX_PIN
	btfss STATUS, Z
	goto waituntillow
	goto midsynced
waituntilhigh
	movfw RF_RX_PORT 
	andlw 1<<RF_RX_PIN
	btfsc STATUS, Z
	goto waituntilhigh
	goto midsynced
midsynced:
; We are now sync'ed to the transition in the middle of a
; Manchester codeword.

	; if not the end of a byte, loop up
	decfsz bitcnt,f 
	goto nextbit

	; a byte is finished. save it
	; TODO : Neaten this
	movfw tmp6
	movwf tmp7
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte8
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte7
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte6
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte5
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte4
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte3
	decf tmp7, F
	btfsc STATUS, Z
	goto loadbyte2
	decf tmp7, F 
	btfsc STATUS, Z
	goto loadbyte1

loadbyte1
	movfw bytetosend
	movwf packet0
	goto finishedsavingbyte
loadbyte2
	movfw bytetosend
	movwf packet1
	goto finishedsavingbyte
loadbyte3
	movfw bytetosend
	movwf packet2
	goto finishedsavingbyte
loadbyte4
	movfw bytetosend
	movwf packet3
	goto finishedsavingbyte
loadbyte5
	movfw bytetosend
	movwf packet4
	goto finishedsavingbyte
loadbyte6
	movfw bytetosend
	movwf packet5
	goto finishedsavingbyte
loadbyte7
	movfw bytetosend
	movwf packet6
	goto finishedsavingbyte
loadbyte8
	movfw bytetosend
	movwf packet7
	goto finishedsavingbyte

finishedsavingbyte

	; have we finished recieving 8 bytes?
	decfsz tmp6, F
	goto recnextbyte		; nope, wait for the next byte.

	; we have finished getting all bytes.
#ifdef debugging
	bsf debugoutput
#endif
	;	call sendpackethwuart	; debug - send echo packet out over hw uart
	return

sendidlesymbol:
	global sendidlesymbol

	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	
	return

sendstartsymbol:
	global sendstartsymbol
	; Send a start word, 0110 
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause

	return

; sendasmanchester - this will send the byte in bytetosend out
; over the  software UART defined by RF_TX_ .., via Manchester
; encoding. NOTE that this doesn't send a start sequence.
sendasmanchester:
	global sendasmanchester

	movwf bytetosend
	movfw bytetosend	;save it
	movwf tmp2			; dupe it?

	movlw 0x08
	movwf bitcnt	; send 8 bits

sam_nextbit:
#ifdef debugging
	bsf debugoutput
	bcf debugoutput
#endif
	btfss tmp2, 7	; check next bit
	goto send10
	goto send01

send10	; send a rising edge
#ifdef debugging
	bsf debugoutput
#endif
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	goto bitdone

send01	; send a falling edge
#ifdef debugging
	bcf debugoutput
#endif

	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	goto bitdone

bitdone:
	rlf tmp2, f
	decfsz bitcnt, f
	goto sam_nextbit

	return

; sendbyte - this will send the byte in bytetosend out over the
; software UART defined by RF_TX.. It will also invert it,to be
; compliant with RS232.
sendbyte:
	global sendbyte
	movlw 0x08
	movwf bitcnt

	; Start bit
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause

nextbit2:
	; set output to appropriate, inverting at the same time
	btfss bytetosend, 0
	bcf RF_TX_PORT, RF_TX_PIN
	btfsc bytetosend, 0
	bsf RF_TX_PORT, RF_TX_PIN

	; wait for a bit period
	call bitpause
	
	; ready next bit
	rrf bytetosend, F

	; do next bit if appropriate
	decfsz bitcnt, f
	goto nextbit2

	; stop bit
	; Start bit
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause

	return

; bitpause - this procedure will pause (block) for one bit period
; according to the settings BITPAUSEHI/LO in main.h.
bitpause:
	global bitpause
	; hang about until it's bit change time.
	movlw BITPAUSEHI
	movwf TMR1H
	movlw BITPAUSELO
	movwf TMR1L

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
	
waitfortimer:
	btfss PIR1, TMR1IF
	goto waitfortimer

	return

halfbitpause:
	movlw BITPAUSEHI
	movwf TMR1H
	movlw BITPAUSELO
	movwf TMR1L

	bsf STATUS, C
	rrf TMR1H, f
	rrf TMR1L, f
	
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going

waitfortimer2:
	btfss PIR1, TMR1IF
	goto waitfortimer2

	return

	end