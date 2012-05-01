
	#include "main.h"
	#include "sensorcfg.h"
	#include "processnamedcmd.asm"
	#include "protocol.h"
	#include "memoryplacement.h"


	errorlevel  -302  

	CODE
do_cmd_set_sensor
	GLOBAL do_cmd_set_sensor

	; This command takes one byte of sensor ID, in packet7,
	; and one byte of state data in packet6.The sensor will
	; respond with 0x00000000 on success, or report the
	; error in packet7 on failure.
	;
	; It is expected that output sensors add to this command
	; rather than create a new command.
	;
	; The following sensors respond to this command:
	;	SENSOR_GENERIC_DIGITAL_OUT - takes one bit, bit 0 of packet6
	; 	SENSOR_PWM_LED - takes one bytes, packet6, specifying the brightness to
	;					 fade to.
	; Now, work out what is selected. 

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT
	movfw packet7		; ID
	xorlw d'(AUTOGEN_EVERY_SENSOR_ID)'
	btfsc STATUS,Z
	goto cmd_set_sensor_(AUTOGEN_EVERY_SENSOR_ID)	; sensor 1 is selected
#endif
(AUTOGEN_END_REPLICATING_BLOCK)

	; don't know this sensor! respond with a failure indicator.
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_NODE_NOT_FOUND
	movwf packet7
	return

;
; If you are creating a handler for a new sensor type, add a test here in the
; form of
;  #if (SENSOR_ID_<your identifier> == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
;    (driver code for sensor (AUTOGEN_EVERY_SENSOR_ID))
;  #endif

(AUTOGEN_BEGIN_REPLICATING_BLOCK)
#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PRESENT

cmd_set_sensor_(AUTOGEN_EVERY_SENSOR_ID):

	; Clamp our output according to max/min
	movfw packet6
	sublw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MIN
	btfss STATUS, C
	goto noclampmin_(AUTOGEN_EVERY_SENSOR_ID)
	; Otherwise, clamp to min.
	movlw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MIN
	movwf packet6
noclampmin_(AUTOGEN_EVERY_SENSOR_ID):
	
	; Now check the max.
	movfw packet6
	sublw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MAX
	btfsc STATUS, C
	goto noclampmax_(AUTOGEN_EVERY_SENSOR_ID)
	movlw SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MAX
	movwf packet6

noclampmax_(AUTOGEN_EVERY_SENSOR_ID):
	
	; SENSOR_GENERIC_DIGITAL_OUT
	#if (SENSOR_ID_GENERIC_DIGITAL_OUT == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		btfss packet6, 0
		goto cmd_set_(AUTOGEN_EVERY_SENSOR_ID)_is_clear
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
		goto cmd_set_return_ok
cmd_set_(AUTOGEN_EVERY_SENSOR_ID)_is_clear:
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PIN
		goto cmd_set_return_ok
	#endif

	; SENSOR_PWM_LED
	#if (SENSOR_ID_PWM_LED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		movfw packet6
		bsf STATUS, RP0	; bank 1
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_TARGET
		bcf STATUS, RP0	; bank 0
		goto cmd_set_return_ok
	#endif

	; SENSOR_TRIAC
	#if (SENSOR_ID_TRIAC == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		;Take our byte of data and scale it from 0-255 to 0-32
		bcf STATUS, C
		rrf packet6, f
		bcf STATUS, C
		rrf packet6, w
		
		bsf STATUS, RP0	; bank 1
		movwf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_PWM_VOLUME
		bcf STATUS, RP0	; bank 0
		goto cmd_set_return_ok
	#endif ; sensor_triac

	; SENSOR_MULTI_LED
	#if (SENSOR_ID_MULTILED == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)

		#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_SINGLEBIT
			; We should illuminate a single bit at a time in the output, eg:
			; input 0 => 0001
			; input 1 => 0010
			; input 2 => 0100
			; input 3 => 1000
			; if _MULTILED_OFF_AT_ZERO is defined, then we translate an input of 0 to turn off all 
			; outputs, and 1 to light the first.
			
			#ifdef SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_OFF_AT_ZERO 
				movfw packet6
				btfss STATUS, Z
				goto notzero_(AUTOGEN_EVERY_SENSOR_ID)
				; OK, the packet is zero. Set our setpoint to zero and skip the bitshifting logic.
				clrf packet6
				goto singlebitDone_(AUTOGEN_EVERY_SENSOR_ID)
