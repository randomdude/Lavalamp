
	#include "main.h"
	#include "..\shared\manchesteruart-lowlevel.h"

	CODE

#ifndef IS_TRANSMITTER
	#include "idletimer.h"
	#include "sensorcfg.h"
	; Define some symbols that we don't actually use.. silly 
	; linker.
#endif
	errorlevel  -302 

; High-level code to operate a software Manchester-encoded Rx/Tx module.
;
; Operates on pins RF_TX_PORT/RF_TX_PIN and RF_RX_PORT/RF_RX_PIN 
; and at a bit period set by BITPAUSEHI/BITPAUSELO.
; 
; For reference, one 'bit period' is half the time it takes
; to xmit or recieve one bit of 'real' non-manchester data.
;
; Manchester-encoded values are marked by the start sequence of
; 0110, or '10' non-Manchestered.

	; these defines will throw out some debug info
	#undefine debugging 
	#undefine debugoutput ;PORTB,0
	#undefine debugtris ;TRISB,0


initswuart:
	global initswuart
	; initialise the software UART tristates
	bsf STATUS, RP0 ; trip to bank 1
	bcf RF_TX_TRIS, RF_TX_PIN
	bsf RF_RX_TRIS, RF_RX_PIN
#ifdef debugging
	bcf debugtris
#endif
	bcf STATUS, RP0 ; trip back to bank 0
	return

; ---------------------------------------------------------------------------

recasmanchester:
	; get data. We get 8 bytes.
	global recasmanchester

	; wait for idle symbol. When we recieve this, we can be reasonably
	; sure that we are listening to the node and not random noise.
	; wait until start symbol occurs
	call awaitstartsymbol

	movlw 0x08
	movwf tmp6			; collect 8 bytes

recnextbyte:
	movlw 0x08
	movwf bitcnt		; collect 8 bits per byte

#ifndef IS_TRANSMITTER
	call disableidetimer
#endif

nextbit
	; At this point, we are sync'ed to the midpoint of the
	; previous manchester codeword. Wait 1.5 codewords-til
	; in the first half of the new bit.

	call bitpause

	call halfbitpause

#ifdef debugging
	bsf debugoutput 
	bcf debugoutput 
	btfss RF_RX_PORT, RF_RX_PIN	
	bsf debugoutput 
#endif

	; sample and invert !
	bcf STATUS, C
	btfss RF_RX_PORT, RF_RX_PIN	
	bsf STATUS, C

	rlf bytetosend, f

	; To make culmative error a non-issue, wait until the
	; transition in the middle of the manchester codeword

	; sync with midpoint transition 
; todo: optimise this
	movfw RF_RX_PORT  ; get current input state
	andlw 1<<RF_RX_PIN
	btfss STATUS, Z	  ; if currently high
	goto waituntillow
	goto waituntilhigh
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
	;	call sendpackethwuart	; debug - send echo packet out over hw uart

#ifndef IS_TRANSMITTER
	call enableidetimer
#endif

	return

; ---------------------------------------------------------------------------

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
;	bsf debugoutput
;	bcf debugoutput
#endif
	btfss tmp2, 7	; check next bit
	goto send10
	goto send01

send10	; send a rising edge
#ifdef debugging
;	bsf debugoutput
#endif
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	goto bitdone

send01	; send a falling edge
#ifdef debugging
;	bcf debugoutput
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

; ---------------------------------------------------------------------------

; sendbyte - this will send the byte in bytetosend out over the
; software UART defined by RF_TX.. 
sendbyte:
	global sendbyte
	movlw 0x08
	movwf bitcnt

	; Start bit (isn't this horribly wrong?)
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause

nextbit2:
	; set output to appropriate
	btfss bytetosend, 0
	bcf RF_TX_PORT, RF_TX_PIN
	btfsc bytetosend, 0
	bsf RF_TX_PORT, RF_TX_PIN

	; wait for a bit period
	call bitpause
	
	; ready next bit
	rrf bytetosend, F

	; loop up to do next bit if appropriate
	decfsz bitcnt, f
	goto nextbit2

	; Stop bit (?) do we need this (!?)
	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause

	return

	end
