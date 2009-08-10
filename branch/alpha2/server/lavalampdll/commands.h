
#ifndef COMMANDS_H
#define COMMANDS_H

#include "protocol.h"
#include "../shared/lavalamp.h"

#define CMD_PING  0x01
#define CMD_IDENT 0x02
#define CMD_GET_SENSOR 0x03
#define CMD_SET_SENSOR 0x04

// these are returned by SET_SENSOR_GENERIC_DIGITAL
#define ERR_SET_SENSOR_NONE				0x00
#define ERR_SET_SENSOR_NODE_NOT_FOUND	0x01
#define ERR_SET_SENSOR_NODE_WRONG_TYPE	0x02

#define ERR_GET_SENSOR_NODE_NOT_FOUND	0x01
#define ERR_GET_SENSOR_NODE_WRONG_TYPE	0x02

#endif