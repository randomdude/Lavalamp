using System;
using System.Collections.Generic;
using netGui.Properties;

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
            bool input1 = (bool)pinStates["input1"];

            // only set the outputs if neccesary! constantly setting the output will result in a stack overflow.
            if ((bool)pinStates["output1"] != input1)
                pinStates["output1"] = input1;
            if ((bool)pinStates["output2"] != input1)
                pinStates["output2"] = input1;
        }

        public ruleItem_splitter()
        {
            this.pinStates.evaluate = new evaluateDelegate(evaluate);
            pinStates.setErrorHandler(new errorDelegate(base.errorHandler));
        }
    }
}
