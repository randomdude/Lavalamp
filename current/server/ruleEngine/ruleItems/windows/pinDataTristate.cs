using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using Microsoft.Scripting;

namespace ruleEngine.ruleItems.windows
{
    public class pinDataTristate : pinData<tristate>
    {
        public pinDataTristate(ruleItemBase parentRuleItem, pin newParentPin) : base(tristate.noValue,parentRuleItem, newParentPin) {}
        public pinDataTristate(tristate initialVal, ruleItemBase parentRuleItem, pin newParentPin) : base(initialVal, parentRuleItem, newParentPin) { }

        private pinDataTristate(pinData<tristate> parentRuleItem) : base(parentRuleItem) {}

        public override void setToDefault()
        {
            _data = tristate.noValue;
            if (base._parentRuleItem.isEnabled)
                base.reevaluate();
        }

        public override String ToString()
        {
            return data.ToString();
        }

        public override Color getColour()
        {
            switch(_data)
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

        public override bool asBoolean()
        {
            return _data == tristate.yes;
        }

        public override IpinData not()
        {

           if (_data == tristate.yes)
               return new pinDataTristate(this) {_data = tristate.no};

           if (_data == tristate.no)
               return new pinDataTristate(this) {_data = tristate.yes};

            //we can't invert a state with no value
            return new pinDataTristate(this) {_data = tristate.noValue};
        }


        public override Type getDataType()
        {
            return typeof (tristate);
        }

        public override object noValue
        {
            get { return tristate.noValue; }
        }

        [Pure]
        protected override tristate convertData(object value)
        {
            if (value == null)
                return tristate.noValue;
            Type tpyConvert = value.GetType();
            tristate convertedValue;
            if (tpyConvert == typeof(tristate))
                convertedValue = (tristate) value;
            else if (tpyConvert == typeof(bool))
            {
                if ((bool)value)
                    convertedValue = tristate.yes;
                else
                    convertedValue = tristate.no;
            }
            else if (tpyConvert == typeof(string))
            {
                convertedValue = ((string)value) == "" ? tristate.no : tristate.yes;
            }
            else
                throw new ArgumentTypeException("Invaild Pin types! No conversion exists between " + tpyConvert + " and tristate");

            return convertedValue;

        }
    }
}