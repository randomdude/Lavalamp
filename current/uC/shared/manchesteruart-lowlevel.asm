
	#include "main.h"

	errorlevel  -302 

	code

a_waitidlesymbol:
	global a_waitidlesymbol
	; This function waits for a 010 sequence, where '1' is at least as
	; long as one manchester bit period.
	; with the default 1Khz carrier signal (delay timer value 0xD8EF),
	; recognised pulses are at 10014 cycles, with a tolerance of around
	; 255 cycles. In real time, thats a pulse of 200 millisec, with a
	; tolerance of 5.1 milliseconds.
	; A test script is included - define TESTBENCH_TEST_AWAITIDLESYMBOL
	; and attatch test-awaitidlesymbol.scl as stimulus for some tests.

waitstart3:
	btfss RF_RX_PORT, RF_RX_PIN
	goto waitstart3
	; time the wait for LOW
	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 

	movlw BITPAUSEHI
	movwf TMR1H
	decf TMR1H,f			; allow some slack
	movlw BITPAUSELO
	movwf TMR1L

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going

timebit2:

	; If too much time has elapsed, this pulse is too wide to be a start bit.
	; becuse we increased the time we wait above (decf tmr1), this is a definite
	; no-no and results in the sequence being discarded.
	btfsc PIR1, TMR1IF
	goto notvalid

	; if the input has gone HIGH, exit the loop.
	btfsc RF_RX_PORT, RF_RX_PIN
	goto timebit2

	bcf T1CON, TMR1ON		; stop timer

	; If we got here, the pulse wasn't too long, but it may be too short. We 
	; want to allow some tolerance, though, so check if (TMR1H=FF || TMR1H=FE || TMR1 is
	; overflowed). 
	btfsc PIR1, TMR1IF
	goto pulsenottoolong

	incf TMR1H, f
	btfsc STATUS, Z
	goto pulsenottoolong

	incf TMR1H,f 
	btfsc STATUS, Z
	goto pulsenottoolong
	
	goto waitstart3	; pulse was too short. Loop up

pulsenottoolong:
	; OK. That should be a start pulse, then.
	return

notvalid:
	; pulse is taking too long. Wait for it to finish then loop.
waitstart4:
	btfsc RF_RX_PORT, RF_RX_PIN
	goto waitstart4
	goto waitstart3

; ---------------------------------------------------------------------------

#ifdef IS_TRANSMITTER
; These functions are used only by the transmitter, and not the node.
awaitidlesymbolwithabort:
	global awaitidlesymbolwithabort
	; This is taken from 'awaitidlesymbol' but modified to return if a byte is
	; recieved at the hardware serial link. The caller is assumed to trap and
	; handle this situation.
	; Wait for our start sequence of 0110.
waitstart2:
	btfsc PIR1, RCIF
	return

	btfss RF_RX_PORT, RF_RX_PIN
	goto waitstart2

	; time the wait for LOW. If it takes less than one bit period, it's definitely not a start seq.
	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw BITPAUSEHI
	movwf TMR1H
	incf TMR1H, f			; slack
	movlw BITPAUSELO
	movwf TMR1L
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit6:
	btfsc PIR1, RCIF		; if data comes in the HW UART, abort
	return
	btfss RF_RX_PORT, RF_RX_PIN	; If RX_PORT goes low for any period of time, then this can't be a good
	goto awaitstartsymbol		; start sequence. bail.
	btfss PIR1, TMR1IF
	goto timebit6

	; OK, the wire has been HIGH for one bit period. See if it lasts for another.

	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw BITPAUSEHI
	movwf TMR1H
	incf TMR1H, f			; slack
	movlw BITPAUSELO
	movwf TMR1L
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit7:
	btfsc PIR1, RCIF		; if data comes in the HW UART, abort
	return

	btfss RF_RX_PORT, RF_RX_PIN		; If RX_PORT goes low for any period of time, then this can't be a good
	goto awaitidlesymbolwithabort ; start sequence. bail.
	btfss PIR1, TMR1IF
	goto timebit7

	; Awesome, the wire has been HIGH for two bit periods (or actually just under, because we add 0x200
	; instr. cycles of tolerance). It should go low within 0x300ic - that's the 0x200 tolerated above plus
	; 100 tolerance for this byte.

	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw 0xFD
	movwf TMR1H
	clrf TMR1L

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit8:
	btfsc PIR1, RCIF		; if data comes in the HW UART, abort
	return

	btfss RF_RX_PORT, RF_RX_PIN	; If RX_PORT goes low for any period of time before the timer overflows, then this
	goto okgotstartsym2 					; is a good start sequence. Return.
	btfss PIR1, TMR1IF
	goto timebit8
	
	; If we got here, we timed out waiting for the line to go low.
	goto awaitidlesymbolwithabort

