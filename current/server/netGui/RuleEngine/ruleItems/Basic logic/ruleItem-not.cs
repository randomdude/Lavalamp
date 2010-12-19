using System;
using System.Collections.Generic;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_not : ruleItemBase
    {
        public override string ruleName() { return "NOT function"; }

        public override System.Drawing.Image background() { return netGui.Properties.Resources.ruleItem_not; }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "input to invert", direction = pinDirection.input, type = typeof (pinDataTristate) });
            pinList.Add("output1", new pin { name = "output1", description = "input is false", direction = pinDirection.output, type = typeof (pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            tristate input1 = (tristate)pinStates["input1"].getData();
            tristate newOutput;

            switch (input1)
            {
                case tristate.noValue:
                    newOutput = tristate.noValue;
                    break;
                case tristate.yes:
                    newOutput = tristate.no;
                    break;
                case tristate.no:
                    newOutput = tristate.yes;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // only set the output if necessary! constantly setting the output will result in a stack overflow.
                if ((tristate)pinStates["output1"].getData() != newOutput)
                    pinStates["output1"].setData(newOutput);
        }

        public ruleItem_not() {}
    }
}
