using System;
using System.Collections.Generic;
using ruleEngine;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_and : ruleItemBase
    {
        public override string ruleName() { return "AND function"; }

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
            IpinData input1 = pinInfo["input1"].value;
            IpinData input2 = pinInfo["input2"].value;

            //selects the pin state to use based on the priority of the pin(s).
            var newState = pinInfo["input1"].isPriority() ? input1.and(input2) : input2.and(input1);
            
            pinInfo["output1"].value.data = newState.data;
        }
    }
}
