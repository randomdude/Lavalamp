using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.ruleItems;
using ruleEngine.pinDataTypes;
using transmitterDriver;

namespace netGui.RuleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node sensors")]
    public class ruleItemTriac : ruleItemBase
    {
        public sensor selectedSensor;
        private short _pwmBrightnessLast;
        private short _pwmFadeDelayLast;

        public override string ruleName()
        {
            return "Triac sensor";
        }

        public override string caption()
        {
            return "Triac";
        }

        public override Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(sensorTypeEnum.triac_out, selectedSensor);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                selectedSensor = options.selectedSensor();
        }

        public override Dictionary<string, ruleEngine.pin> getPinInfo()
        {
            var pinList = base.getPinInfo();
            pinList.Add("triac_fadeDelay",
                        new pin
                            {
                                name = "pwm_fadeDelay",
                                description = "pwm fade delay",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataInt)
                            });
            pinList.Add("triac_brightness",
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
            try
            {
                short triacFade = (short)pinInfo["pwm_fadeDelay"].value.data;
                short triacBrightness = (short) pinInfo["pwm_brightness"].value.data;
                if (triacFade != _pwmFadeDelayLast)
                    selectedSensor.setValue(new pwm_speed(triacFade) , true);
                _pwmFadeDelayLast = triacFade;

                if (triacBrightness != _pwmBrightnessLast)
                    selectedSensor.setValue(new pwm_brightness(triacBrightness) , true);
                _pwmBrightnessLast = triacBrightness;
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }
    }
}
