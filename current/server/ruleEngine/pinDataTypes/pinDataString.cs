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
            return Color.Transparent;
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

        [Pure]
        protected override string convertData(object value)
        {
            
            if (value == null)
                return null;
            string convertedType = "";
            Type cvtType = value.GetType();
            if (cvtType == typeof(string) || cvtType == typeof(Number))
                convertedType = value.ToString();
            else if (cvtType == typeof(bool))
            {
                if ((bool)value)
                    convertedType = "yes";
                else
                    convertedType = "no";
            }
            else if (cvtType == typeof(tristate))
            {
                switch ((tristate)value)
                {
                        case tristate.yes:
                            convertedType = "yes";
                            break;
                        case tristate.no:
                            convertedType = "no";
                            break;
                        case tristate.noValue:
                            convertedType = "nothing";
                            break;
                }
            }
            else
                throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + cvtType + " and string");

            return convertedType;



        }
    }
}