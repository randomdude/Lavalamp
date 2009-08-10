# MPLAB IDE generated this makefile for use with GNU make.
# Project: lavalamp.mcp
# Date: Sun Aug 03 21:19:49 2008

AS = mpasmwin.exe
CC = mcc18.exe
LD = mplink.exe
AR = mplib.exe
RM = rm

lavalamp.cof : main.o tea.o protocol.o maths.o cmd-ping.o cmd-ident.o testbench.o cmd-get-sensor.o debugger.o cmd-set-sensor.o autogen_sensorcode_init.o eeprom-contents.o eeprom.o swuart.o init.o cmd-get-sensor-type.o idletimer.o cmd-fade-sensor.o idle-pwm.o cmd-set-p.o cmd-set-id.o cmd-set-key-byte.o cmd-reload-from-flash.o
	$(LD) "..\shared\linker-16f628.lkr" "main.o" "tea.o" "protocol.o" "maths.o" "cmd-ping.o" "cmd-ident.o" "testbench.o" "cmd-get-sensor.o" "debugger.o" "cmd-set-sensor.o" "autogen_sensorcode_init.o" "eeprom-contents.o" "eeprom.o" "swuart.o" "init.o" "cmd-get-sensor-type.o" "idletimer.o" "cmd-fade-sensor.o" "idle-pwm.o" "cmd-set-p.o" "cmd-set-id.o" "cmd-set-key-byte.o" "cmd-reload-from-flash.o" /u_DEBUG /z__MPLAB_BUILD=1 /z__MPLAB_DEBUG=1 /m"lavalamp.map" /o"lavalamp.cof"

main.o : main.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h protocol.h cmds.h maths.h testbench.h debugger.h ../shared/swuart.h eeprom.h idletimer.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "main.asm" /l"main.lst" /e"main.err" /o"main.o" /d__DEBUG=1

tea.o : tea.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc maths.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "tea.asm" /l"tea.lst" /e"tea.err" /o"tea.o" /d__DEBUG=1

protocol.o : protocol.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h ../shared/swuart.h idletimer.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "protocol.asm" /l"protocol.lst" /e"protocol.err" /o"protocol.o" /d__DEBUG=1

maths.o : maths.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h ../shared/init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "maths.asm" /l"maths.lst" /e"maths.err" /o"maths.o" /d__DEBUG=1

cmd-ping.o : cmd-ping.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc debugger.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-ping.asm" /l"cmd-ping.lst" /e"cmd-ping.err" /o"cmd-ping.o" /d__DEBUG=1

cmd-ident.o : cmd-ident.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc eeprom.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-ident.asm" /l"cmd-ident.lst" /e"cmd-ident.err" /o"cmd-ident.o" /d__DEBUG=1

testbench.o : testbench.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc tea.h maths.h ../shared/init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "testbench.asm" /l"testbench.lst" /e"testbench.err" /o"testbench.o" /d__DEBUG=1

cmd-get-sensor.o : cmd-get-sensor.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm debugger.h protocol.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-get-sensor.asm" /l"cmd-get-sensor.lst" /e"cmd-get-sensor.err" /o"cmd-get-sensor.o" /d__DEBUG=1

debugger.o : debugger.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc ../shared/init.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "debugger.asm" /l"debugger.lst" /e"debugger.err" /o"debugger.o" /d__DEBUG=1

cmd-set-sensor.o : cmd-set-sensor.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm debugger.h protocol.h memoryplacement.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-set-sensor.asm" /l"cmd-set-sensor.lst" /e"cmd-set-sensor.err" /o"cmd-set-sensor.o" /d__DEBUG=1

autogen_sensorcode_init.o : autogen_sensorcode_init.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc protocol.h memoryplacement.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "autogen_sensorcode_init.asm" /l"autogen_sensorcode_init.lst" /e"autogen_sensorcode_init.err" /o"autogen_sensorcode_init.o" /d__DEBUG=1

eeprom-contents.o : eeprom-contents.asm sensorcfg.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "eeprom-contents.asm" /l"eeprom-contents.lst" /e"eeprom-contents.err" /o"eeprom-contents.o" /d__DEBUG=1

eeprom.o : eeprom.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "eeprom.asm" /l"eeprom.lst" /e"eeprom.err" /o"eeprom.o" /d__DEBUG=1

swuart.o : ../shared/swuart.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc debugger.h idletimer.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "..\shared\swuart.asm" /l"swuart.lst" /e"swuart.err" /o"swuart.o" /d__DEBUG=1

