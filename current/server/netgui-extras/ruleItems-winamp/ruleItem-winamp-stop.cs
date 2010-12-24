using System.Drawing;
using ruleEngine;

namespace ruleItems_winamp
{
    [ToolboxRule]
    [ToolboxRuleCategory("Winamp")]
    public class ruleItem_winamp_stop : ruleItem_winamp_base
    {
        public override char[] Cmd
        {
            get { return new char[] { (char)0x04 }; }
        }
        public override string ruleName() { return "Stop current song"; }
        public override Image background() { return Properties.Resources.winamp_stop;   }
    }
}