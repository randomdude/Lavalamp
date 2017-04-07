using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ruleEngine.ruleItems;
using netGui.Properties;
using ruleEngine.ruleItems.Basic_logic;

namespace netGui.RuleEngine
{
    public partial class ctlSwitchWidget : ctlRuleItemWidget
    {
        public ctlSwitchWidget(ruleItemBase ruleItem) :base(ruleItem)
        {
            ruleItem.newTimelineEvent += changeBackgroundImage;
            backgroundImg.Image = Resources.switch_false;

        }

        private void changeBackgroundImage(ruleItemBase sender, timelineEventArgs e)
        {
            var switchEvent = e as switchTimeLineEventArgs;
            
            if (switchEvent?.switchedValue == ruleEngine.ruleItems.windows.tristate.yes)
            {
                backgroundImg.Image = Resources.switch_true;
            }
            else {
                backgroundImg.Image = Resources.switch_false;

            }

        }
    }
}