init.o : ../shared/init.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc ../SHARED/swuart.h idletimer.h eeprom.h autogen_sensorcode.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "..\shared\init.asm" /l"init.lst" /e"init.err" /o"init.o" /d__DEBUG=1

cmd-get-sensor-type.o : cmd-get-sensor-type.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h processnamedcmd.asm debugger.h protocol.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-get-sensor-type.asm" /l"cmd-get-sensor-type.lst" /e"cmd-get-sensor-type.err" /o"cmd-get-sensor-type.o" /d__DEBUG=1

idletimer.o : idletimer.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h protocol.h memoryplacement.h idle-pwm.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "idletimer.asm" /l"idletimer.lst" /e"idletimer.err" /o"idletimer.o" /d__DEBUG=1

cmd-fade-sensor.o : cmd-fade-sensor.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc protocol.h memoryplacement.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-fade-sensor.asm" /l"cmd-fade-sensor.lst" /e"cmd-fade-sensor.err" /o"cmd-fade-sensor.o" /d__DEBUG=1

idle-pwm.o : idle-pwm.asm main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc sensorcfg.h protocol.h memoryplacement.h
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "idle-pwm.asm" /l"idle-pwm.lst" /e"idle-pwm.err" /o"idle-pwm.o" /d__DEBUG=1

cmd-set-p.o : cmd-set-p.asm sensorcfg.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-set-p.asm" /l"cmd-set-p.lst" /e"cmd-set-p.err" /o"cmd-set-p.o" /d__DEBUG=1

cmd-set-id.o : cmd-set-id.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-set-id.asm" /l"cmd-set-id.lst" /e"cmd-set-id.err" /o"cmd-set-id.o" /d__DEBUG=1

cmd-set-key-byte.o : cmd-set-key-byte.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-set-key-byte.asm" /l"cmd-set-key-byte.lst" /e"cmd-set-key-byte.err" /o"cmd-set-key-byte.o" /d__DEBUG=1

cmd-reload-from-flash.o : cmd-reload-from-flash.asm sensorcfg.h eeprom.h main.h ../../../../Program\ Files/Microchip/MPASM\ Suite/p16f628.inc
	"C:\Program Files\Microchip\MPASM Suite\MPASMWIN.exe" /q /p16F628 "cmd-reload-from-flash.asm" /l"cmd-reload-from-flash.lst" /e"cmd-reload-from-flash.err" /o"cmd-reload-from-flash.o" /d__DEBUG=1

clean : 
	$(RM) "main.o" "main.ERR" "main.lst" "tea.o" "tea.ERR" "tea.lst" "protocol.o" "protocol.ERR" "protocol.lst" "maths.o" "maths.ERR" "maths.lst" "cmd-ping.o" "cmd-ping.ERR" "cmd-ping.lst" "cmd-ident.o" "cmd-ident.ERR" "cmd-ident.lst" "testbench.o" "testbench.ERR" "testbench.lst" "cmd-get-sensor.o" "cmd-get-sensor.ERR" "cmd-get-sensor.lst" "debugger.o" "debugger.ERR" "debugger.lst" "cmd-set-sensor.o" "cmd-set-sensor.ERR" "cmd-set-sensor.lst" "autogen_sensorcode_init.o" "autogen_sensorcode_init.ERR" "autogen_sensorcode_init.lst" "eeprom-contents.o" "eeprom-contents.ERR" "eeprom-contents.lst" "eeprom.o" "eeprom.ERR" "eeprom.lst" "swuart.o" "swuart.ERR" "..\shared\swuart.lst" "init.o" "init.ERR" "..\shared\init.lst" "cmd-get-sensor-type.o" "cmd-get-sensor-type.ERR" "cmd-get-sensor-type.lst" "idletimer.o" "idletimer.ERR" "idletimer.lst" "cmd-fade-sensor.o" "cmd-fade-sensor.ERR" "cmd-fade-sensor.lst" "idle-pwm.o" "idle-pwm.ERR" "idle-pwm.lst" "cmd-set-p.o" "cmd-set-p.ERR" "cmd-set-p.lst" "cmd-set-id.o" "cmd-set-id.ERR" "cmd-set-id.lst" "cmd-set-key-byte.o" "cmd-set-key-byte.ERR" "cmd-set-key-byte.lst" "cmd-reload-from-flash.o" "cmd-reload-from-flash.ERR" "cmd-reload-from-flash.lst" "lavalamp.cof" "lavalamp.hex" "lavalamp.cod" "lavalamp.lst" "lavalamp.map"

