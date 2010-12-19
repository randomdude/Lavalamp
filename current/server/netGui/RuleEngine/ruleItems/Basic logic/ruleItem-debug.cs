using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using netGui.Properties;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine.ruleItems
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    class ruleItem_debug : ruleItemBase 
    {
        private PictureBox indicator;
        private string lastState = string.Empty;

        public override string ruleName() { return "Debug indicator"; }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "to monitor", direction = pinDirection.input, type = typeof(pinDataTristate) });
            pinList.Add("output", new pin { name = "output", description = "input is true", direction = pinDirection.output, type = typeof(pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            if (pinStates["input"].ToString() != lastState)
            {
                if ( (pinStates["input"].ToString() == "False") ||
                     (pinStates["input"].ToString() == "no") )
                    indicator.Image = netGui.Properties.Resources._0;
                else
                    indicator.Image = netGui.Properties.Resources._1;

                indicator.Invalidate();

                lastState = pinStates["input"].ToString();
            }

            if (pinStates["output"].ToString() != pinStates["input"].ToString())
                pinStates["output"] = pinStates["input"];
        }


        public ruleItem_debug()
        {
            Size currentPreferredSize = preferredSize();

            indicator = new PictureBox();
            indicator.Image = netGui.Properties.Resources._0;
            indicator.Size = indicator.Image.Size;
            indicator.Left = (currentPreferredSize.Width / 2) - (indicator.Width / 2);
            indicator.Top = (currentPreferredSize.Width / 2) - (indicator.Height / 2);
            indicator.Visible = true;

            controls.Add(indicator);

        }
    }
}
