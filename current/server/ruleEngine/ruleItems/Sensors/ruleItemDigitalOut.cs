namespace ruleEngine.ruleItems.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using ruleEngine;
    using ruleEngine.nodes;
    using ruleEngine.ruleItems;

    using transmitterDriver;

    [ToolboxRule]
    [ToolboxRuleCategory("Node sensors")]
    public class ruleItemDigitalOut : ruleItemBase
    {
        
        public sensor selectedSensor;
        private object _prevVal;

        public override string ruleName()
        {
            return "Digital out";
        }

        public override string caption()
        {
            return "Digital out";
        }

        public override IFormOptions setupOptions()
        {
            SensorOptions options = new SensorOptions(sensorTypeEnum.generic_digital_out, this.selectedSensor);
            return options;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string, pin> pinList = base.getPinInfo();
            pinList.Add("out" , new pin
            {
                name = "out",
                description = "to sensor",
                direction = pinDirection.input,
            });
            return pinList;
        }

        public override void evaluate()
        {
            try
            {
                //the lack of type info here makes Kat an unhappy girl (TODO)
                if (this.pinInfo["out"].value.data != this._prevVal)
                {
                    this.selectedSensor.setValue(this.pinInfo["out"].value.data , true);
                }
                this._prevVal = this.pinInfo["out"].value.data;
            }
            catch(Exception e)
            {
                this.errorHandler(e);
            }
        }
    }
}
