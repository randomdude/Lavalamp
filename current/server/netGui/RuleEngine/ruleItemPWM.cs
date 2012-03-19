using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netgui.ruleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node sensors")]
    public class ruleItemPWM : ruleItemBase
    {
        public sensor selectedSensor;

        private short[] _previousValues = new short[2]; 

        public override string ruleName()
        {
            return "PWM sensor";
        }

        public override string caption()
        {
            return "PWM";
        }

        public override Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(sensorTypeEnum.pwm_out, selectedSensor);
            options.Closed += sensorOptClosed;

            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                selectedSensor = options.selectedSensor();
        }


        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("pwm_fadeDelay",
                        new pin
                            {
                                name = "pwm_fadeDelay",
                                description = "PWM Fade delay",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataInt)
                            });
            pinList.Add("pwm_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "PWM Brightness",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataInt)
                            });
            return pinList;
        }
        
        public override void evaluate()
        {
            try
            {
                short newPWMSpeedVal = Convert.ToInt16( pinInfo["pwm_fadeDelay"].value.data );
                short newPWMBrightnessVal = Convert.ToInt16( pinInfo["pwm_brightness"].value.data );
                
                if (newPWMSpeedVal != _previousValues[0])
                    selectedSensor.setValue(new pwm_speed(newPWMSpeedVal) , true);
                _previousValues[0] = newPWMSpeedVal;

                if (newPWMBrightnessVal != _previousValues[1])
                    selectedSensor.setValue(new pwm_brightness(newPWMBrightnessVal) , true);
                
                _previousValues[1] = newPWMBrightnessVal;
            }
            catch(Exception e)
            {
                errorHandler(e);
            }
        }
    }
}
