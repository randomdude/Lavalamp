using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netGui.RuleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node Sensors")]
    class ruleItemTriac : ruleItemBase
    {
        public sensorSettings settings;
        private short _pwmBrightnessLast;
        private short _pwmFadeDelayLast;

        public override string ruleName()
        {
            return "Triac Sensor";
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

        public override Dictionary<string, ruleEngine.pin> getPinInfo()
        {
            var pinList = base.getPinInfo();
            pinList.Add("triac_fadeDelay",
                        new pin
                            {
                                name = "pwm_fadeDelay",
                                description = "pwm fade delay",
                                direction = pinDirection.input
                            });
            pinList.Add("triac_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "pwm brightness",
                                direction = pinDirection.input
                            });
            return pinList;
        } 
        public override void evaluate()
        {
            if ((short)pinInfo["pwm_fadeDelay"].value.data != _pwmFadeDelayLast)
                settings.selectedSensor.setValue(new pwm_speed((short)pinInfo["pwm_fadeDelay"].value.data), true);
            if ((short)pinInfo["pwm_brightness"].value.data != _pwmBrightnessLast)
                settings.selectedSensor.setValue(new pwm_brightness((short)pinInfo["pwm_fadeDelay"].value.data), true);
 
        }
    }

    [Serializable]
    internal class sensorSettings
    {
        public sensor selectedSensor;
    }
}
