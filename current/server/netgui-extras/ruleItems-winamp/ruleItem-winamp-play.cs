﻿using System.Drawing;
using ruleEngine;

namespace ruleItems_winamp
{
    [ToolboxRule]
    [ToolboxRuleCategory("Winamp")]
    public class ruleItem_winamp_play : ruleItem_winamp_base
    {
        public override char[] Cmd
        {
            get { return new char[] { (char)0x05 }; }
        }
        public override string ruleName() { return "Play current song"; }
        public override Image background() { return Properties.Resources.winamp_play; }
    }
}