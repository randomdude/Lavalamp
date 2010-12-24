using System;
using System.Drawing;
using ruleEngine;

namespace ruleEngine.ruleItems.windows
{
    public class pinDataBool : pinData
    {
        private bool data;

        public pinDataBool(ruleItemBase parentRuleItem, pin newParentPin) : base(parentRuleItem, newParentPin) {}

        public override void setToDefault()
        {
            data = false;
        }

        public override object getData()
        {
            return data;
        }

        public override void setData(Object newData)
        {
            if (data == (bool)newData)
                return;

            data = (bool)newData;
            base.reevaluate();
        }

        public override String ToString()
        {
            return data.ToString();
        }

        public override Color getColour()
        {
            if (data)
                return Color.Green;
            else
                return Color.Transparent;
        }
    }
}