using System;
using System.Collections.Generic;
using ruleEngine;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_or : ruleItemBase
    {
        public override string ruleName() { return "OR function"; }

        public override System.Drawing.Image background()
        {
            return Properties.Resources.ruleItem_or;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input pin", direction = pinDirection.input });
            pinList.Add("input2", new pin { name = "input2", description = "input pin", direction = pinDirection.input });
            pinList.Add("output1", new pin { name = "output1", description = "either input is true", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
            IPinData input1 = pinInfo["input1"].value;
            IPinData input2 = pinInfo["input2"].value;

            var newState = pinInfo["input1"].isPriority() ? input1.or(input2) : input2.or(input1);

            pinInfo["output1"].value.data = newState;
        }

    }
}
