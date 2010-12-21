using System;
using System.Collections.Generic;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_splitter : ruleItemBase
    {
        public override string ruleName() { return "Splitter"; }

        public override System.Drawing.Image background()
        {
            return netGui.Properties.Resources.ruleItem_splitter;
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
            bool input1 = (bool)pinInfo["input1"].value.getData();

            // only set the outputs if necessary! constantly setting the output will result in a stack overflow.
            if ((bool)pinInfo["output1"].value.getData() != input1)
                pinInfo["output1"].value.setData(input1);
            if ((bool)pinInfo["output2"].value.getData() != input1)
                pinInfo["output2"].value.setData(input1);
        }
    }
}