notzero_(AUTOGEN_EVERY_SENSOR_ID):
				; The packet is not zero. Sub one from it to allow for the extra 'zero' state we inserted.
				; Then go through our bitshifty logic as normal.
				decf packet6, f
			#endif

			; Store 0x01 in tmp1, and rotate it left. Dec packet six, and loop if not zero.
			movlw h'01'
			movwf tmp1
			incf packet6, f
rotateAgain_(AUTOGEN_EVERY_SENSOR_ID):
			decfsz packet6, f
			goto rotateOneMore_(AUTOGEN_EVERY_SENSOR_ID)
			goto rotationDone_(AUTOGEN_EVERY_SENSOR_ID)
rotateOneMore_(AUTOGEN_EVERY_SENSOR_ID):
			#ifdef SENSOR_2_MULTILED_SINGLEBIT_FILL
				bsf STATUS, C
			#else
				bcf STATUS, C
			#endif
			rlf tmp1, f
			goto rotateAgain_(AUTOGEN_EVERY_SENSOR_ID)
rotationDone_(AUTOGEN_EVERY_SENSOR_ID):
			; OK. Now set the output as normal.
			movfw tmp1
			movwf packet6
singlebitDone_(AUTOGEN_EVERY_SENSOR_ID):
		#endif ; _SINGLEBIT

		; Apply one byte of data to the multiple binary output pins which we have.
		; The LSB should be on pin 1.
		; If _SINGLEBIT is set, then the above routine will have changed packet6 so that it represents
		; what we really want to see.
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 8)
			#error SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT too great
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 7)
			btfss packet6, 7
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_8, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_8
			btfsc packet6, 7
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_8, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_8
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 6)
			btfss packet6, 6
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_7, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_7
			btfsc packet6, 6
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_7, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_7
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 5)
			btfss packet6, 5
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_6, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_6
			btfsc packet6, 5
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_6, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_6
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 4)
			btfss packet6, 4
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
			btfsc packet6, 4
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 3)
			btfss packet6, 3
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
			btfsc packet6, 3
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 2)
			btfss packet6, 2
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
			btfsc packet6, 2
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 1)
			btfss packet6, 1
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
			btfsc packet6, 1
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		#endif
		#if (SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_ELEMENTCOUNT > 0)
			btfss packet6, 0
			bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
			btfsc packet6, 0
			bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		#endif
		goto cmd_set_return_ok
	#endif

	; SENSOR_MULTI_LED_MULTIPLEX
	#if (SENSOR_ID_MULTILED_MULTIPLEX == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		; For this sensor type, we have the following inputs:
		;  * Five data lines (0-4), wired to each output bank in parallel
		;  * Two 'enable' lines.
		; We take our input number and decode it like this:
		;  * if the input is 4 or less, set enable line 0, clear enable line 1, and put the input on the outputs
		;  * Otherwise, clear enable line 1, and set enable line 0, then put the input minus 4 on the outputs.

		; First, are we below 4?
		movlw 0x05
		subwf packet6, w
		btfsc STATUS, C
		goto aboveFour_(AUTOGEN_EVERY_SENSOR_ID)
		
		; OK, we're four (or below). 
		; First, set those control lines.
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_2_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_2_PIN
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_1_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_1_PIN

below4_(AUTOGEN_EVERY_SENSOR_ID):
		; We need to set the n'th bit in our output byte.
		clrf tmp1
		incf tmp1, f
		
		; If the input was zero, we should just set to 0x01.
		incf packet6, f
		decfsz packet6, f
		goto shift_(AUTOGEN_EVERY_SENSOR_ID); Input was not zero. do some shifts.
		goto set_(AUTOGEN_EVERY_SENSOR_ID)	; OK, the input was zero. Go set it.

