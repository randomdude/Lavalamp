namespace ruleEngine.ruleItems.Sensors
{
    using System;
    using System.Collections.Generic;

    using ruleEngine;
    using ruleEngine.nodes;
    using ruleEngine.ruleItems;
    using ruleEngine.pinDataTypes;

    [ToolboxRule]
    [ToolboxRuleCategory("Node sensors")]
    public class ruleItemTriac : ruleItemBase
    {
        public SensorOptions options;
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

        public override IFormOptions setupOptions()
        {
            return this.options;
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
                                valueType = typeof(pinDataNumber)
                            });
            pinList.Add("triac_brightness",
                        new pin
                            {
                                name = "pwm_brightness",
                                description = "pwm brightness",
                                direction = pinDirection.input,
                                valueType = typeof(pinDataNumber)
                            });
            return pinList;
        } 
        public override void evaluate()
        {
            try
            {
                short triacFade = (short)this.pinInfo["pwm_fadeDelay"].value.data;
                short triacBrightness = (short) this.pinInfo["pwm_brightness"].value.data;
                if (triacFade != this._pwmFadeDelayLast)
                    this.selectedSensor.setValue(new pwm_speed(triacFade) , true);
                this._pwmFadeDelayLast = triacFade;

                if (triacBrightness != this._pwmBrightnessLast)
                    this.selectedSensor.setValue(new pwm_brightness(triacBrightness) , true);
                this._pwmBrightnessLast = triacBrightness;
            }
            catch (Exception e)
            {
                this.errorHandler(e);
            }
        }
    }
}
