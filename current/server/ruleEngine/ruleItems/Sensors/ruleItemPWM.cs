namespace ruleEngine.ruleItems.Sensors
{
    using System;
    using System.Collections.Generic;
    using ruleEngine;
    using ruleEngine.nodes;
    using ruleEngine.pinDataTypes;
    using ruleEngine.ruleItems;

    using transmitterDriver;

    [ToolboxRule]
    [ToolboxRuleCategory("Node sensors")]
    public class ruleItemPWM : ruleItemBase
    {
        public sensor selectedSensor;

        private short[] _previousValues = new short[2];

        public override string typedName
        {
            get
            {
                return "PWM";
            }
        }

        public override string ruleName()
        {
            return "PWM sensor";
        }

        public override string caption()
        {
            return "PWM";
        }

        public override IFormOptions setupOptions()
        {
            SensorOptions options = new SensorOptions(sensorTypeEnum.pwm_out, this.selectedSensor);
            return options;
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
                                valueType = typeof(pinDataNumber)
                            });
            pinList.Add("pwm_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "PWM Brightness",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataNumber)
                            });
            return pinList;
        }
        
        public override void evaluate()
        {
            try
            {
                short newPWMSpeedVal = Convert.ToInt16( this.pinInfo["pwm_fadeDelay"].value.data );
                short newPWMBrightnessVal = Convert.ToInt16( this.pinInfo["pwm_brightness"].value.data );
                
                if (newPWMSpeedVal != this._previousValues[0])
                    this.selectedSensor.setValue(new pwm_speed(newPWMSpeedVal) , true);
                this._previousValues[0] = newPWMSpeedVal;

                if (newPWMBrightnessVal != this._previousValues[1])
                    this.selectedSensor.setValue(new pwm_brightness(newPWMBrightnessVal) , true);
                
                this._previousValues[1] = newPWMBrightnessVal;
            }
            catch(Exception e)
            {
                this.errorHandler(e);
            }
        }
    }
}
