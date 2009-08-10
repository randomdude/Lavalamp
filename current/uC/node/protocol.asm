

	radix	hex

	#include "main.h"
	#include "tea.h"
	#include "../shared/init.h"
	#include "../shared/swuart.h"
	#include "../shared/swuart-lowlevel.h"
	#include "idletimer.h"

	errorlevel  -302  

	CODE

sendpacket:
	GLOBAL sendpacket
	; Sent a packet of data out via the software manchester
	; UART.

	; DEBUG - send to debug channel first
	; call sendpackethwuart
	; end debug code

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

sendpackethwuart:
	GLOBAL sendpackethwuart

; This just sends a packet out through the hardware UART.
; Mainly used for debugging.

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
	
	return

sendbytehwuart:
waituntilidle1:
	btfss PIR1, TXIF	; are we idle?
	goto waituntilidle1	; wait for idleness!
	movwf TXREG
	bsf TXSTA, TXEN
waituntilidle2:			; we wait for our packet to finish sending before returning
	btfss PIR1, TXIF	; are we idle?
	goto waituntilidle2	; wait for idleness!

	return

	end
