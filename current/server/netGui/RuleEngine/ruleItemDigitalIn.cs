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
    class ruleItemDigitalIn : ruleItemBase
    {
        private sensorSettings _settings;
        private object inVal;

        public override string ruleName()
        {
            return "Digital In";
        }

        public override System.Windows.Forms.Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(typeof(sensorTypeEnum));
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            
        }

        public override Dictionary<string, ruleEngine.pin> getPinInfo()
        {
            var pinList = base.getPinInfo();
            pinList.Add("in", new pin
            {
                name = "in",
                description = "digital in",
                direction = pinDirection.output,
            });
            return pinList;
        }

        public override void evaluate()
        {
            if (pinInfo["in"].value.data != inVal)
                _settings.selectedSensor.setValue(pinInfo["in"].value.data, true);
        }
    }
}
