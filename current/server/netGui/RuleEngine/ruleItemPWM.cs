using System;
using System.Collections.Generic;
using netGui;
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
        private sensor _sensor;
        private object[] _previousValues; 

        public override string ruleName()
        {
            return "PWM Sensor";
        }

        public override System.Windows.Forms.Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(typeof(sensorTypeEnum));
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
                            });
            pinList.Add("pwm_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "pwm brightness",
                                direction = pinDirection.input,
                            });
            return pinList;
        }
        
        public override void evaluate()
        {
          
            if (pinInfo["pwm_fadeDelay_" + _sensor.id ].value.data != _previousValues[0])
                _sensor.setValue(new pwm_speed((short) pinInfo["pwm_fadeDelay_" + _sensor.id].value.data),true);
            if (pinInfo["pwm_brightness_" + _sensor.id].value.data != _previousValues[1])
                _sensor.setValue(new pwm_brightness((short)pinInfo["pwm_fadeDelay_" + _sensor.id].value.data),true);
        }
    }
}
