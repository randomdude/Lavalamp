using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;

using ruleEngine.ruleItems;

namespace ruleEngine.pinDataTypes
{
    public abstract class pinDataBase<T> : IPinData where T : IComparable
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

        public bool hasChanged
        {
            get; private set; }
        public abstract IPinData not();
        /// <summary>
        /// Gets the underlining data type, the type parameter to the class
        /// </summary>
        /// <returns>the underlining data type</returns>
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
                T newVal = convertData(value);
                hasChanged = newVal.CompareTo(_data) != 0;
                _data = newVal;
                reevaluate();
            }
        }

        /// <summary>
        /// this function returns the entered data as represented as the underlining datatype (T)
        /// if possible, if not it will throw an exception. 
        /// </summary>
        /// <param name="value">the raw data</param>
        /// <returns>converted data</returns>
        [Pure]
        protected T convertData(object value) 
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (value.GetType() == typeof(T))
                return (T)value;
       //     if (!converter.CanConvertFrom(value.GetType()))
         //       throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + value.GetType() + " and " + typeof(T));

            return (T)(converter.ConvertTo(value, typeof(T)));
        }

        public void performUpdate()
        {
            _parentPin.value.data = this.data;
        }
        
        public IPinData and(IPinData toCompareTo)
        {
            bool result = asBoolean() && toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin);
        }

        public IPinData or(IPinData toCompareTo)
        {
            bool result = asBoolean() || toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin);
        }

        public IPinData xor(IPinData toCompareTo)
        {
            bool result = asBoolean() ^ toCompareTo.asBoolean();
            return new pinDataBool(result, _parentRuleItem, _parentPin);
        }
        /// <summary>
        /// called when we want to evaluate any changes to the data
        /// </summary>
        public void reevaluate()
        {
            // We propagate the change out of the pin. 
            _parentPin.invokeChangeHandlers();

            // and update our image icon
            _parentPin.updateUI();
        }
    }
}