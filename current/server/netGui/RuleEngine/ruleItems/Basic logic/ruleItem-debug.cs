using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using netGui.Properties;

namespace netGui.RuleEngine.ruleItems
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    class ruleItem_debug : ruleItemBase 
    {
        private PictureBox indicator;
        private bool lastState = false;

        public override string ruleName() { return "Debug indicator"; }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "to monitor", direction = pinDirection.input });
            pinList.Add("output", new pin { name = "output", description = "input is true", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
            if ((bool)pinStates["input"] != lastState)
            {
                if ((bool) pinStates["input"] == true)
                    indicator.Image = netGui.Properties.Resources._1;
                else
                    indicator.Image = netGui.Properties.Resources._0;

                indicator.Invalidate();

                lastState = (bool) pinStates["input"];
            }

            if (pinStates["output"] != pinStates["input"])
                pinStates["output"] = pinStates["input"];
        }


        public ruleItem_debug()
        {
            indicator = new PictureBox();
            indicator.Image = netGui.Properties.Resources._0;
            indicator.Size = indicator.Image.Size;
            indicator.Left = (preferredSize.Width / 2) - (indicator.Width / 2);
            indicator.Top  = (preferredSize.Width  / 2) - (indicator.Height / 2);
            indicator.Visible = true;

            controls.Add(indicator);

            this.pinStates.evaluate = new evaluateDelegate(evaluate);
            pinStates.setErrorHandler(new errorDelegate(base.errorHandler));
        }
    }
}
