
	#include "p16f628.inc"


tmp1 EQU	H'20'
tmp2 EQU	H'21'

bitcnt		EQU	H'22'
bytetosend	EQU H'23'
bytecnt		EQU	H'24'

packet0		EQU H'25'
packet1		EQU H'26'
packet2		EQU H'27'
packet3		EQU H'28'
packet4		EQU H'29'
packet5		EQU H'2A'
packet6		EQU H'2B'
packet7		EQU H'2C'
packetpos	EQU H'2D'

syncbytesseen  EQU H'2E'
pendingpackets EQU H'2F'

tmp6 EQU	H'30' ; temp vars
tmp7 EQU	H'31'
tmp8 EQU	H'31'

#define RF_TX_PORT  PORTA
#define RF_RX_PORT  PORTA

#define RF_TX_TRIS  PORTA
#define RF_RX_TRIS  PORTA

#define RF_TX_PIN  1
#define RF_RX_PIN  0

; these should be set to 
;
; 0xFFFF - ( (clockrate)Hz/(rf speed)Baud / 2 )
;
#define BITPAUSEHI H'd8'
#define BITPAUSELO H'ef'
#define HALFBITPAUSEHI H'EC'
#define HALFBITPAUSELO H'77'

