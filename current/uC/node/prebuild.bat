
cd C:\c0adz\lavalamp\current\uC\node
perl regen.pl template_autogen_sensorcode_init.asm > autogen_sensorcode_init.asm
perl regen.pl template_cmd-get-sensor.asm > cmd-get-sensor.asm
perl regen.pl template_cmd-set-sensor.asm > cmd-set-sensor.asm
perl regen.pl template_cmd-fade-sensor.asm > cmd-fade-sensor.asm
perl regen.pl template_cmd-get-sensor-type.asm > cmd-get-sensor-type.asm
perl regen.pl template_memoryplacement.h > memoryplacement.h
perl regen.pl template_idle-pwm.asm > idle-pwm.asm
REM perl regen.pl template_idle-triac.asm > idle-triac.asm
