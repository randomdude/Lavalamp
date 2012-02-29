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
    [ToolboxRuleCategory("Node Sensors")]
    class ruleItemDigitalIn : ruleItemBase
    {
        public sensorSettings settings = new sensorSettings(sensorTypeEnum.generic_digital_in);
        private bool _inVal;

        public override string ruleName()
        {
            return "Digital in";
        }

        public override Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(settings);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                settings.selectedSensor = options.selectedSensor();
        }

        public override Dictionary<string, ruleEngine.pin> getPinInfo()
        {
            var pinList = base.getPinInfo();
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
            //digital is only boolean atm this will hopefully change in the future
            var newVal = (bool)settings.selectedSensor.getValue(true);
            if (newVal != _inVal)
            {
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(newVal, this, pinInfo["in"])));
            }
            _inVal = newVal;
        }
    }
}
