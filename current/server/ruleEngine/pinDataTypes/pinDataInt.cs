using System;
using System.Drawing;
using System.Globalization;
using Microsoft.Scripting;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{
    public class pinDataInt : pinDataBase<int>
    {
        public pinDataInt(pinDataBase<int> cpy) : base(cpy) {}
        public pinDataInt(ruleItemBase newParentRuleItem, pin newParentPin) : base(0,newParentRuleItem,newParentPin) {}
        public pinDataInt(int defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(defaultVal, newParentRuleItem, newParentPin) { }
        
        public override void setToDefault()
        {
            _data = 0;
        }

        public override string ToString()
        {
            return _data.ToString(CultureInfo.InvariantCulture);
        }

        public override Color getColour()
        {
            return Color.Transparent;
        }

        public override bool asBoolean()
        {
            return _data != 0;
        }

        public override IPinData not()
        {
            throw new NotImplementedException();
        }

        public override Type getDataType()
        {
            return typeof (int);
        }

        public override object noValue
        {
            get { return int.MinValue; }
        }

        protected override int convertData(object value)
        {
            int convertedType = 0;
            Type valType = value.GetType();
            if (valType == typeof(string))
                convertedType = int.Parse((string) value);
            else if (valType == typeof(bool))
            {
                if ((bool)value)
                    convertedType = 1;
                else
                    convertedType = 0;
            }
            else if (valType == typeof(tristate))
            {
                switch ((tristate)value)
                {
                    case tristate.yes:
                        convertedType = 1;
                        break;
                    case tristate.no:
                        convertedType = 0;
                        break;
                    case tristate.noValue:
                        convertedType = (int) noValue;
                        break;
                }
            }
            else
                throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + valType + " and int");

            return convertedType;
        }
    }
}
