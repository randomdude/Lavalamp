; Select a communication scheme to use.
;
; COMMLINK_SWMANCHESTER involves using cheap, bare, RF modules
; and doing Manchester encoding, etc, in software. It is buggy
; and it is broken. Do not use it.
;#define COMMLINK_SWMANCHESTER
;
; COMMLINK_HWUART uses a simple serial link to a host PC.
#define COMMLINK_HWUART

; Use 32-round XTEA encryption?
; #define CRYPTO_XTEA

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

#define SENSOR_COUNT 3

; config for sensor 1
#define SENSOR_1_PRESENT
#define SENSOR_1_PORT PORTA
#define SENSOR_1_TRIS TRISA
#define SENSOR_1_PIN 2
#define SENSOR_1_TYPE SENSOR_ID_PWM_LED

; config for sensor 2
#define SENSOR_2_PRESENT
#define SENSOR_2_PORT PORTA
#define SENSOR_2_TRIS TRISA
#define SENSOR_2_PIN 3
#define SENSOR_2_TYPE SENSOR_ID_PWM_LED

; config for sensor 3
#define SENSOR_3_PRESENT
#define SENSOR_3_PORT PORTB
#define SENSOR_3_TRIS TRISB
#define SENSOR_3_PIN 3
#define SENSOR_3_TYPE SENSOR_ID_PWM_LED
