// wallboard.h
#pragma once
#include <windows.h>
#include <stdio.h>
#include <string>

enum wallboardErrorState
{
	TimeOutError = 0xFF,
	IllegalCommand = 0x6,
	ChecksumError = 0x5,
	BufferOverflow = 0x4,
	SerialTimeout = 0x3,
	BaudrateError = 0x2,
	ParityError = 0x1,
	None = 0
};

HANDLE connectWallboard(const char* port);
void sendWallMessage(HANDLE hndPort,const std::string& sayIt, char pos, long style, char col, unsigned char special, bool dumppkt);
wallboardErrorState checkWallboardErrorState(HANDLE hndPort);
void resetWallboard(HANDLE hndPort);
void closeWallboard(HANDLE hndPort);
bool readPacket(HANDLE hndPort,char* packet);