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
#define SENSOR_1_PORT PORTB
#define SENSOR_1_TRIS TRISB
#define SENSOR_1_PIN 3
#define SENSOR_1_TYPE SENSOR_ID_PWM_LED
#define SENSOR_1_PWM_DEFAULT 10

; config for sensor 2
;#define SENSOR_2_PRESENT
;#define SENSOR_2_PORT PORTA
;#define SENSOR_2_TRIS TRISA
;#define SENSOR_2_PIN 1
;#define SENSOR_2_TYPE SENSOR_ID_PWM_LED
;#define SENSOR_2_PWM_PRESCALER 0xC4

