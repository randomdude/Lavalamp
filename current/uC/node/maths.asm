	radix	hex

	#include "main.h"
	#include "tea.h"
	#include "../../shared/init.h"

	errorlevel  -302  

; Does some misc. maths.
; addargs will do a 32bit add, subargs does similar.
; shiftleft/shiftright shift 1 bit.

	CODE
addargs:
	global addargs
	; 32bit add : arg1 to arg4 += arg5 to arg8
	; arg1 is MSB, arg4 is LSB
	; arg5 is MSB, arg8 is LSB
	; Add arg5 to tmp1-tmp4
	movf arg8, W
	addwf arg4, F	; arg1 = arg1+021
	btfss STATUS, C	; carry?
	goto addargsdigit2
	incf arg3, F;
	btfsc STATUS, Z	; carry from tmp2 ?
	incf arg2, F;
	btfsc STATUS, Z	; carry from tmp3 ?
	incf arg1, F;		; We don't care if the whole thing overflowed.

addargsdigit2:

	; Add arg6 to arg2-arg4
	movf arg7, W
	addwf arg3, F	; arg2 = agr2+023
	btfss STATUS, C	; carry?
	goto addargsdigit3
	incf arg2, F;
	btfsc STATUS, Z	; carry from tmp3 ?
	incf arg1, F;

addargsdigit3:

	; Add tmp7 to tmp3-tmp4
	movf arg6, W
	addwf arg2, F	; tmp3 = tmp3+023
	btfsc STATUS, C	; carry?			** should this be btfsS?
	incf arg1, F;

	; And finally add tmp8 to tmp4
	movf arg5, W
	addwf arg1, F	; tmp4 = tmp4+024

	return

subargs:
	GLOBAL subargs
	movf arg8, W
	subwf arg4, F	; arg1 = arg1+021
	btfsc STATUS, C	; carry?
	goto subargsdigit2
	decf arg3, F;   ; Checking this is a bitch.
	comf arg3, W;
	btfss STATUS, Z	; carry from tmp2 ?
	goto subargsdigit2
	decf arg2, F;
	comf arg2, W;
	btfss STATUS, Z	; carry from tmp3 ?
	goto subargsdigit2
	decf arg1, F;		; We don't care if the whole thing overflowed.
subargsdigit2:
	; Add arg6 to arg2-arg4
	movf arg7, W
	subwf arg3, F	; arg2 = agr2+023
	btfsc STATUS, C	; carry?
	goto subargsdigit3
	decf arg2, F;
	comf arg2, W;
	btfss STATUS, Z	; carry from tmp3 ?
	goto subargsdigit3
	decf arg1, F;
subargsdigit3:
	; Add tmp7 to tmp3-tmp4
	movf arg6, W
	subwf arg2, F	; tmp3 = tmp3+023
	btfss STATUS, C	; carry?
	decf arg1, F;

	; And finally add tmp8 to tmp4
	movf arg5, W
	subwf arg1, F	; tmp4 = tmp4+024

	return

shiftright:
	GLOBAL shiftright
	; Rotate tmp1 through tmp2 .. tmp13.. tmp14 .. pad with 0's.
	bcf STATUS, C;
	rrf arg1, f;
	rrf arg2, f;
	rrf arg3, f;
	rrf arg4, f;

	return;

shiftleft:
	GLOBAL shiftleft
	; Rotate tmp1 through tmp2 .. tmp13.. tmp14 .. (pad with 0's)
	bcf STATUS, C;
	rlf arg4, f;
	rlf arg3, f;
	rlf arg2, f;
	rlf arg1, f;

	return;
	

	end
