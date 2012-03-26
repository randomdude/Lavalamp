using System;
using System.Drawing;
using Microsoft.Scripting;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{
    public class Number
    {
        private double? _floatVal;
        private long? _longVal;
        private Type _actualType;

        public Type actualType { get { return _actualType; } }

        public Number(float number)
        {
            _floatVal = number;
            _actualType = typeof (float);
        }
        public Number(double number)
        {
            _floatVal = number;
            _actualType = typeof(double);
        }
        public Number(short number)
        {
            _longVal = number;
            _actualType = typeof(short);
        }
        public Number(int number)
        {
            _longVal = number;
            _actualType = typeof(int);
        }
        public Number(long number)
        {
            _longVal = number;
            _actualType = typeof(long);
        }
        public static implicit operator Number(double num)  
        {
            return new Number(num); 
        }
        public static implicit operator Number(float num)  
        {
            return new Number(num); 
        }
        public static implicit operator Number(int num)
        {
            return new Number(num);
        }
        public static implicit operator Number(short num)
        {
            return new Number(num);
        }

        public static implicit operator Number(long num)
        {
            return new Number(num);
        }

        public static implicit operator double(Number num)
        {
            if (!num._floatVal.HasValue)
                throw new InvalidCastException("Cannot cast a whole number to a floating point number");
            return num._floatVal.Value; // implicit conversion
        }
        public static implicit operator float(Number num)
        {
            if (!num._floatVal.HasValue)
                throw new InvalidCastException("Cannot cast a whole number to a floating point number");
            if (num._actualType == typeof(double) && (num._floatVal > float.MaxValue || num._floatVal < float.MinValue))
                throw new InvalidCastException("Number is too large to fit in a float");

            return (float)num._floatVal.Value; // implicit conversion
        }
        public static implicit operator int(Number num)
        {
            if (!num._longVal.HasValue)
                throw new InvalidCastException("Cannot cast a floating point number to a whole number");

            if (num._actualType == typeof(long) && (num._longVal > int.MaxValue || num._longVal < int.MinValue))
                throw new InvalidCastException("Number is too large to fit in a float");

            return (int)num._longVal.Value; // implicit conversion
        }
        public static implicit operator short(Number num)
        {
            if (!num._longVal.HasValue)
                throw new InvalidCastException("Cannot cast a floating point number to a whole number");

            if ((num._actualType == typeof(long) || num._actualType == typeof(int)) && (num._longVal > short.MaxValue || num._longVal < short.MinValue))
                throw new InvalidCastException("Number is too large to fit in a float");

            return (short)num._longVal.Value; // implicit conversion
        }
        public static implicit operator long(Number num)
        {
            if (!num._longVal.HasValue)
                throw new InvalidCastException("Cannot cast a floating point number to a whole number");

            return num._longVal.Value; // implicit conversion
        }

        public static bool operator == (Number n,Number n2)
        {
            if (n == null && n2 == null)
                return true;
            if (n == null || n2 == null)
                return false;

            if (n._floatVal.HasValue && n2._floatVal.HasValue)
            {
                return Math.Abs(n._floatVal.Value - n2._floatVal.Value) < double.Epsilon;
            }
            if (n._longVal.HasValue && n2._longVal.HasValue)
            {
                return n._longVal == n2._longVal;
            }
            return false;
        }

        public static bool operator !=(Number n , Number n2)
        {
            return !(n == n2);
        }

        public override string ToString()
        {
            if (_longVal.HasValue)
                return _longVal.ToString();
            if (_floatVal.HasValue)
                return _floatVal.ToString();

            return "0";
        }

        public bool Equals(Number other)
        {
            if (ReferenceEquals(null , other)) return false;
            if (ReferenceEquals(this , other)) return true;
            return other._floatVal.Equals(_floatVal) && other._longVal.Equals(_longVal) && Equals(other._actualType , _actualType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (Number)) return false;
            return Equals((Number) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_floatVal.HasValue ? _floatVal.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_longVal.HasValue ? _longVal.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_actualType != null ? _actualType.GetHashCode() : 0);
                return result;
            }
        }
    }
    public class pinDataNumber : pinDataBase<Number>
    {
        public pinDataNumber(pinDataBase<Number> cpy) : base(cpy) { }
        public pinDataNumber(ruleItemBase newParentRuleItem, pin newParentPin) : base(new Number(0L),newParentRuleItem,newParentPin) {}
        public pinDataNumber(Number defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(defaultVal, newParentRuleItem, newParentPin) { }
       
        public override void setToDefault()
        {
            _data = new Number(0L);
        }

        public override string ToString()
        {
            return _data.ToString();
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
            return typeof(Number);
        }

        public override object noValue
        {
            get { return 0; }
        }

        protected override Number convertData(object value)
        {
            Number convertedType = new Number(0L);
            Type valType = value.GetType();
            if (valType == typeof(Number))
                convertedType = (Number)value;
            else if (valType == typeof(int))
                convertedType = (int)value;
            else if (valType == typeof(long))
                convertedType = (long)value;
            else if (valType == typeof(short))
                convertedType = (short)value;
            else if (valType == typeof(float))
                convertedType = (float)value;
            else if (valType == typeof(double))
                convertedType = (double)value;
            else if (valType == typeof(string))
            {
                string convVal = (string)value;
                if (convVal.Contains("."))
                {
                    double num;
                    if (!double.TryParse(convVal, out num))
                        throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + valType + " and Number");

                    convertedType = num;
                }
                else
                {
                    long num;
                    if (!long.TryParse(convVal, out num))
                        throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + valType + " and Number");
                    convertedType = num;
                }

            }
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
                        convertedType = (int)noValue;
                        break;
                }
            }
            else
                throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + valType + " and Number");

            return convertedType;
        }
    }
}
