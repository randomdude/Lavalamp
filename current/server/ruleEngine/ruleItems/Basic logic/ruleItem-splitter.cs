using System;
using System.Collections.Generic;
using ruleEngine;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_splitter : ruleItemBase
    {
        public override string ruleName() { return "Splitter"; }

        public override System.Drawing.Image background()
        {
            return Properties.Resources.ruleItem_splitter;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input pin", direction = pinDirection.input });
            pinList.Add("output2", new pin { name = "output2", description = "input pin is true", direction = pinDirection.output });
            pinList.Add("output1", new pin { name = "output1", description = "input pin is true", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
            var input1 = (bool) pinInfo["input1"].value.data;
            foreach (var pin in pinInfo)
            {
                if (pin.Value.direction == pinDirection.output)
                    pin.Value.value.data = input1;
            }
        }
    }
}
