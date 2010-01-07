	#include "main.h"
	#include "maths.h"
	#include "sensorcfg.h"

	errorlevel  -302  

; Handles encryption and decryption using XTEA. Uses a number
; of tmp* variables, so handle this with care.

	CODE
decrypt
	global decrypt

#ifndef CRYPTO_XTEA
	return;
#endif

#ifdef CRYPTO_XTEA
	movlw H'01'
	movwf cryptdir	; set our direction flag to indicate decrypt

	; calculate sum as roundcount*delta
	bcf STATUS, C
	rrf halfroundcount, W
	movwf tmp1

	clrf arg1;
	clrf arg2;
	clrf arg3;
	clrf arg4;
	
	; initial sum
	call loaddelta
	call swaphalvesaround	; initial swap around, since we're decrypting

	; repeatedly add delta
addmore:
	call addargs;
	decfsz tmp1, F
	goto addmore

	; plop it in sum1-4
	call loadarg1to4tosum1to4
	; aaand we have roundcount*delta in sum.
	goto nexthalfcycle
#endif

encrypt
	global encrypt

#ifndef CRYPTO_XTEA
	return;
#endif

#ifdef CRYPTO_XTEA
	clrf cryptdir
	clrf sum1;
	clrf sum2;
	clrf sum3;
	clrf sum4;

	; Do half a Fiestel round
nexthalfcycle:
	; shift 02 right 5 and store in tmp1-4
	call loadpacket4to7toarg1to4

decmoar:
	call shiftright
	call shiftright
	call shiftright
	call shiftright
	call shiftright

	movf arg1, W
	movwf tmp1;
	movf arg2, W
	movwf tmp2;
	movf arg3, W
	movwf tmp3;
	movf arg4, W
	movwf tmp4;

	; shift 02 left 4 and XOR with tmp1-4, storing in tmp1-4
	call loadpacket4to7toarg1to4

	call shiftleft;
	call shiftleft;
	call shiftleft;
	call shiftleft;

	movf arg1, W
	xorwf tmp1, F;
	movf arg2, W
	xorwf tmp2, F;
	movf arg3, W
	xorwf tmp3, F;
	movf arg4, W
	xorwf tmp4, F;

	; Now add 02 to tmp1-4, storing result in tmp9-12
	call loadpacket4to7toarg1to4

	movf tmp1, W
	movwf arg5
	movf tmp2, W
	movwf arg6
	movf tmp3, W
	movwf arg7
	movf tmp4, W
	movwf arg8

	call addargs

	movf arg1, W;
	movwf tmp9;
	movf arg2, W;
	movwf tmp10;
	movf arg3, W;
	movwf tmp11;
	movf arg4, W;
	movwf tmp12;

	; Now select the right 32bit subkey into tmp5

	; Note that encryption and decryption use 'opposite' forks in
	; this code.
	btfsc cryptdir, 0
	goto weredecrypting
wereencrypting:

	btfsc halfroundcount, 0
	goto secondhalfofcycle
	goto firsthalfofcycle
	; if we're on the first half of a cycle, we use the bottom three
	; bits of sum1 to select the key index. If not, we use bits 
	; 3 and 4 of sum2.
weredecrypting:
	btfss halfroundcount, 0
	goto secondhalfofcycle
firsthalfofcycle:
	; if we get here, we're in the first half of the cycle.
	movfw sum4;
	andlw H'03';
	movwf tmp5;
	goto determinedkeyindex
secondhalfofcycle:
	; if we get here, we're in the second half of the encrypting cycle, or first of the decryting cycle.
	movfw sum3;
	movwf tmp5;
	bcf STATUS, C
	rrf tmp5, F
	rrf tmp5, F
	rrf tmp5, W
	andlw H'03';
	movwf tmp5;
determinedkeyindex:

	movf tmp5, F;
	btfsc STATUS, Z;
	goto selectkey1;
	decf tmp5, F;
	btfsc STATUS, Z;
	goto selectkey2;
	decf tmp5, F;
	btfsc STATUS, Z;
	goto selectkey3;

	goto selectkey4;

selectkey3:				; select key byte 3
	movfw key9;
	movwf arg1;
	movfw key10;
	movwf arg2;
	movfw key11;
	movwf arg3;
	movfw key12;
	movwf arg4;
	goto endkeysel
selectkey1:
	movfw key1;
	movwf arg1;
	movfw key2;
	movwf arg2;
	movfw key3;
	movwf arg3;
	movfw key4;
	movwf arg4;
	goto endkeysel
