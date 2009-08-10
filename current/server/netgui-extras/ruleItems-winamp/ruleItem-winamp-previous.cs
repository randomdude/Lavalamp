using System.Drawing;
using System.IO.Pipes;
using netGui.RuleEngine;
using netGui.RuleEngine.ruleItems;

namespace ruleItems_winamp
{
    [ToolboxRule]
    [ToolboxRuleCategory("Winamp")]
    public class ruleItem_winamp_previous : ruleItem_winamp_base
    {
        public override char[] Cmd
        {
            get { return new char[] { (char)0x01 }; }
        }
        public override string ruleName() { return "Playlist: Previous song"; }
        public override Image background() { return Properties.Resources.winamp_previous;   }
    }
}