
	#include "main.h"
	#include "../shared/init.h"
	#include "../shared/swuart.h"
	#include "../shared/swuart-lowlevel.h"

	errorlevel  -302  

	; Main code.

	__CONFIG _CP_OFF & _WDT_OFF & _HS_OSC & _LVP_OFF & _BODEN_OFF
STARTUP CODE

	goto start
	nop
	nop
	nop
	nop

page0 CODE

	; This code will listen to the hardware UART, relaying info
	; via a software UART to the RF chipset.
start 
	global start

	movlw 0x01
	clrf packetpos
	clrf syncbytesseen

	call init
	; We now enter an idle loop, sending hi-to-low transitions
	; to keep the nodes locked in to us.
	; we check to see if the PC has sent us anything, and if it
	; has, we send it to the PC

	; todo: update to use interrupts more?

idle
	btfsc RCSTA, OERR
	call holycrapanoverflow

	bsf RF_TX_PORT, RF_TX_PIN
	call bitpause
	bcf RF_TX_PORT, RF_TX_PIN
	call bitpause
	
	call checkfrompc
	call sendpendingpackets

	goto idle

; ---

holycrapanoverflow:
; Holy crap, an overflow! best clear it and hope for the best.
	bcf RCSTA, CREN
	bsf RCSTA, CREN

	return 

checkfrompc
	; see if there is anything in the UART normally linked
	; to the PC.
	btfsc PIR1, RCIF
	call savebytefrompc
	btfsc PIR1, RCIF
	goto checkfrompc	; data still pending

	return

; This saves a byte from the PC. Note that this also handles
; synchronisation - the PC will sync with the transmitter,
; totally independantly of the transmitter syncronising with
; the RF network.
savebytefrompc

	movfw RCREG
	movwf tmp1
	
	xorlw 0xAA
	btfss STATUS, Z
	goto notalignment
	; it is an alignment byte. inc the count of consecutive
	; alignment bytes seen,and if it is 8, reset the packet
	; buffer to zero.
	incf syncbytesseen, f
	movfw syncbytesseen
	xorlw 0x08
	btfss STATUS, Z
	goto savepacket	; No, this isn't the last of an alignment
					; packet. Save it and continue with the 
					; next byte.
	
	; yes, 8 alignemnt bytes seen.
	clrf packetpos
	clrf syncbytesseen
	return

notalignment
	; packet is not an alignment byte. clear the number of consec.
	; alignemnt bytes seen, and save it.
	clrf syncbytesseen

savepacket:
	movfw packetpos
	movwf tmp2

	movfw tmp1		; the recieved byte

	movf tmp2, f
;	decf  tmp2, f
	btfsc STATUS, Z	; is it zero?
	movwf packet0
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet1
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet2
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet3
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet4
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet5
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet6
	decf  tmp2, f
	btfsc STATUS, Z
	movwf packet7

	; inc. packet position, and if not the last, return

	movlw 0x08
	incf packetpos, f
	xorwf packetpos, w
	btfss STATUS, Z
	return

	; packet is completely recieved. reset to start of packet
	; buffer and mark that we have a packet pending.
	clrf packetpos
	incf pendingpackets, f
	return

sendpendingpackets

	movfw pendingpackets
	btfsc STATUS, Z
	return					; nothing pending, so return

	; process the packet by sending it all out of the manc. sw 
	; UART.

	; send start word
	call sendstartsymbol

	movfw packet0
	movwf bytetosend
	call sendasmanchester
	movfw packet1
	movwf bytetosend
	call sendasmanchester
	movfw packet2
	movwf bytetosend
	call sendasmanchester
	movfw packet3
	movwf bytetosend
	call sendasmanchester
	movfw packet4
	movwf bytetosend
	call sendasmanchester
	movfw packet5
	movwf bytetosend
	call sendasmanchester
	movfw packet6
	movwf bytetosend
	call sendasmanchester
	movfw packet7
	movwf bytetosend
	call sendasmanchester

	bcf RF_TX_PORT, RF_TX_PIN 
;	bcf PORTB, 0

	decf pendingpackets, f	; mark no packets remaining

;	bcf PORTB,0
;	bsf PORTB,0

	; Now that we've sent the data, we wait for the addressed node to
	; respond. We wait for a few bit transitions as a kind of noise
	; protection measure.

	bsf PORTB,0

	movlw 0x10
	movwf bytecnt
waitmoreidles
	call awaitidlesymbolwithabort

	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0

	decfsz bytecnt
	goto waitmoreidles

	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0

	btfsc PIR1, RCIF
	return				; if anything has come in from the PC, abort!

	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0
	bcf PORTB,0
	bsf PORTB,0

	call recasmanchester

;	bcf PORTB,0
;	bsf PORTB,0

	; Now we've recieved a packet, send it to the PC.
	
waittosenddata1:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata1	; wait for idleness!
	movfw packet0
	movwf TXREG
	bsf TXSTA, TXEN
waittosenddata2:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata2	; wait for idleness!
	movfw packet1
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata3:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata3	; wait for idleness!
	movfw packet2
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata4:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata4	; wait for idleness!
	movfw packet3
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata5:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata5	; wait for idleness!
	movfw packet4
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata6:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata6	; wait for idleness!
	movfw packet5
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata7:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata7	; wait for idleness!
	movfw packet6
	movwf TXREG 
	bsf TXSTA, TXEN
waittosenddata8:
	btfss PIR1, TXIF	; are we idle?
	goto waittosenddata8	; wait for idleness!
	movfw packet7
	movwf TXREG 
	bsf TXSTA, TXEN

;	bcf PORTB,0

	return


	end