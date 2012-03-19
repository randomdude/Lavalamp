using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netGui.RuleEngine
{
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

        public override Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(sensorTypeEnum.generic_digital_in, connectedSensor);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                connectedSensor = options.selectedSensor();
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
            if (pinInfo["trigger"].value.asBoolean())
            {
                try
                {
                    //digital is only boolean atm this will hopefully change in the future
                    bool newVal = (bool) connectedSensor.getValue(true);
                    if (newVal != _inVal)
                    {
                        onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(newVal , this , pinInfo["in"])));
                    }
                    _inVal = newVal;
                }
                catch(Exception e)
                {
                    errorHandler(e);
                }
            }
            
        }
    }
}
