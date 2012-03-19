
cd C:\c0adz\lavalamp\current\uC\node
C:\cygwin\bin\perl regen.pl template_autogen_sensorcode_init.asm > autogen_sensorcode_init.asm
C:\cygwin\bin\perl regen.pl template_cmd-get-sensor.asm > cmd-get-sensor.asm
C:\cygwin\bin\perl regen.pl template_cmd-set-sensor.asm > cmd-set-sensor.asm
C:\cygwin\bin\perl regen.pl template_cmd-fade-sensor.asm > cmd-fade-sensor.asm
C:\cygwin\bin\perl regen.pl template_cmd-get-sensor-type.asm > cmd-get-sensor-type.asm
C:\cygwin\bin\perl regen.pl template_memoryplacement.h > memoryplacement.h
C:\cygwin\bin\perl regen.pl template_idle-pwm.asm > idle-pwm.asm
REM perl regen.pl template_idle-triac.asm > idle-triac.asm
