﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Basic logic")]
    public class ruleItem_debug : ruleItemBase 
    {
        private PictureBox indicator;
        private bool lastState;

        public override string ruleName() { return "Debug indicator"; }

        public override string caption()
        {
            return "Debug";
        }

        public override IFormOptions setupOptions()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "to monitor", direction = pinDirection.input, valueType = typeof(pinDataTristate) });
            pinList.Add("output", new pin { name = "output", description = "input is true", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }

        public override void evaluate()
        {
            if (pinInfo["input"].value.asBoolean() != lastState)
            {
                if (!pinInfo["input"].value.asBoolean())
                    indicator.Image = Properties.Resources._0;
                else
                    indicator.Image = Properties.Resources._1;

                indicator.Invalidate();

                lastState = pinInfo["input"].value.asBoolean();
            }

            if (pinInfo["output"].value.data != pinInfo["input"].value.data)
            {
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTristate((tristate)pinInfo["input"].value.data, this, pinInfo["output"])));
            }
        }


        public ruleItem_debug()
        {
            Size currentPreferredSize = preferredSize();

            indicator = new PictureBox();
            indicator.Image = Properties.Resources._0;
            indicator.Size = indicator.Image.Size;
            indicator.Left = (currentPreferredSize.Width / 2) - (indicator.Width / 2);
            indicator.Top = (currentPreferredSize.Width / 2) - (indicator.Height / 2);
            indicator.Visible = true;

            controls.Add(indicator);

        }
    }
}