shift_(AUTOGEN_EVERY_SENSOR_ID):
		bcf STATUS, C		; Rotate is through C, so make sure we shift in zeros
		rlf tmp1, f			; Shift!

		decfsz packet6, f
		goto shift_(AUTOGEN_EVERY_SENSOR_ID)	; Need more bitshifts
		goto set_(AUTOGEN_EVERY_SENSOR_ID)		; OK, finished.

aboveFour_(AUTOGEN_EVERY_SENSOR_ID):

		; Above four, OK. Lets set the control lines and then subtract five, then just pass to the 'below four' logic.
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_1_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_1_PIN
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_2_PORT, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_CTL_2_PIN
		
		movlw 0x05
		subwf packet6, f
		goto below4_(AUTOGEN_EVERY_SENSOR_ID)

set_(AUTOGEN_EVERY_SENSOR_ID):
		; Now set each output byte according to what we have in tmp1.
		; This is where we do the inversion on the data lines.
		btfss tmp1, 0
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		btfsc tmp1, 0
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_1, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_1
		btfss tmp1, 1
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		btfsc tmp1, 1
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_2, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_2
		btfss tmp1, 2
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		btfsc tmp1, 2
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_3, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_3
		btfss tmp1, 3
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		btfsc tmp1, 3
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_4, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_4
		btfss tmp1, 4
		bsf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		btfsc tmp1, 4
		bcf SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PORT_5, SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_MULTILED_PIN_5
		
		goto cmd_set_return_ok
	#endif

	; SENSOR_ID_COMPOSITE
	#if (SENSOR_ID_COMPOSITE == SENSOR_(AUTOGEN_EVERY_SENSOR_ID)_TYPE)
		; This sensor should take its input and sent it to a number of child sensors.
		; For example, this sensor coluld take a number between 00 and 99, and send
		; each digit seperately to a seven segment display.
		;
		; First, decode our input in to the values we will present to child sensors.
		; This is complicted slightly by the fact that our values may not be at powers
		; if two, and so we can't simply mask/shift.
		;
		; We do this really naively. We simply subtract our radix until it is less than
		; zero; the amount of times we subtracted is the Most Significant Digit, and the
		; remainder (+ raidx!) is the Least.
		clrf tmp3	;	Store MSD here
		clrf tmp2	;	Store LSD here.
subtractAgain_(AUTOGEN_EVERY_SENSOR_ID):
		movlw SENSOR_1_RADIX
		subwf packet6, w
		btfss STATUS, C
		goto doneSubtracting_(AUTOGEN_EVERY_SENSOR_ID)
		movwf packet6		; Propogate our subtracted value to W only if we are not
							; below zero.
		incf tmp3, f
		goto subtractAgain_(AUTOGEN_EVERY_SENSOR_ID)

doneSubtracting_(AUTOGEN_EVERY_SENSOR_ID):
		movfw packet6
		movwf tmp2
		; OK, cool. Now we have our two 'slave' values in tmp3 and tmp2. We should now
		; propogate them to the sensors themselves, which is easiest done by sending
		; them a set_sensor command.
		; TODO: Is there a way to get the proprocessor to do more here? It'd be good to
		; call the relevant set_sensor func (eg cmd_set_sensor_2) and thus ensure safety
		; at build time.
		movwf packet6				; Send tmp2 as the new value
		movlw SENSOR_1_SENSOR_ID_1	; to the sensor ID we define at build time
		movwf packet7
		call do_cmd_set_sensor;
		movfw tmp3					; Then send tmp3 to the other sensor ID.
		movwf packet6
		movlw SENSOR_1_SENSOR_ID_2
		movwf packet7
		call do_cmd_set_sensor

		goto cmd_set_return_ok
	#endif

	; Add your own handlers here, in an #if..#endif block. Fallthrough to this goto.
	goto cmd_set_return_wrong_type

#endif ; sensor present
(AUTOGEN_END_REPLICATING_BLOCK)

cmd_set_return_wrong_type:

	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SET_SENSOR_NODE_WRONG_TYPE
	movwf packet7
	return

cmd_set_return_ok:
	clrf packet4
	clrf packet5
	clrf packet6
	movlw ERR_SENSOR_NONE
	movwf packet7
	return

	end 