okgotstartsym2:
#ifndef IS_TRANSMITTER
;	call disableidetimer
#endif
	return
#else 
awaitidlesymbolwithabort:
	global awaitidlesymbolwithabort
	return
#endif

; ---------------------------------------------------------------------------

awaitstartsymbol:
	global awaitstartsymbol
	; Wait for our start sequence of 0110.

waitstart1:
	btfss RF_RX_PORT, RF_RX_PIN
	goto waitstart1

	; time the wait for LOW. If it takes less than one bit period, it's definitely not a start seq.
	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw BITPAUSEHI
	movwf TMR1H
	incf TMR1H, f			; slack
	movlw BITPAUSELO
	movwf TMR1L
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit4:
	btfss RF_RX_PORT, RF_RX_PIN	; If RX_PORT goes low for any period of time, then this can't be a good
	goto awaitstartsymbol		; start sequence. bail.
	btfss PIR1, TMR1IF
	goto timebit4

	; OK, the wire has been HIGH for one bit period. See if it lasts for another.

	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw BITPAUSEHI
	movwf TMR1H
	incf TMR1H, f			; slack
	movlw BITPAUSELO
	movwf TMR1L
	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit5:
	btfss RF_RX_PORT, RF_RX_PIN	; If RX_PORT goes low for any period of time, then this can't be a good
	goto awaitstartsymbol		; start sequence. bail.
	btfss PIR1, TMR1IF
	goto timebit5

	; Awesome, the wire has been HIGH for two bit periods (or actually just under, because we add 0x200
	; instr. cycles of tolerance). It should go low within 0x300ic - that's the 0x200 tolerated above plus
	; 100 tolerance for this byte.

	bcf T1CON, T1OSCEN		; stop timer 
	bcf T1CON, TMR1ON		; stop timer 
	movlw 0xFD
	movwf TMR1H
	clrf TMR1L

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going
timebit9:
	btfss RF_RX_PORT, RF_RX_PIN	; If RX_PORT goes low for any period of time before the timer overflows, then this
	goto okgotstartsym 					; is a good start sequence. Return.
	btfss PIR1, TMR1IF
	goto timebit9
	
	; If we got here, we timed out waiting for the line to go low.
	goto awaitstartsymbol

okgotstartsym:
#ifndef IS_TRANSMITTER
;	call disableidetimer
#endif
	return


; ---------------------------------------------------------------------------

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

; ---------------------------------------------------------------------------

; bitpause - this procedure will pause (block) for one bit period
; according to the settings BITPAUSEHI/LO in main.h.
bitpause:
	global bitpause

	call halfbitpause
	call halfbitpause

	return

; ---------------------------------------------------------------------------

halfbitpause:
	global halfbitpause

	bcf T1CON, T1OSCEN
	bcf T1CON, TMR1ON

	movlw HALFBITPAUSEHI
	movwf TMR1H
	movlw HALFBITPAUSELO
	movwf TMR1L

	bcf PIR1, TMR1IF		; clear overflow flag
	bsf T1CON, T1OSCEN		; set timer going
	bsf T1CON, TMR1ON		; set timer going

waitfortimer2:
	btfss PIR1, TMR1IF
	goto waitfortimer2

	return

	end