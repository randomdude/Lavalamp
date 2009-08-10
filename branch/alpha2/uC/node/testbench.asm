
	radix	hex

	#include "main.h"
	#include "tea.h"
	#include "maths.h"
	#include "../shared/init.h"

	errorlevel  -302  

; Some test values, for debugging through in the debugger mainly.

	CODE

#ifndef TESTBENCH
testbench_main:
	return
#else
testbench_main:
	global testbench_main

	call test_crypto1
	call test_crypto2

	return

test_crypto1:
	; This encrypts a block of 00's with a key 0x00112233445566778899aabbccddeeff
	; encrypted values shold be 0xaed866b64a8115f8. Block is then decrypted again
	; and compared to original. 
	movlw 0x00
	movwf key1
	movlw 0x11
	movwf key2
	movlw 0x22
	movwf key3
	movlw 0x33
	movwf key4
	movlw 0x44
	movwf key5
	movlw 0x55
	movwf key6
	movlw 0x66
	movwf key7
	movlw 0x77
	movwf key8
	movlw 0x88
	movwf key9
	movlw 0x99
	movwf key10
	movlw 0xaa
	movwf key11
	movlw 0xbb
	movwf key12
	movlw 0xcc
	movwf key13
	movlw 0xdd
	movwf key14
	movlw 0xee
	movwf key15
	movlw 0xff
	movwf key16

	clrf packet0
	clrf packet1
	clrf packet2
	clrf packet3
	clrf packet4
	clrf packet5
	clrf packet6
	clrf packet7

	movlw 0x40;		; 32 rounds
	movwf halfroundcount
; should result in encrypted words 0xaed866b6 0x4a8115f8 when used
	call encrypt
	movfw packet0
	xorlw 0xAE
	btfss STATUS, Z
	call crypt_failure
	movfw packet1
	xorlw 0xd8
	btfss STATUS, Z
	call crypt_failure
	movfw packet2
	xorlw 0x66
	btfss STATUS, Z
	call crypt_failure
	movfw packet3
	xorlw 0xb6
	btfss STATUS, Z
	call crypt_failure
	movfw packet4
	xorlw 0x4A
	btfss STATUS, Z
	call crypt_failure
	movfw packet5
	xorlw 0x81
	btfss STATUS, Z
	call crypt_failure
	movfw packet6
	xorlw 0x15
	btfss STATUS, Z
	call crypt_failure
	movfw packet7
	xorlw 0xf8
	btfss STATUS, Z
	call crypt_failure

	movlw 0x40;		; 32 rounds
	movwf halfroundcount

	; this should decrypt the above back to all 00's.
	call decrypt

	movfw packet0
	btfss STATUS, Z
	call crypt_failure
	movfw packet1
	btfss STATUS, Z
	call crypt_failure
	movfw packet2
	btfss STATUS, Z
	call crypt_failure
	movfw packet3
	btfss STATUS, Z
	call crypt_failure
	movfw packet4
	btfss STATUS, Z
	call crypt_failure
	movfw packet5
	btfss STATUS, Z
	call crypt_failure
	movfw packet6
	btfss STATUS, Z
	call crypt_failure
	movfw packet7
	btfss STATUS, Z
	call crypt_failure

	return

crypt_failure
	return

test_crypto2:
	movlw 0x00
	movwf key1
	movlw 0x11
	movwf key2
	movlw 0x22
	movwf key3
	movlw 0x33
	movwf key4
	movlw 0x44
	movwf key5
	movlw 0x55
	movwf key6
	movlw 0x66
	movwf key7
	movlw 0x77
	movwf key8
	movlw 0x88
	movwf key9
	movlw 0x99
	movwf key10
	movlw 0xaa
	movwf key11
	movlw 0xbb
	movwf key12
	movlw 0xcc
	movwf key13
	movlw 0xdd
	movwf key14
	movlw 0xee
	movwf key15
	movlw 0xff
	movwf key16

	clrf test1
	clrf test2
	clrf test3
	clrf test4
	clrf test5
	clrf test6
	clrf test7
	clrf test8

crypto2_donext:

	movfw test1
	movwf packet0
	movfw test2
	movwf packet1
	movfw test3
	movwf packet2
	movfw test4
	movwf packet3
	movfw test5
	movwf packet4
	movfw test6
	movwf packet5
	movfw test7
	movwf packet6
	movfw test8
	movwf packet7

	movlw 0x40;		; 32 rounds
	movwf halfroundcount

	call encrypt
	movlw 0x40;		; 32 rounds
	movwf halfroundcount
	call decrypt

	movfw test1
	xorwf packet0, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test2
	xorwf packet1, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test3
	xorwf packet2, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test4
	xorwf packet3, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test5
	xorwf packet4, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test6
	xorwf packet5, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test7
	xorwf packet6, W
	btfss STATUS, Z
	call crypt2_failure
	movfw test8
	xorwf packet7, W
	btfss STATUS, Z
	call crypt2_failure
	
	; now do next
	incf test1, F
	btfss STATUS, Z
	goto crypto2_donext
	incf test2, F
	btfss STATUS, Z
	goto crypto2_donext
	incf test3, F
	btfss STATUS, Z
	goto crypto2_donext
	incf test4, F
	btfss STATUS, Z
	goto crypto2_donext

	return

crypt2_failure
	return
#endif

	end
