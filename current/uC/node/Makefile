# MPLAB IDE generated this makefile for use with GNU make.
# Project: lavalamp.mcp
# Date: Wed Jan 06 17:13:43 2010

AS = MPASMWIN.exe
CC = 
LD = mplink.exe
AR = mplib.exe
RM = rm

lavalamp.cof : main.o tea.o protocol.o maths.o cmd-ping.o cmd-ident.o cmd-get-sensor.o cmd-set-sensor.o autogen_sensorcode_init.o eeprom-contents.o eeprom.o init.o cmd-get-sensor-type.o idletimer.o cmd-fade-sensor.o idle-pwm.o cmd-set-p.o cmd-set-id.o cmd-set-key-byte.o cmd-reload-from-flash.o tests.o swuart-lowlevel.o swuart.o
	$(CC) /p16F628 "main.o" "tea.o" "protocol.o" "maths.o" "cmd-ping.o" "cmd-ident.o" "cmd-get-sensor.o" "cmd-set-sensor.o" "autogen_sensorcode_init.o" "eeprom-contents.o" "eeprom.o" "init.o" "cmd-get-sensor-type.o" "idletimer.o" "cmd-fade-sensor.o" "idle-pwm.o" "cmd-set-p.o" "cmd-set-id.o" "cmd-set-key-byte.o" "cmd-reload-from-flash.o" "tests.o" "swuart-lowlevel.o" "swuart.o" /z__MPLAB_BUILD=1 /m"lavalamp.map" /o"lavalamp.cof"

main.o : main.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h protocol.h cmds.h maths.h ../shared/swuart.h eeprom.h idletimer.h tests.h
	$(AS) /q /p16F628 "main.asm" /l"main.lst" /e"main.err" /o"main.o"

tea.o : tea.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc maths.h
	$(AS) /q /p16F628 "tea.asm" /l"tea.lst" /e"tea.err" /o"tea.o"

protocol.o : protocol.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h ../shared/swuart.h ../shared/swuart-lowlevel.h idletimer.h
	$(AS) /q /p16F628 "protocol.asm" /l"protocol.lst" /e"protocol.err" /o"protocol.o"

maths.o : maths.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h
	$(AS) /q /p16F628 "maths.asm" /l"maths.lst" /e"maths.err" /o"maths.o"

cmd-ping.o : cmd-ping.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "cmd-ping.asm" /l"cmd-ping.lst" /e"cmd-ping.err" /o"cmd-ping.o"

cmd-ident.o : cmd-ident.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc eeprom.h
	$(AS) /q /p16F628 "cmd-ident.asm" /l"cmd-ident.lst" /e"cmd-ident.err" /o"cmd-ident.o"

cmd-get-sensor.o : cmd-get-sensor.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm protocol.h
	$(AS) /q /p16F628 "cmd-get-sensor.asm" /l"cmd-get-sensor.lst" /e"cmd-get-sensor.err" /o"cmd-get-sensor.o"

cmd-set-sensor.o : cmd-set-sensor.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm protocol.h memoryplacement.h
	$(AS) /q /p16F628 "cmd-set-sensor.asm" /l"cmd-set-sensor.lst" /e"cmd-set-sensor.err" /o"cmd-set-sensor.o"

autogen_sensorcode_init.o : autogen_sensorcode_init.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc protocol.h memoryplacement.h
	$(AS) /q /p16F628 "autogen_sensorcode_init.asm" /l"autogen_sensorcode_init.lst" /e"autogen_sensorcode_init.err" /o"autogen_sensorcode_init.o"

eeprom-contents.o : eeprom-contents.asm sensorcfg.h
	$(AS) /q /p16F628 "eeprom-contents.asm" /l"eeprom-contents.lst" /e"eeprom-contents.err" /o"eeprom-contents.o"

eeprom.o : eeprom.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "eeprom.asm" /l"eeprom.lst" /e"eeprom.err" /o"eeprom.o"

init.o : ../shared/init.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc ../SHARED/swuart.h sensorcfg.h idletimer.h eeprom.h autogen_sensorcode.h
	$(AS) /q /p16F628 "..\shared\init.asm" /l"init.lst" /e"init.err" /o"init.o"

cmd-get-sensor-type.o : cmd-get-sensor-type.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm protocol.h
	$(AS) /q /p16F628 "cmd-get-sensor-type.asm" /l"cmd-get-sensor-type.lst" /e"cmd-get-sensor-type.err" /o"cmd-get-sensor-type.o"

