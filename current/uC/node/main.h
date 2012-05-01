
	#include <p16f628.inc>

key1 EQU	H'20' ; key bytes
key2 EQU	H'21'
key3 EQU	H'22'
key4 EQU	H'23'
key5 EQU	H'24'
key6 EQU	H'25'
key7 EQU	H'26'
key8 EQU	H'27'
key9 EQU	H'28'
key10 EQU	H'29'
key11 EQU	H'2a'
key12 EQU	H'2b'
key13 EQU	H'2c'
key14 EQU	H'2d'
key15 EQU	H'2e'
key16 EQU	H'2f'

halfroundcount EQU H'38' ; stores how many half-rounds are left to do

packet0		EQU H'39'
packet1		EQU H'3a'
packet2		EQU H'3b'
packet3		EQU H'3c'
packet4		EQU H'3d'
packet5		EQU H'3e'
packet6		EQU H'3f'
packet7		EQU H'40'

tmp1 EQU	H'41' ; temp vars - 3x4x8 bits
tmp2 EQU	H'42'
tmp3 EQU	H'43'
tmp4 EQU	H'44'

tmp5 EQU	H'45'
tmp6 EQU	H'46'
tmp7 EQU	H'47'
tmp8 EQU	H'48'

tmp9 EQU	H'49'
tmp10 EQU	H'4a'
tmp11 EQU	H'4b'
tmp12 EQU	H'4c'

arg1 EQU	H'4d' ; used for passing to maths
arg2 EQU	H'4e' ; routines
arg3 EQU	H'4f'
arg4 EQU	H'50'
arg5 EQU	H'51'
arg6 EQU	H'52'
arg7 EQU	H'53'
arg8 EQU	H'54'

sum1 EQU	H'55' ; the 'sum' used by XTEA - it gets 
sum2 EQU	H'56' ; incremented by DELTA every cycle
sum3 EQU	H'57'
sum4 EQU	H'58'

cryptdir	EQU	H'59'
nodeid		EQU	H'5b'

; How many consecutive 0xAA characters - sync bytes - have we seen?
syncbytes 	EQU H'5c'

p1	 EQU	H'5d'	; pretty important - these are used by 
p2	 EQU	H'5e'	; the authentication process. They are 
p3	 EQU	H'5f'	; 'p' in the accompanying readme.

state EQU	H'60'

dbg1	EQU	H'61'
dbg2	EQU	H'62'
dbg3	EQU	H'63'

bitcnt	  EQU H'65'
bytecnt	  EQU H'66'
bytetosend EQU H'67'

; These two are used by the interrupt routine.
W_TEMP EQU H'68'
STATUS_TEMP EQU H'69'

mux_1_1	EQU H'70'
mux_1_2	EQU H'71'
mux_1_state	EQU H'72'
mux_1_delay	EQU H'73'

#define RF_TX_PORT  PORTA
#define RF_RX_PORT  PORTA

#define RF_TX_TRIS  PORTA
#define RF_RX_TRIS  PORTA

#define RF_TX_PIN  1
#define RF_RX_PIN  0

#define LEADIN_CYCLES 0x30

; This defines how many cycles of 'lead in' (ie, manchester-encoded
; 0x00, a square wave) should be sent before each packet. They are 
; needed to give time for the transciever to 'lock in' to the signal.

; these should be set to 
;
; 0xFFFF - ( (clockrate)Hz/(rf speed)Baud / 2 )
;
; eg, 4mhz, 300baud
;   0xffff - 4000000/300/2 = 
;
#define BITPAUSEHI H'd8'
#define BITPAUSELO H'ef'
#define HALFBITPAUSEHI H'EC'
#define HALFBITPAUSELO H'77'


; Positions of various things in EEPROM
#define EEPROM_KEY		0x00
#define EEPROM_NODEID 	0x10
#define EEPROM_START_P	0x11
#define EEPROM_NAME		0x15

#define STATUS_BIT_CRYPTOSTATE  0
#define STATUS_BIT_NEEDRELOAD   1

; Speed of idletimer, if used. 
#define IDLETIMER_SPEED 0xA0 ; 40usec, good for LEDs
;#define IDLETIMER_SPEED 0xF0
;#define IDLETIMER_SPEED 0x10	; realllly slow

; These debug #defines are intended to be used in conjunction with
; the associated stimulus files. Enable one and attatch said stim.
; They are implemented in main.asm, where there are comments to
; explain how to use them.

; #define TESTBENCH_TEST_AWAITIDLESYMBOL
; #define TESTBENCH_TEST_AWAITSTARTSYMBOL