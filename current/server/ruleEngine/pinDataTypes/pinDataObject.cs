// -----------------------------------------------------------------------
// <copyright file="pinDataObject.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine.pinDataTypes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using ruleEngine.ruleItems;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TypeConverter(typeof(ObjectConverter))]
    public class pinDataObject : pinDataBase<string>
    {
        private IObjectDataType _value;

        public pinDataObject(ruleItemBase newParentRuleItem, pin newParentPin)
            : base("", newParentRuleItem, newParentPin)
        {
        }

        public pinDataObject(pinDataBase<string> cpy)
            : base(cpy)
        {
            this._value = ((pinDataObject)cpy)._value;
        }

        public pinDataObject(IObjectDataType defaultVal, ruleItemBase newParentRuleItem, pin newParentPin)
            : base(defaultVal.serialize(), newParentRuleItem, newParentPin)
        {
            _value = defaultVal;
        }

        public override void setToDefault()
        {
            _value.setToDefault();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override Color getColour()
        {
            return _value == null ? Color.Red : _value.asBoolean() ? Color.Green : Color.Red;
        }

        public override bool asBoolean()
        {
            return _value.asBoolean();
        }

        public override IPinData not()
        {
            throw new NotImplementedException();
            //return new pinDataObject(this){_data = };
        }

        public override Type getDataType()
        {
            return _value.GetType();
        }

        public override object noValue
        {
            get
            {
                return "";
            }
        }

        public override object data
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.GetType().GetInterface("IObjectDataType") != null)
                {
                    _value = (IObjectDataType)value;
                    _data = value.ToString();
                    this.reevaluate();
                    return;
                }
                string newVal = convertData(value);
                XmlSerializer serializer = new XmlSerializer(typeof(PingDataType));
                XmlReader reader = new XmlTextReader(new StringReader(newVal));
                if (!serializer.CanDeserialize(reader))
                    throw new NotSupportedException();
                _value = (IObjectDataType)serializer.Deserialize(reader);
                _data = newVal;
                this.reevaluate();
            }
        }
}
}
