using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using ruleEngine.ruleItems;
using System.ComponentModel;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.pinDataTypes
{

    [TypeConverter(typeof(GenericNumberConverter))]
    public interface INumber
    {
        Type actualType { get; }
        TX getAs<TX>();
    }

    public class GenericNumberConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            Type sourceType = value.GetType();

            if (sourceType.GetInterface("INumber") == destinationType)
                return value;
            if (sourceType == typeof(string))
            {
                long lresult;
                if (long.TryParse(value.ToString(), NumberStyles.Integer, null, out lresult))
                    return new GenericNumber<long>(lresult);
                double dresult;
                if (double.TryParse(value.ToString(), out dresult))
                    return new GenericNumber<double>(dresult);
                throw new InvalidCastException();
            }
            if (sourceType == typeof(bool))
            {
                if ((bool) value)
                        return new GenericNumber<short>(1);
                else
                    return new GenericNumber<short>(0);
            }
            if (sourceType == typeof(tristate))
            {
                switch ((tristate)value)
                {
                        case tristate.yes:
                            return new GenericNumber<short>(1);
                            break;
                        case tristate.no:
                            return new GenericNumber<short>(0);
                            break;
                        case tristate.noValue:
                            return null;
                            break;
                }   
            }
            throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", sourceType, destinationType));
        }
    }

    public class GenericNumber<T> : INumber where T : struct, IComparable 
    {
        private T _value;

        [Pure]
        public Type actualType
        {
            get
            {
                return _value.GetType(); 
            } 
        }

        public TX getAs<TX>()
        {
             var converter = TypeDescriptor.GetConverter(typeof(T));
             //     if (!converter.CanConvertFrom(value.GetType()))
             //       throw new ArgumentTypeException("Invalid Pin types! No conversion exists between " + value.GetType() + " and " + typeof(T));

             return (TX)(converter.ConvertTo(_value, typeof(TX)));
            
        }

        public GenericNumber(T num)
        {
            _value = num;
        }

        public static implicit operator GenericNumber<T>(T num)  
        {
            return new GenericNumber<T>(num); 
        }


        public static implicit operator T(GenericNumber<T> num)
        {
            return num._value; // implicit conversion
        }

        public static bool operator ==(GenericNumber<T> n, GenericNumber<T> n2)
        {
            if (n == n2)
                return true;
            if (n == null || n2 == null)
                return false;

            if (n.actualType != n2.actualType)
                return false;

           return Comparer<T>.Default.Compare(n._value , n2._value) == 0;
        }
            
        public static bool operator !=(GenericNumber<T> n, GenericNumber<T> n2)
        {
            return !(n == n2);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public bool Equals(GenericNumber<T> other)
        {
            return !ReferenceEquals(null , other) && other._value.Equals(_value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            return obj.GetType() == typeof(GenericNumber<T>) && Equals((GenericNumber<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _value.GetHashCode();
                result = (result * 397) ^ (actualType != null ? actualType.GetHashCode() : 0);
                return result;
            }
        }
    }
    public class pinDataNumber : pinDataBase<INumber>
    {
        public pinDataNumber(pinDataBase<INumber> cpy) : base(cpy) { }
        public pinDataNumber(ruleItemBase newParentRuleItem , pin newParentPin) : base(new GenericNumber<long>(0L),newParentRuleItem,newParentPin) {}
        #region "Type Specific Constructors"
        public pinDataNumber(int defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<int>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(float defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<float>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(long defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<long>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(decimal defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<decimal>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(INumber defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(defaultVal, newParentRuleItem, newParentPin) { }
        public pinDataNumber(double defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<double>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(ulong defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<ulong>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(uint defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<uint>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(short defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<short>(defaultVal), newParentRuleItem, newParentPin) { }
        public pinDataNumber(ushort defaultVal, ruleItemBase newParentRuleItem, pin newParentPin) : base(new GenericNumber<ushort>(defaultVal), newParentRuleItem, newParentPin) { }
        #endregion
        public override void setToDefault()
        {
            _data = null;
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
            return _data != null;
        }

        public override IPinData not()
        {
            throw new NotImplementedException();
        }

        public override Type getDataType()
        {
            return _data.GetType();
        }

        public override object noValue
        {
            get { return 0; }
        }

    }
}
