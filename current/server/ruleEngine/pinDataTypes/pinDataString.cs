using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using Microsoft.Scripting;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{
    public class pinDataString : pinDataBase<string>
    {
        public pinDataString(ruleItemBase newParentRuleItem, pin newParentPin) : base("", newParentRuleItem, newParentPin) { }
        public pinDataString(string defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(defaultVal, newParentRuleItem, newParentPin) {}
        public pinDataString(IPinData cpy) : base((pinDataBase<string>) cpy) {}

        public override void setToDefault()
        {
            _data = "";
        }

        public override string ToString()
        {
            return _data;
        }

        public override Color getColour()
        {
            return hasChanged ? Color.Green : Color.Transparent;
        }

        public override bool asBoolean()
        {
            return !String.IsNullOrEmpty(_data);
        }

        public override IPinData not()
        {
            throw new NotImplementedException();
        }

        public override Type getDataType()
        {
            return typeof (string);
        }

        public override object noValue
        {
            get { return null; }
        }
    }
}