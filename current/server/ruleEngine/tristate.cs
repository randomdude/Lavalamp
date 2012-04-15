using System;
using System.ComponentModel;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems.windows
{

    public class tristateConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            Type sourceType = value.GetType();

            if (sourceType == typeof(tristate))
                return value;
            if (sourceType == typeof(string))
                return Enum.Parse(typeof (tristate) , (string) value);
            if (sourceType == typeof(bool))
            {
                if ((bool)value)
                    return tristate.yes;
                return tristate.no;
            }
            if (sourceType == typeof(INumber))
            {
                 return (tristate)((INumber) value).getAs<int>();
            }
            throw new NotSupportedException();
        }
        
    }

    [TypeConverter(typeof(tristateConverter))]
    public enum tristate
    {
        noValue, yes, no
    }

    
}