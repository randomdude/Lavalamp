using ruleEngine.ruleItems.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.ruleItems.Basic_logic
{
    public class switchTimeLineEventArgs : timelineEventArgs
    {
        public switchTimeLineEventArgs(tristate switchState, IPinData pinData) : base(pinData)
        {
            switchedValue = switchState;
        }

        public tristate switchedValue
        {
            get; private set;
        }
    }
}