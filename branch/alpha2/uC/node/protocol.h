	EXTERN sendpacket
	EXTERN sendpackethwuart

CMD_PING	EQU H'01'
CMD_IDENT	EQU H'02'
CMD_GETSENSOR	EQU H'03'
CMD_SETSENSOR	EQU H'04'
CMD_GETSENSORTYPE	EQU H'05'

; Note that errors returned from the PIC are 
; command-dependant and thus need parsing at
; the PC - side depending which command they 
; are returned from.

ERR_SET_SENSOR_NONE				EQU H'00'
ERR_SET_SENSOR_NODE_NOT_FOUND	EQU H'01'
ERR_SET_SENSOR_NODE_WRONG_TYPE	EQU H'02'

ERR_GET_SENSOR_NODE_NOT_FOUND	EQU H'01'
ERR_GET_SENSOR_NODE_WRONG_TYPE	EQU H'02'

ERR_GET_SENSOR_TYPE_NODE_NOT_FND	EQU H'01'

#define SENSOR_ID_GENERIC_DIGITAL_IN	1
#define SENSOR_ID_GENERIC_DIGITAL_OUT	2

