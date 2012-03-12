using System;
using System.Collections.Generic;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_not : ruleItemBase
    {
        public override string ruleName() { return "NOT function"; }

        public override System.Drawing.Image background() { return Properties.Resources.ruleItem_not; }

        public override string caption()
        {
            return "NOT";
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input to invert", direction = pinDirection.input, valueType = typeof (pinDataTristate),possibleTypes = new[]{typeof(pinDataBool), typeof(pinDataTristate)} });
            pinList.Add("output1", new pin { name = "output1", description = "input is false", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            var newOutput = pinInfo["input1"].value.not() ;

            // only set the output if necessary! constantly setting the output will result in a stack overflow.
            if (pinInfo["output1"].value.data != newOutput.data)
            {
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTristate((tristate)newOutput.data, this, pinInfo["output1"])));
            }
        }
    }
}
