using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netGui.RuleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node Sensors")]
    class ruleItemDigitalOut : ruleItemBase
    {
        private sensorSettings _settings;
        private bool pollPrevVal;
        private bool prevSetVal;

        public override string ruleName()
        {
            return "Digital Out";
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
            if (pinInfo["poll"].value.asBoolean() != (bool)pollPrevVal)
            {
                bool val = (bool)_settings.selectedSensor.getValue(true);
                if (val != (bool)prevSetVal)
                {
                    pinInfo["out"].value.data = val;
                    //propagate
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(val, this, pinInfo["out"])));
                }
            }
        }
    }
}
