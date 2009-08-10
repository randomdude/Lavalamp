using System;
using System.Collections.Generic;
using netGui.Properties;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_not : ruleItemBase
    {
        public override string ruleName() { return "NOT function"; }

        public override System.Drawing.Image background()
        {
            return netGui.Properties.Resources.ruleItem_not;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input to invert", direction = pinDirection.input });
            pinList.Add("output1", new pin { name = "output1", description = "input is false", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
            bool input1 = (bool)pinStates["input1"];

            bool newState;

            if (!input1)
                newState = true;
            else
                newState = false;

            // only set the output if neccesary! constantly setting the output will result in a stack overflow.
            if ((bool)pinStates["output1"] != newState)
                pinStates["output1"] = newState;
        }

        public ruleItem_not()
        {
            this.pinStates.evaluate = new evaluateDelegate(evaluate);
            pinStates.setErrorHandler(new errorDelegate(base.errorHandler));
        }
    }
}
