// -----------------------------------------------------------------------
// <copyright file="ruleItemPingDataType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine.ruleItems
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using ruleEngine.pinDataTypes;
    using ruleEngine.ruleItems.windows;

    public class ObjectConverter : TypeConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value,
            Type destinationType)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PingDataType));
            XmlReader reader = new XmlTextReader(new StringReader(value as string));
            if (!serializer.CanDeserialize(reader))
                throw new NotSupportedException();
            IObjectDataType toRet = (IObjectDataType)serializer.Deserialize(reader);
            if (destinationType == typeof(IObjectDataType))
                return toRet;
            if (destinationType == typeof(string))
                return toRet.ToString();
            if (destinationType == typeof(bool))
                return toRet.asBoolean();
            if (destinationType == typeof(tristate))
                return toRet.asBoolean() ? tristate.yes : tristate.no;
            throw new NotSupportedException();
        }

    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PingDataType : IObjectDataType 
    {
        public long TimeTaken { get; set; }

        public string IPAddress { get; set; }

        public string DomainName { get; set; }

        public bool PingRecived { get; set; }

        public void setToDefault()
        {
            PingRecived = false;
        }

        public override string ToString()
        {
            if (PingRecived)
            {
                return string.Format("Ping recived from {0} {1}",
                    DomainName ?? (IPAddress ?? ""), TimeTaken + "ms");
            }
            return string.Format("No ping recived from {0} {1}",
                this.DomainName ?? "", this.IPAddress ?? "");
        }
        public bool asBoolean()
        {
            return PingRecived;
        }

        public string serialize()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter(new StringBuilder());
            serializer.Serialize(writer,this);
            return writer.GetStringBuilder().ToString();
        }

        public string TypeName { get; set ; }

        public string AssemblyName { get;set;}
    }
}