idletimer.o : idletimer.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h protocol.h memoryplacement.h idle-pwm.h
	$(AS) /q /p16F628 "idletimer.asm" /l"idletimer.lst" /e"idletimer.err" /o"idletimer.o"

cmd-fade-sensor.o : cmd-fade-sensor.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc protocol.h memoryplacement.h
	$(AS) /q /p16F628 "cmd-fade-sensor.asm" /l"cmd-fade-sensor.lst" /e"cmd-fade-sensor.err" /o"cmd-fade-sensor.o"

idle-pwm.o : idle-pwm.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h protocol.h memoryplacement.h
	$(AS) /q /p16F628 "idle-pwm.asm" /l"idle-pwm.lst" /e"idle-pwm.err" /o"idle-pwm.o"

cmd-set-p.o : cmd-set-p.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "cmd-set-p.asm" /l"cmd-set-p.lst" /e"cmd-set-p.err" /o"cmd-set-p.o"

cmd-set-id.o : cmd-set-id.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "cmd-set-id.asm" /l"cmd-set-id.lst" /e"cmd-set-id.err" /o"cmd-set-id.o"

cmd-set-key-byte.o : cmd-set-key-byte.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "cmd-set-key-byte.asm" /l"cmd-set-key-byte.lst" /e"cmd-set-key-byte.err" /o"cmd-set-key-byte.o"

cmd-reload-from-flash.o : cmd-reload-from-flash.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "cmd-reload-from-flash.asm" /l"cmd-reload-from-flash.lst" /e"cmd-reload-from-flash.err" /o"cmd-reload-from-flash.o"

tests.o : tests.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc ../shared/swuart.h ../shared/swuart-lowlevel.h
	$(AS) /q /p16F628 "tests.asm" /l"tests.lst" /e"tests.err" /o"tests.o"

swuart-lowlevel.o : ../shared/swuart-lowlevel.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	$(AS) /q /p16F628 "C:\lavalamp\current\uC\shared\swuart-lowlevel.asm" /l"swuart-lowlevel.lst" /e"swuart-lowlevel.err" /o"swuart-lowlevel.o"

swuart.o : ../shared/swuart.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc ../SHARED/swuart-lowlevel.h idletimer.h sensorcfg.h
	$(AS) /q /p16F628 "C:\lavalamp\current\uC\shared\swuart.asm" /l"swuart.lst" /e"swuart.err" /o"swuart.o"

clean : 
	$(CC) "main.o" "main.err" "main.lst" "tea.o" "tea.err" "tea.lst" "protocol.o" "protocol.err" "protocol.lst" "maths.o" "maths.err" "maths.lst" "cmd-ping.o" "cmd-ping.err" "cmd-ping.lst" "cmd-ident.o" "cmd-ident.err" "cmd-ident.lst" "cmd-get-sensor.o" "cmd-get-sensor.err" "cmd-get-sensor.lst" "cmd-set-sensor.o" "cmd-set-sensor.err" "cmd-set-sensor.lst" "autogen_sensorcode_init.o" "autogen_sensorcode_init.err" "autogen_sensorcode_init.lst" "eeprom-contents.o" "eeprom-contents.err" "eeprom-contents.lst" "eeprom.o" "eeprom.err" "eeprom.lst" "init.o" "init.err" "init.lst" "cmd-get-sensor-type.o" "cmd-get-sensor-type.err" "cmd-get-sensor-type.lst" "idletimer.o" "idletimer.err" "idletimer.lst" "cmd-fade-sensor.o" "cmd-fade-sensor.err" "cmd-fade-sensor.lst" "idle-pwm.o" "idle-pwm.err" "idle-pwm.lst" "cmd-set-p.o" "cmd-set-p.err" "cmd-set-p.lst" "cmd-set-id.o" "cmd-set-id.err" "cmd-set-id.lst" "cmd-set-key-byte.o" "cmd-set-key-byte.err" "cmd-set-key-byte.lst" "cmd-reload-from-flash.o" "cmd-reload-from-flash.err" "cmd-reload-from-flash.lst" "tests.o" "tests.err" "tests.lst" "swuart-lowlevel.o" "swuart-lowlevel.err" "swuart-lowlevel.lst" "swuart.o" "swuart.err" "swuart.lst" "lavalamp.cof" "lavalamp.hex" "lavalamp.cod" "lavalamp.lst" "lavalamp.map"

