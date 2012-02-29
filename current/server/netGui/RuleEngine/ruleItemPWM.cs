using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui;
using netGui.RuleEngine;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netgui.ruleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node Sensors")]
    class ruleItemPWM : ruleItemBase
    {
        public sensorSettings settings = new sensorSettings(sensorTypeEnum.pwm_out);

        private short[] _previousValues = new short[2]; 

        public override string ruleName()
        {
            return "PWM Sensor";
        }

        public override System.Windows.Forms.Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(settings);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                settings.selectedSensor = options.selectedSensor();
        }


        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("pwm_fadeDelay",
                        new pin
                            {
                                name = "pwm_fadeDelay",
                                description = "pwm fade delay",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataInt)
                            });
            pinList.Add("pwm_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "pwm brightness",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataInt)
                            });
            return pinList;
        }
        
        public override void evaluate()
        {
            short newPWMSpeedVal = (short) pinInfo["pwm_fadeDelay_" + settings.selectedSensor.id].value.data;
            short newPWMBrightnessVal = (short) pinInfo["pwm_brightness_" + settings.selectedSensor.id].value.data;
            if (newPWMSpeedVal != _previousValues[0])
                settings.selectedSensor.setValue(new pwm_speed(newPWMSpeedVal), true);
            _previousValues[0] = newPWMSpeedVal;
            if (newPWMBrightnessVal != _previousValues[1])
                settings.selectedSensor.setValue(new pwm_brightness(newPWMBrightnessVal), true);
            _previousValues[1] = newPWMBrightnessVal;
        }
    }
}
