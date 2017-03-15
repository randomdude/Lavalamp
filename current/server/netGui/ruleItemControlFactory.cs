using netGui.RuleEngine;
using ruleEngine.ruleItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui
{
    internal class ruleItemControlFactory : formFactory<ctlRuleItemWidget, ctlRuleItemWidget, ruleItemBase>
    {
        public ruleItemControlFactory() : base("ctl", "Widget")
        {
        }

        protected override ctlRuleItemWidget DefaultForm(ruleItemBase arg)
        {
            return new ctlRuleItemWidget(arg);
        }
    }
}
