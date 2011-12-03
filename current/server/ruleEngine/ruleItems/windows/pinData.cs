using System;
using System.Drawing;
using ruleEngine;

namespace ruleEngine.ruleItems.windows
{
    public abstract class pinData<T> : IpinData
    {
        protected T _data;
        protected readonly ruleItemBase _parentRuleItem;
        protected readonly pin _parentPin;

        protected pinData(pinData<T> cpy)
        {
            _parentRuleItem = cpy._parentRuleItem;
            _parentPin = cpy._parentPin;
            _data = cpy._data;
        }

        protected pinData(T defaultVal,ruleItemBase newParentRuleItem, pin newParentPin)
        {
            _data = defaultVal;
            _parentRuleItem = newParentRuleItem;
            _parentPin = newParentPin;
        }
 
        public abstract void setToDefault();
        public new abstract string ToString();
        public abstract Color getColour();
        public abstract bool asBoolean();
        public abstract IpinData not();
        public abstract Type getDataType();
        public abstract object noValue { get; }

        public object data 
        { 
            get { return _data; }
            set
            {
                // only set the data when neccessary
                if (_data.Equals(value))
                    return;
                _data = convertData(value);
                reevaluate();
            }
        }
        /// <summary>
        /// this function returns the entered data as represented as the underlining datatype (T)
        /// if possible, if not it will throw an exception. 
        /// note currently data maybe lost if converting down a type i.e string -> boolean -> string the string would be lost in this conversion
        /// </summary>
        /// <param name="value">the raw data</param>
        /// <returns>converted data</returns>
        protected abstract T convertData(object value);

        public IpinData and(IpinData pinData)
        {
            return asBoolean() && pinData.asBoolean() ? this : (new pinDataBool(false,_parentRuleItem,_parentPin) as IpinData);
        }

        public IpinData or(IpinData pinData)
        {
            return asBoolean() || pinData.asBoolean() ? this : (new pinDataBool(false, _parentRuleItem, _parentPin) as IpinData);
        }

        public IpinData xor(IpinData pinData)
        {
            return asBoolean() ^ pinData.asBoolean() ? this : (new pinDataBool(false, _parentRuleItem, _parentPin) as IpinData); ;
        }

        protected void reevaluate()
        {
            // evaluate this ruleItem only if the pin we changed was an input pin
            if (_parentPin.direction == pinDirection.input)
            {
                try
                {
                    _parentRuleItem.evaluate();
                }
                catch (Exception e)
                {
                    _parentRuleItem.errorHandler(e);
                }
            }

            if (_parentPin.direction == pinDirection.output)
            {
                // If it was an output pin, or if we don't know what direction it was, we propagate the change out of the pin.
                // pass new pins to next ruleItem
                _parentPin.invokeChangeHandlers();
            }

            _parentPin.updateUI();
        }
    }
}