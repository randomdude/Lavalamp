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

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "input to invert", direction = pinDirection.input, valueType = typeof (pinDataTristate) });
            pinList.Add("output", new pin { name = "output", description = "input is false", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            var newOutput = pinInfo["input"].value.not() ;

            // only set the output if necessary! constantly setting the output will result in a stack overflow.
            if (pinInfo["output"].value.data != newOutput.data)
                 pinInfo["output"].value.data = newOutput;
        }
    }
}
