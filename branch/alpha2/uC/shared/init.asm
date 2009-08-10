
	#include "main.h"
	#include "swuart.h"

	errorlevel  -302  

;   Global code to initialise the chip. Used by both the transmitter
;	and the reciever.

	CODE

#ifndef IS_TRANSMITTER
	#include "eeprom.h"
	#include "autogen_sensorcode.h"
#else
	; Define some symbols that we don't actually use.. damn 
	; linker.
autogen_sensors_init:
	global autogen_sensors_init
init_from_eeprom:
	global init_from_eeprom
#endif

init:
	global init

	call disableperiphs
#ifndef IS_TRANSMITTER
	; If we're a device node, we have EEPROM config and sensors to init.
	call init_from_eeprom
	call initsensors
#endif

	call inituart
	call initswuart
	return



disableperiphs:
	; Here we disable any part-specific on-chip periphials which
	; otherwise would stop us getting proper digital IO.

	bsf CMCON, CM0
	bsf CMCON, CM1	; Kill comparators on my 16f627/8
	bsf CMCON, CM2

	return

#ifndef IS_TRANSMITTER
initsensors:
	; set any initialisation for generic sensors
	call autogen_sensors_init
	return
#endif

inituart:
	global inituart

	bsf STATUS, RP0  ; page 1

	movlw 0x19		; 2400 @ 4mhz if you clear BRGH
	movwf SPBRG		; set baud rate 

	bcf STATUS, RP0  ; page 0

	movlw b'10010000'	; RCSTA - set SPEN and CREN, clear the rest
	movwf RCSTA

	bsf STATUS, RP0	 ; page 1

	bsf TRISB, 2	; USART requires both to be set to 
	bsf TRISB, 1	; INPUTs.

	movlw b'10000000'	; TXSTA - set CSRC , and not brgh (2)
	movwf TXSTA

	bsf TXSTA, TXEN

	bcf STATUS, RP0  ; page 0
	
	return

	end
