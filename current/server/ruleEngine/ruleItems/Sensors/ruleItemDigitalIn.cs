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
    public class ruleItemDigitalIn : ruleItemBase
    {
        public sensor connectedSensor;
        private bool _inVal;

        public override string ruleName()
        {
            return "Digital in";
        }

        public override string caption()
        {
            return "Digital in";
        }

        public override IFormOptions setupOptions()
        {
            SensorOptions options = new SensorOptions(sensorTypeEnum.generic_digital_in, this.connectedSensor);
            
            return options;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var pinList = base.getPinInfo();
            pinList.Add("trigger",new pin
            {
                name = "trigger", 
                description = "check digital in", 
                direction = pinDirection.input,
                valueType = typeof(pinDataTrigger)
            });
            pinList.Add("in", new pin
            {
                name = "in",
                description = "Digital in",
                direction = pinDirection.output,
            });
            return pinList;
        }

        public override void evaluate()
        {
            if (this.pinInfo["trigger"].value.asBoolean())
            {
                try
                {
                    //digital is only boolean atm this will hopefully change in the future
                    bool newVal = (bool) this.connectedSensor.getValue(true);
                    if (newVal != this._inVal)
                    {
                        this.onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(newVal , this , this.pinInfo["in"])));
                    }
                    this._inVal = newVal;
                }
                catch(Exception e)
                {
                    this.errorHandler(e);
                }
            }
            
        }
    }
}
