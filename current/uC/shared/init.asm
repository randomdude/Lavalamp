
	#include "..\node\main.h"
	#include "..\shared\manchesteruart.h"
#ifndef IS_TRANSMITTER
	#include "sensorcfg.h"
	#include "idletimer.h"
#endif
	errorlevel  -302  

;   Global code to initialise the chip. Used by both the transmitter
;	and the reciever.

	CODE

#ifndef IS_TRANSMITTER
	#include "eeprom.h"
	#include "autogen_sensorcode.h"
#else
	; Define some symbols that we don't actually use.. silly 
	; linker.
autogen_sensors_init:
	global autogen_sensors_init
init_from_eeprom:
	global init_from_eeprom
#endif

init:
	global init

	call disableperiphs
	call initDebug
#ifndef IS_TRANSMITTER
	; If we're a device node, we have EEPROM config and sensors to init.
	call init_from_eeprom
	call initsensors
#endif

	call inituart
#ifdef COMMLINK_SWMANCHESTER
	call initswuart
#endif
#ifndef IS_TRANSMITTER
	call enableidetimer
#endif
	return

disableperiphs:
	; Here we disable any part-specific on-chip periphials which
	; otherwise would stop us getting proper digital IO.

#Ifdef __16F627
	bsf CMCON, CM0
	bsf CMCON, CM1	; Kill comparators
	bsf CMCON, CM2
#ENDIF
#ifdef __16F628
	bsf CMCON, CM0
	bsf CMCON, CM1	; Kill comparators
	bsf CMCON, CM2
#ENDIF
	return

#ifndef IS_TRANSMITTER
initsensors:
	; set any initialisation for generic sensors
	call autogen_sensors_init
	return
#endif

;#ifdef IS_TRANSMITTER
inituart:
	global inituart

	bsf STATUS, RP0  ; page 1

	; Set the baud rate according to the user config
	movlw COMMLINK_HWUART_DELAY
	movwf SPBRG		; set baud rate 

	bcf STATUS, RP0  ; page 0

	movlw b'10010000'	; RCSTA - set SPEN and CREN, clear the rest
	movwf RCSTA

	bsf STATUS, RP0	 ; page 1

	bsf TRISB, 2	; USART requires both to be set to 
	bsf TRISB, 1	; INPUTs.

#ifdef COMMLINK_HWUART_BRGH
	movlw b'00100100'	; TXSTA - set CSRC , and set brgh (2)
#else
	movlw b'00000100'	; TXSTA - set CSRC , and clear brgh (2)
#endif
	movwf TXSTA

	bsf TXSTA, TXEN

	bcf STATUS, RP0  ; page 0
	
	; Also, init our sync bytes var, which the hw UART uses.
	movlw 0x08
	movwf syncbytes

	return
;#endif

initDebug:
	; Set tristsates so that our debug pins can be pulsed.

#ifdef DEBUG_PULSE_ON_SYNC
	bsf STATUS, RP0	 ; page 1
	bcf DEBUG_PULSE_ON_SYNC_TRIS, DEBUG_PULSE_ON_SYNC_PIN
	bcf STATUS, RP0	 ; page 0
#endif
	return

	end
