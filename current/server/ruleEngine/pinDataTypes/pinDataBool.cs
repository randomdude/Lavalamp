using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using Microsoft.Scripting;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{
    public class pinDataBool : pinDataBase<bool>
    {
        public pinDataBool(ruleItemBase parentRuleItem, pin newParentPin) : base(false, parentRuleItem, newParentPin) {}
        public pinDataBool(bool initalVal, ruleItemBase parentRuleItem, pin newParentPin) : base(initalVal,parentRuleItem, newParentPin) { }
       
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="pinDataBase"></param>
        public pinDataBool(pinDataBool pinDataBase) : base(pinDataBase) { }

        public override void setToDefault()
        {
            data = noValue;
            if (_parentRuleItem.isEnabled)
                reevaluate();
        }

        public override String ToString()
        {
            return data.ToString();
        }

        public override Color getColour()
        {
            return _data ? Color.Green : Color.Transparent;
        }

        public override bool asBoolean()
        {
            return _data;
        }

        public override IPinData not()
        {
            return new pinDataBool(this) {data = !_data};
        }

        public override Type getDataType()
        {
            return typeof(bool);
        }

        public override object noValue
        {
            get { return false; }
        }

        [Pure]
        protected override bool convertData(object value)
        {
            bool convertedData = false;
            if (value == null)
                return false;
            Type tpyToConvert = value.GetType();
            if (tpyToConvert == typeof(bool))
                convertedData = (bool)value;
            else if (tpyToConvert == typeof(string))
                convertedData = !String.IsNullOrEmpty(value as string);
            else if (tpyToConvert == typeof(tristate))
            {
                if (((tristate)value) == tristate.yes)
                    convertedData = true;
            }
            else
                throw new ArgumentTypeException("Invaild Pin types! No conversion exists between " + tpyToConvert + " and bool");

            return convertedData;

        }
    }
}