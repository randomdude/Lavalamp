using System.Drawing;
using System.IO.Pipes;
using netGui.RuleEngine;
using netGui.RuleEngine.ruleItems;

namespace ruleItems_winamp
{
    [ToolboxRule]
    [ToolboxRuleCategory("Winamp")]
    public class ruleItem_winamp_next : ruleItem_winamp_base
    {
        public override char[] Cmd
        {
            get { return new char[] { (char)0x02 }; }
        }
        public override string ruleName() { return "Playlist: Next song"; }
        public override Image background() { return Properties.Resources.winamp_next;   }
    }
}