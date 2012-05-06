using System;
using System.Collections.Generic;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_and : ruleItemBase
    {
        public override string ruleName() { return "AND function"; }

        public override string caption()
        {
            return "AND";
        }

        public override IFormOptions setupOptions()
        {
            return null;
        }

        public override System.Drawing.Image background()
        {
            return Properties.Resources.ruleItem_and;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input pin", direction = pinDirection.input });
            pinList.Add("input2", new pin { name = "input2", description = "input pin", direction = pinDirection.input });
            pinList.Add("output1", new pin { name = "output1", description = "both inputs are true", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
            IPinData input1 = pinInfo["input1"].value;
            IPinData input2 = pinInfo["input2"].value;

            //propagate changed value
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataBool(input1.and(input2 as pinDataBool).asBoolean(), this, pinInfo["output1"])));
        }
    }
}
