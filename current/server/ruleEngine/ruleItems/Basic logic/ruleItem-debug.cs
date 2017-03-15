using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_debug : ruleItemBase 
    {
        private bool lastState;

        public override string typedName
        {
            get
            {
                return "Debug";
            }
        }

        public override string ruleName() { return "Debug indicator"; }

        public override string caption()
        {
            return "Debug";
        }

        public override IFormOptions setupOptions()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "to monitor", direction = pinDirection.input, valueType = typeof(pinDataTristate) });
            pinList.Add("output", new pin { name = "output", description = "input is true", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            if (pinInfo["input"].value.asBoolean() != lastState)
            {

                lastState = pinInfo["input"].value.asBoolean();
            }

            if (pinInfo["output"].value.data != pinInfo["input"].value.data)
            {
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTristate((tristate)pinInfo["input"].value.data, this, pinInfo["output"])));
            }
        }


        public ruleItem_debug()
        {

        }
    }
}
