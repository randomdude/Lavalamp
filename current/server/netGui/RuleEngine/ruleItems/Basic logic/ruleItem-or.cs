﻿using System;
using System.Collections.Generic;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_or : ruleItemBase
    {
        public override string ruleName() { return "OR function"; }

        public override System.Drawing.Image background()
        {
            return netGui.Properties.Resources.ruleItem_or;
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
            bool input1 = (bool)pinInfo["input1"].value.getData();
            bool input2 = (bool)pinInfo["input2"].value.getData();

            bool newState;

            if (input1 || input2)
                newState = true;
            else
                newState = false;

            // only set the output if necessary! constantly setting the output will result in a stack overflow.
            if ((bool)pinInfo["output1"].value.getData() != newState)
                pinInfo["output1"].value.setData(newState);
        }

    }
}
