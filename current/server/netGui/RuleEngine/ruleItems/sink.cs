using System;
using System.Collections.Generic;

namespace netGui.RuleEngine.ruleItems
{
    // only used during dev/debug. Real people don't want something this useless.
    //[RuleEngine.ToolboxRule]
    class sink : ruleItemBase 
    {
        public override string ruleName() { return "Some test sink"; }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { direction = pinDirection.input });

            return pinList;
        }

        public sink()
        {
        }

        public override void evaluate()
        {
        }
    }
}
