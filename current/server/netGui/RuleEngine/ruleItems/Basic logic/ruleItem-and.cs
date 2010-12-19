﻿using System;
using System.Collections.Generic;
using netGui.Properties;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_and : ruleItemBase
    {
        public override string ruleName() { return "AND function"; }

        public override System.Drawing.Image background()
        {
            return netGui.Properties.Resources.ruleItem_and;
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
            bool input1 = (bool)pinStates["input1"].getData();
            bool input2 = (bool)pinStates["input2"].getData();

            bool newState;

            if (input1 && input2)
                newState = true;
            else
                newState = false;

            // only set the output if necessary! constantly setting the output will result in a stack overflow.
            if ((bool)pinStates["output1"].getData() != newState)
                pinStates["output1"].setData(newState);
        }

        public ruleItem_and()
        {
        }
    }
}
