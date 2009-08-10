DEEPROM CODE

	#include "sensorcfg.h"

	DATA KEY_0	; secret key, byte 1
	DATA KEY_1
	DATA KEY_2
	DATA KEY_3
	DATA KEY_4
	DATA KEY_5
	DATA KEY_6
	DATA KEY_7
	DATA KEY_8
	DATA KEY_9
	DATA KEY_A
	DATA KEY_B
	DATA KEY_C
	DATA KEY_D
	DATA KEY_E
	DATA KEY_F	; secret key, byte 0x10

	DATA NODEID	; NODE ID BYTE
	
	DATA 0xFF	;
	DATA 0x00	; initial P
	DATA 0x00	;

	DATA 'T'	; Start of node friendly ID
	DATA 'e'	
	DATA 's'	
	DATA 't'	
	DATA ' '	
	DATA 'n'	
	DATA 'o'	
	DATA 'd'	
	DATA 'e'	
	DATA ' '	
	DATA 'v'	
	DATA '1'	
	DATA 0x00	; End of node friendly ID string
	
	end