selectkey2:				; select key byte 2
	movfw key5;
	movwf arg1;
	movfw key6;
	movwf arg2;
	movfw key7;
	movwf arg3;
	movfw key8;
	movwf arg4;
	goto endkeysel
selectkey4:				; select key byte 4
	movfw key13;
	movwf arg1;
	movfw key14;
	movwf arg2;
	movfw key15;
	movwf arg3;
	movfw key16;
	movwf arg4;
	goto endkeysel

endkeysel;
	; So now we have the current key byte in arg1-4.
	; Add it to sum1-4 
	movf sum1, W
	movwf arg5;
	movf sum2, W
	movwf arg6;
	movf sum3, W
	movwf arg7;
	movf sum4, W
	movwf arg8;

	call addargs;

	; Sorted. Now, XOR this (arg1-arg4, corresponding to
	; ( (delta*i) + subkey)) to tmp9-tmp12,corresponding
	; to ( (o2<<4 ^ o2>>5)+o2 ).

	movf arg1, W
	xorwf tmp9, F
	movf arg2, W
	xorwf tmp10, F
	movf arg3, W
	xorwf tmp11, F
	movf arg4, W
	xorwf tmp12, F

	; Almost there. Now, add to o1, or subtract if decrypting.

	movf packet0, W
	movwf arg1
	movf packet1, W
	movwf arg2
	movf packet2, W
	movwf arg3
	movf packet3, W
	movwf arg4

	movf tmp9, W
	movwf arg5
	movf tmp10, W
	movwf arg6
	movf tmp11, W
	movwf arg7
	movf tmp12, W
	movwf arg8

	btfsc cryptdir, 0
	call subargs;
	btfss cryptdir, 0
	call addargs;
;	goto decryptingsosubtract
;encryptingsoadd:
;	call addargs;
;	goto anyway
;decryptingsosubtract:
;	call subargs;
;	goto anyway
;anyway:

	movf arg1, W
	movwf packet0
	movf arg2, W
	movwf packet1
	movf arg3, W
	movwf packet2
	movf arg4, W
	movwf packet3

	; Awesome! Thats one half-round done.
	; Add to the delta only after each odd round
	btfsc halfroundcount, 0
	goto dontadddelta

	movfw sum1;
	movwf arg1;
	movfw sum2;
	movwf arg2;
	movfw sum3;
	movwf arg3;
	movfw sum4;
	movwf arg4;

	call loaddelta

	btfsc cryptdir, 0
	call subargs;
	btfss cryptdir, 0
	call addargs;

;	goto decryptingsosubtractdelta
;encryptingsoadddelta:
;	call addargs;
;	goto anywaydelta
;decryptingsosubtractdelta:
;	call subargs;
;	goto anywaydelta
;anywaydelta:

	call loadarg1to4tosum1to4
dontadddelta:

	; Now, swap 02 and 01
	call swaphalvesaround

	; do next (half)-cycle?
	decfsz halfroundcount, F;
	goto nexthalfcycle


	; encrypt/decrypt operation is finished.
	; if we're decrypting:
	;  swap the first and second words.
	;  reverse bytes in word 2.
	; if encrypting
	;  reverse bytes in words 1 and 2.
	btfss cryptdir, 0
	goto finishupEncrypting

	call swaphalvesaround

	return

finishupEncrypting:
	return;

swaphalvesaround:
	; swap first 4 bytes with last 4 bytes in packet
	movfw packet4
	movwf tmp5
	movfw packet0
	movwf packet4
	movfw tmp5
	movwf packet0

	movfw packet5
	movwf tmp5
	movfw packet1
	movwf packet5
	movfw tmp5
	movwf packet1

	movfw packet6
	movwf tmp5
	movfw packet2
	movwf packet6
	movfw tmp5
	movwf packet2

	movfw packet7
	movwf tmp5
	movfw packet3
	movwf packet7
	movfw tmp5
	movwf packet3

	return

loaddelta:
	; Load the delta value in to arg5-8
	movlw h'9E'
	movwf arg5;
	movlw h'37'
	movwf arg6;
	movlw h'79'
	movwf arg7;
	movlw h'B9'
	movwf arg8;
	return

loadarg1to4tosum1to4:
	movfw arg1
	movwf sum1
	movfw arg2
	movwf sum2
	movfw arg3
	movwf sum3
	movfw arg4
	movwf sum4
	return

loadpacket4to7toarg1to4:
	movf packet4, W;
	movwf arg1;
	movf packet5, W;
	movwf arg2;
	movf packet6, W;
	movwf arg3;
	movf packet7, W;
	movwf arg4;

	return
#endif
	end
