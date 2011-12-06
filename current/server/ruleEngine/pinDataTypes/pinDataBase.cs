using System;
using System.Drawing;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{
    public abstract class pinDataBase<T> : IPinData
    {
        protected T _data;
        protected readonly ruleItemBase _parentRuleItem;
        private readonly pin _parentPin;

        protected pinDataBase(pinDataBase<T> cpy)
        {
            _parentRuleItem = cpy._parentRuleItem;
            _parentPin = cpy._parentPin;
            _data = cpy._data;
        }

        protected pinDataBase(T defaultVal, ruleItemBase newParentRuleItem, pin newParentPin)
        {
            _data = defaultVal;
            _parentRuleItem = newParentRuleItem;
            _parentPin = newParentPin;
        }
 
        public abstract void setToDefault();
        public new abstract string ToString();
        public abstract Color getColour();
        public abstract bool asBoolean();
        public abstract IPinData not();
        public abstract Type getDataType();
        public abstract object noValue { get; }

        public object data
        {
            get 
            { 
                return _data; 
            }
            set
            {
                _data = convertData(value);
                reevaluate();
            }
        }

        /// <summary>
        /// this function returns the entered data as represented as the underlining datatype (T)
        /// if possible, if not it will throw an exception. 
        /// </summary>
        /// <param name="value">the raw data</param>
        /// <returns>converted data</returns>
        protected abstract T convertData(object value);

        public void performUpdate()
        {
            _parentPin.value.data = this.data;
        }

        public IPinData and(IPinData toCompareTo)
        {
            bool result = asBoolean() && toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin) as IPinData;
        }

        public IPinData or(IPinData toCompareTo)
        {
            bool result = asBoolean() || toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin) as IPinData;
        }

        public IPinData xor(IPinData toCompareTo)
        {
            bool result = asBoolean() ^ toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin) as IPinData;
        }

        public void reevaluate()
        {
            // We propagate the change out of the pin. 
            _parentPin.invokeChangeHandlers();

            // and update our image icon
            _parentPin.updateUI();
        }
    }
}