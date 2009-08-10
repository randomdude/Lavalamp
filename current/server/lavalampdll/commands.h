
#ifndef COMMANDS_H
#define COMMANDS_H

#include "protocol.h"
#include "../shared/lavalamp.h"

#define CMD_PING  0x01
#define CMD_IDENT 0x02
#define CMD_GET_SENSOR	0x03
#define CMD_SET_SENSOR	0x04
#define CMD_GET_SENSOR_TYPE 0x05
#define CMD_SET_SENSOR_FADE 0x06
#define CMD_SET_P_LOW	0x07
#define CMD_SET_P_HIGH	0x08
#define CMD_SET_NODE_ID 0x09
#define CMD_SET_KEY_BYTE		0x0A
#define CMD_RELOAD_FROM_FLASH	0x0B

// these are returned by SET_SENSOR_GENERIC_DIGITAL
#define ERR_SET_SENSOR_NONE				0x00
#define ERR_SET_SENSOR_NODE_NOT_FOUND	0x01
#define ERR_SET_SENSOR_NODE_WRONG_TYPE	0x02

#define ERR_GET_SENSOR_NODE_NOT_FOUND	0x01
#define ERR_GET_SENSOR_NODE_WRONG_TYPE	0x02

#define ERR_SET_SENSOR_FADE_NOT_FOUND	0x01
#define ERR_SET_SENSOR_FADE_WRONG_TYPE	0x02

#define ERR_GET_SENSOR_TYPE_SENSOR_NOT_FOUND	0x01

#endif