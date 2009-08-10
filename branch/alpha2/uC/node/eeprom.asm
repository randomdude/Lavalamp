
	#include "sensorcfg.h"
	#include "main.h"

	#define EEPROM_KEY		0x00
	#define EEPROM_NODEID 	0x10
	#define EEPROM_START_P	0x11
	#define EEPROM_NAME		0x15

	errorlevel  -302  

PROG CODE

	
; EEPROM reading routine - read a single byte
readbyte:
	bsf STATUS, RP0 ; bank 1
	incf EEADR, F;
	bsf EECON1, RD
	movf EEDATA, W
	bcf STATUS, RP0 ; back to bank 0
	return

; Load all the EEPROM-stored values at powerup
init_from_eeprom:
	global init_from_eeprom
	; Pull the nodeid and shared secret key out of EEPROM (0-0x11)
    bsf STATUS, RP0 ; bank 1
    bcf STATUS, RP1 ; bank 1
	clrf EEADR
	decf EEADR, F
    bcf STATUS, RP0 ; bank 0
    bcf STATUS, RP1 ; bank 0

	call readbyte
	movwf key1;
	call readbyte;
	movwf key2;
	call readbyte;
	movwf key3;
	call readbyte;
	movwf key4;
	call readbyte;
	movwf key5;
	call readbyte;
	movwf key6;
	call readbyte;
	movwf key7;
	call readbyte;
	movwf key8;
	call readbyte;
	movwf key9;
	call readbyte;
	movwf key10;
	call readbyte;
	movwf key11;
	call readbyte;
	movwf key12;
	call readbyte;
	movwf key13;
	call readbyte;
	movwf key14;
	call readbyte;
	movwf key15;
	call readbyte;
	movwf key16;

	call readbyte
	movwf nodeid;

	call readbyte
	movwf p1;
	call readbyte
	movwf p2;
	call readbyte
	movwf p3;

	return
savep:
	global savep

	bsf STATUS, RP0 	; bank 1
	movlw EEPROM_START_P		
	movwf EEADR
	decf EEADR, F		;   one before start of P bytes in flash
	bcf STATUS, RP0 	; bank0

	movfw p1
	call writeabyte
	movfw p2
	call writeabyte
	movfw p3
	call writeabyte

	bcf STATUS, RP0  ; page 0
	bcf STATUS, RP1	 ; page 0

	return

; Writes the byte in w to ++EEADR and waits until it's finished.
writeabyte:

	bsf STATUS, RP0	; page 1

	bcf EECON1, EEIF
	movwf EEDATA

	incf EEADR, F
	movfw EEADR

	bsf EECON1, WREN
	bcf INTCON, GIE
	movlw h'55'
	movwf EECON2
	movlw h'AA'
	movwf EECON2
	bsf EECON1, WR

waitforpwrite:
	btfsc EECON1, WR
	goto waitforpwrite

	bcf STATUS, RP0	; page 0

	return

	end
