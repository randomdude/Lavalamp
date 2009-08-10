# MPLAB IDE generated this makefile for use with GNU make.
# Project: lavalamp.mcp
# Date: Mon Jun 09 13:22:58 2008

AS = mpasmwin.exe
CC = mcc18.exe
LD = mplink.exe
AR = mplib.exe
RM = rm

lavalamp.cof : main.o tea.o init.o protocol.o maths.o cmd-ping.o cmd-ident.o testbench.o cmd-get-sensor.o debugger.o cmd-set-sensor.o swuart.o autogen_sensorcode_init.o eeprom-contents.o eeprom.o
	$(LD) "linker-16f628.lkr" "main.o" "tea.o" "init.o" "protocol.o" "maths.o" "cmd-ping.o" "cmd-ident.o" "testbench.o" "cmd-get-sensor.o" "debugger.o" "cmd-set-sensor.o" "swuart.o" "autogen_sensorcode_init.o" "eeprom-contents.o" "eeprom.o" /z__MPLAB_BUILD=1 /m"lavalamp.map" /o"lavalamp.cof"

main.o : main.asm main.h  tea.h init.h protocol.h cmds.h maths.h testbench.h debugger.h swuart.h eeprom.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "main.asm" /l"main.lst" /e"main.err" /o"main.o"

tea.o : tea.asm main.h  maths.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "tea.asm" /l"tea.lst" /e"tea.err" /o"tea.o"

init.o : init.asm main.h  swuart.h eeprom.h autogen_sensorcode.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "init.asm" /l"init.lst" /e"init.err" /o"init.o"

protocol.o : protocol.asm main.h  tea.h init.h swuart.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "protocol.asm" /l"protocol.lst" /e"protocol.err" /o"protocol.o"

maths.o : maths.asm main.h  tea.h init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "maths.asm" /l"maths.lst" /e"maths.err" /o"maths.o"

cmd-ping.o : cmd-ping.asm main.h  debugger.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-ping.asm" /l"cmd-ping.lst" /e"cmd-ping.err" /o"cmd-ping.o"

cmd-ident.o : cmd-ident.asm main.h 
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-ident.asm" /l"cmd-ident.lst" /e"cmd-ident.err" /o"cmd-ident.o"

testbench.o : testbench.asm main.h  tea.h maths.h init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "testbench.asm" /l"testbench.lst" /e"testbench.err" /o"testbench.o"

cmd-get-sensor.o : cmd-get-sensor.asm main.h  sensorcfg.h processnamedcmd.asm debugger.h protocol.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-get-sensor.asm" /l"cmd-get-sensor.lst" /e"cmd-get-sensor.err" /o"cmd-get-sensor.o"

debugger.o : debugger.asm main.h  init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "debugger.asm" /l"debugger.lst" /e"debugger.err" /o"debugger.o"

cmd-set-sensor.o : cmd-set-sensor.asm main.h  sensorcfg.h processnamedcmd.asm debugger.h protocol.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-set-sensor.asm" /l"cmd-set-sensor.lst" /e"cmd-set-sensor.err" /o"cmd-set-sensor.o"

swuart.o : swuart.asm main.h  init.h debugger.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "swuart.asm" /l"swuart.lst" /e"swuart.err" /o"swuart.o"

autogen_sensorcode_init.o : autogen_sensorcode_init.asm sensorcfg.h main.h 
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "autogen_sensorcode_init.asm" /l"autogen_sensorcode_init.lst" /e"autogen_sensorcode_init.err" /o"autogen_sensorcode_init.o"

eeprom-contents.o : eeprom-contents.asm sensorcfg.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "eeprom-contents.asm" /l"eeprom-contents.lst" /e"eeprom-contents.err" /o"eeprom-contents.o"

eeprom.o : eeprom.asm sensorcfg.h main.h 
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "eeprom.asm" /l"eeprom.lst" /e"eeprom.err" /o"eeprom.o"

clean : 
	$(RM) "main.o" "main.ERR" "main.lst" "tea.o" "tea.ERR" "tea.lst" "init.o" "init.ERR" "init.lst" "protocol.o" "protocol.ERR" "protocol.lst" "maths.o" "maths.ERR" "maths.lst" "cmd-ping.o" "cmd-ping.ERR" "cmd-ping.lst" "cmd-ident.o" "cmd-ident.ERR" "cmd-ident.lst" "testbench.o" "testbench.ERR" "testbench.lst" "cmd-get-sensor.o" "cmd-get-sensor.ERR" "cmd-get-sensor.lst" "debugger.o" "debugger.ERR" "debugger.lst" "cmd-set-sensor.o" "cmd-set-sensor.ERR" "cmd-set-sensor.lst" "swuart.o" "swuart.ERR" "swuart.lst" "autogen_sensorcode_init.o" "autogen_sensorcode_init.ERR" "autogen_sensorcode_init.lst" "eeprom-contents.o" "eeprom-contents.ERR" "eeprom-contents.lst" "eeprom.o" "eeprom.ERR" "eeprom.lst" "lavalamp.cof" "lavalamp.hex" "lavalamp.cod" "lavalamp.lst" "lavalamp.map"

