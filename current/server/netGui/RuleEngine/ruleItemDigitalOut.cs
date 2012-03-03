using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui.RuleEngine.windows;
using ruleEngine;
using ruleEngine.ruleItems;
using transmitterDriver;

namespace netGui.RuleEngine
{
    [ToolboxRule]
    [ToolboxRuleCategory("Node Sensors")]
    public class ruleItemDigitalOut : ruleItemBase
    {
        
        public sensor selectedSensor;
        private object _prevVal;

        public override string ruleName()
        {
            return "Digital Out";
        }

        public override string caption()
        {
            return "Digital Out";
        }

        public override Form ruleItemOptions()
        {
            frmSensorOptions options = new frmSensorOptions(sensorTypeEnum.generic_digital_out, selectedSensor);
            options.Closed += sensorOptClosed;
            return options;
        }

        private void sensorOptClosed(object sender, EventArgs e)
        {
            frmSensorOptions options = (frmSensorOptions)sender;
            if (options.DialogResult == DialogResult.OK)
                selectedSensor = options.selectedSensor();
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
                if (pinInfo["out"].value.data != _prevVal)
                {
                    selectedSensor.setValue(pinInfo["out"].value.data , true);
                }
                _prevVal = pinInfo["out"].value.data;
            }
            catch(Exception e)
            {
                errorHandler(e);
            }
        }
    }
}
