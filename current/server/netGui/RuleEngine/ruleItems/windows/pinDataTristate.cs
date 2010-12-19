using System;
using System.Drawing;

namespace netGui.RuleEngine.ruleItems.windows
{
    public class pinDataTristate : pinData
    {
        private tristate data;

        public pinDataTristate(ruleItemBase parentRuleItem, pin newParentPin) : base(parentRuleItem, newParentPin) {}

        public override void setToDefault()
        {
            data = tristate.noValue;
            base.reevaluate();
        }

        public override object getData()
        {
            return data;
        }

        public override void setData(Object newData)
        {
            data = (tristate)newData;
            base.reevaluate();
        }

        public override String ToString()
        {
            return data.ToString();
        }

        public override Color getColour()
        {
            switch(data)
            {
                case tristate.noValue:
                    return Color.Transparent;
                case tristate.yes:
                    return Color.Green;
                case tristate.no:
                    return Color.Red;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}