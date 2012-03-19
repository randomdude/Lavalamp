; Select a communication scheme to use.
;
; COMMLINK_SWMANCHESTER involves using cheap, bare, RF modules
; and doing Manchester encoding, etc, in software. It is buggy
; and it is broken. Do not use it.
;#define COMMLINK_SWMANCHESTER
;
; COMMLINK_HWUART uses a simple serial link to a host PC.
#define COMMLINK_HWUART
;
; If you are using the HWUART, set the baud rate here. Some baud 
; Select the desired baud rate and the oscillator speed on your
; board, uncomment that line, and set 'COMMLINK_HWUART_BRGH' if
; necessary.
; 2400 @ 20mhz if you clear BRGH
;#define COMMLINK_HWUART_DELAY 0x81
; 9600 @  4mhz if you set BRGH
#define COMMLINK_HWUART_DELAY 0x19
; 9600 @ 20mhz if you clear BRGH
;#define COMMLINK_HWUART_DELAY 0x81
; 250000 @  4mhz if you set BRGH
;#define COMMLINK_HWUART_DELAY 0x00
;
; Set this if instructed to by the COMMLINK_HWUART_DELAY define 
; you selected above.
#define COMMLINK_HWUART_BRGH

; Use 32-round XTEA encryption?
; #define CRYPTO_XTEA

; Select any 'debugging modes' you wish to build here.
;
; define this to transmit data continuously, in order to test
; the communications link.
; #define TEST_TRANSMISSION
;
; Define these to set pins to singal various events which are
; useful. These will not impair the rest of the code.
;
;#define DEBUG_PULSE_ON_SYNC
;#define DEBUG_PULSE_ON_SYNC_PORT PORTB
;#define DEBUG_PULSE_ON_SYNC_TRIS TRISB
;#define DEBUG_PULSE_ON_SYNC_PIN 0

#define idletimerenabled

#define KEY_0 0x00
#define KEY_1 0x11
#define KEY_2 0x22
#define KEY_3 0x33
#define KEY_4 0x44
#define KEY_5 0x55
#define KEY_6 0x66
#define KEY_7 0x77
#define KEY_8 0x88
#define KEY_9 0x99
#define KEY_A 0xaa
#define KEY_B 0xbb
#define KEY_C 0xcc
#define KEY_D 0xdd
#define KEY_E 0xee
#define KEY_F 0xff

#define NODEID 0x01

#define SENSOR_COUNT 1

; config for sensor 1
;
#define SENSOR_1_PRESENT
#define SENSOR_1_TYPE SENSOR_ID_MULTILED_MULTIPLEX

#define SENSOR_1_MULTILED_PORT_1 PORTA
#define SENSOR_1_MULTILED_TRIS_1 TRISA
#define SENSOR_1_MULTILED_PIN_1 0
#define SENSOR_1_MULTILED_PORT_2 PORTA
#define SENSOR_1_MULTILED_TRIS_2 TRISA
#define SENSOR_1_MULTILED_PIN_2 1
#define SENSOR_1_MULTILED_PORT_3 PORTA
#define SENSOR_1_MULTILED_TRIS_3 TRISA
#define SENSOR_1_MULTILED_PIN_3 2
#define SENSOR_1_MULTILED_PORT_4 PORTA
#define SENSOR_1_MULTILED_TRIS_4 TRISA
#define SENSOR_1_MULTILED_PIN_4 3
#define SENSOR_1_MULTILED_PORT_5 PORTA
#define SENSOR_1_MULTILED_TRIS_5 TRISA
#define SENSOR_1_MULTILED_PIN_5 4

#define SENSOR_1_MULTILED_CONTROL_1_PORT PORTB
#define SENSOR_1_MULTILED_CONTROL_1_TRIS TRISB
#define SENSOR_1_MULTILED_CONTROL_1_PIN 5
#define SENSOR_1_MULTILED_CONTROL_2_PORT PORTB
#define SENSOR_1_MULTILED_CONTROL_2_TRIS TRISB
#define SENSOR_1_MULTILED_CONTROL_2_PIN 4

#define SENSOR_1_MIN 0
#define SENSOR_1_DEFAULT d'0'
#define SENSOR_1_MAX d'255'


; Use the MULTILED sensor type to drive multiple on/off signals
; (eg. LEDS) from multiple pins. By default, the value set will 
; be placed on to the pins like this:
;  value 0 -> 001
;  value 1 -> 010
;  value 2 -> 011
;  value 3 -> 100
; However, you can also define _SINGLEBIT to illuminate only one
; bit at a time, like this:
;  value 0 -> 0001
;  value 1 -> 0010
;  value 2 -> 0100
;  value 3 -> 1000
; If you have set _SINGLEBIT, you can also set _SINGLEBIT_FILL to
; create the following effect:
;  value 0 -> 0001
;  value 1 -> 0011
;  value 2 -> 0111
;  value 3 -> 1111
; Any sensor time can also accomodate OFF_AT_ZERO, which will turn
; off all outputs when the value zero is recieved, thus:
;  value 0 -> 0000
;  value 1 -> 0001
;  value 2 -> 0010
;  value 3 -> 0011

; #define SENSOR_1_PRESENT
;#define SENSOR_1_TYPE SENSOR_ID_MULTILED
;#define SENSOR_1_MIN 0
;#define SENSOR_1_DEFAULT d'255'
;#define SENSOR_1_MAX d'255'
; #define SENSOR_1_MULTILED_OFF_AT_ZERO
; #define SENSOR_1_MULTILED_SINGLEBIT_FILL
;#define SENSOR_1_MULTILED_SINGLEBIT
;#define SENSOR_1_MULTILED_ELEMENTCOUNT 4
;#define SENSOR_1_MULTILED_PORT_1 PORTA
;#define SENSOR_1_MULTILED_TRIS_1 TRISA
;#define SENSOR_1_MULTILED_PIN_1 0
;#define SENSOR_1_MULTILED_PORT_2 PORTA
;#define SENSOR_1_MULTILED_TRIS_2 TRISA
;#define SENSOR_1_MULTILED_PIN_2 1
;#define SENSOR_1_MULTILED_PORT_3 PORTA
;#define SENSOR_1_MULTILED_TRIS_3 TRISA
;#define SENSOR_1_MULTILED_PIN_3 2
;#define SENSOR_1_MULTILED_PORT_4 PORTA
;#define SENSOR_1_MULTILED_TRIS_4 TRISA
;#define SENSOR_1_MULTILED_PIN_4 3
;#define SENSOR_1_MULTILED_PORT_5 PORTA
;#define SENSOR_1_MULTILED_TRIS_5 TRISA
;#define SENSOR_1_MULTILED_PIN_5 4
;#define SENSOR_1_MULTILED_PORT_6 PORTB
;#define SENSOR_1_MULTILED_TRIS_6 TRISB
;#define SENSOR_1_MULTILED_PIN_6 0
;#define SENSOR_1_MULTILED_PORT_7 PORTB
;#define SENSOR_1_MULTILED_TRIS_7 TRISB
;#define SENSOR_1_MULTILED_PIN_7 3
;#define SENSOR_1_MULTILED_PORT_8 PORTB
;#define SENSOR_1_MULTILED_TRIS_8 TRISB
;#define SENSOR_1_MULTILED_PIN_8 4
;#define SENSOR_1_MULTILED_PORT_9 PORTB
;#define SENSOR_1_MULTILED_TRIS_9 TRISB
;#define SENSOR_1_MULTILED_PIN_9 5