using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class ruleItemDigitalOut : ruleItemBase
    {
        public sensorSettings settings = new sensorSettings(sensorTypeEnum.generic_digital_out);
        private object _prevVal;

        public override string ruleName()
        {
            return "Digital Out";
        }

        public override System.Windows.Forms.Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(settings);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                settings.selectedSensor = options.selectedSensor(); ;
        }


        public override Dictionary<string, ruleEngine.pin> getPinInfo()
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
            //the lack of type info here makes Kat an unhappy girl (TODO)
            if (pinInfo["out"].value.data != _prevVal)
            {
                settings.selectedSensor.setValue(pinInfo["out"].value.data, true);
            }
            _prevVal = pinInfo["out"].value.data;

        }
    }
}
