using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ruleEngine.ruleItems;
using netGui.Properties;

namespace netGui.RuleEngine
{
    public class ctlFileWidget : ctlRuleItemWidget
    {
        
        public ctlFileWidget(ruleItemBase newRuleItemBase) : base(newRuleItemBase)
        {
            backgroundImg.Image = Resources.ruleItem_text;
            backgroundImg.Width = 35;
            backgroundImg.Height = 35;
            backgroundImg.Location = new System.Drawing.Point(5,5);

           backgroundImg.Invalidate();
        }


    }
}
