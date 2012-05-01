	extern dotests

	; Set these defines to present a hardcoded packet on boot. This
	; is only really useful for testing the uC core.
	; This will also set a crypto handshake to 'done', so
	; the raw packet here will be decrypted (if neccessary) and 
	; then processed as normal.

	;#define INSERT_TEST_PACKET
	;#define TEST_PACKET_BYTE_0 0x01
	;#define TEST_PACKET_BYTE_1 0x01
	;#define TEST_PACKET_BYTE_2 0x01
	;#define TEST_PACKET_BYTE_3 0x01
	;#define TEST_PACKET_BYTE_4 0x01
	;#define TEST_PACKET_BYTE_5 CMD_SETSENSOR
	;#define TEST_PACKET_BYTE_6 0x20
	;#define TEST_PACKET_BYTE_7 0x01
