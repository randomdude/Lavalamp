using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public static class xmlWriterExtender
    {
        public static void WriteElementLineChainDictionary(this XmlWriter writer, String newElementName, Dictionary<String, lineChain> toSerialise)
        {
            writer.WriteStartElement(newElementName);

            foreach(string thisKey in toSerialise.Keys)
            {
                writer.WriteStartElement(newElementName);
                writer.WriteElementLineChain(thisKey, toSerialise[thisKey]);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();        
        }

        public static void WriteElementRuleItemDictionary(this XmlWriter writer, String newElementName, Dictionary<String, ruleItemBase> toSerialise)
        {
            writer.WriteStartElement(newElementName);

            foreach (string thisKey in toSerialise.Keys)
            {
                if (toSerialise[thisKey].isDeleted)
                    continue;
                writer.WriteStartElement("ruleItem");
                writer.WriteAttributeString("serial", toSerialise[thisKey].serial.ToString() );
                writer.WriteElementString("X", toSerialise[thisKey].location.X.ToString());
                writer.WriteElementString("Y", toSerialise[thisKey].location.Y.ToString());
                writer.WriteElementString("type", toSerialise[thisKey].GetType().ToString());
                writer.WriteEndElement();
                toSerialise[thisKey].WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        public static void WriteElementPinDictionary(this XmlWriter writer, String newElementName, Dictionary<String, pin> toSerialise)
        {
            writer.WriteStartElement(newElementName);

            foreach (string thisKey in toSerialise.Keys)
            {
                writer.WriteStartElement("pin");
                writer.WriteElementString("serial", toSerialise[thisKey].serial.id.ToString());
                writer.WriteElementString("direction", toSerialise[thisKey].direction.ToString());
                writer.WriteElementString("description", toSerialise[thisKey].description);
                writer.WriteElementString("linkedTo", toSerialise[thisKey].linkedTo.id.ToString());
                writer.WriteElementString("name", toSerialise[thisKey].name);
                writer.WriteElementString("parentLineChain", toSerialise[thisKey].parentLineChain.id.ToString());
                writer.WriteElementString("parentRuleItem", toSerialise[thisKey].parentRuleItem.id.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        public static void WriteElementLineChain(this XmlWriter writer, String localname, lineChain value)
        {
            writer.WriteStartElement("lineChain");
            value.WriteXml(writer);
            writer.WriteEndElement();
        }

        // These dead simple structs are just serialsed here. 
        public static void WriteColor(this XmlWriter writer, string name, Color writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteElementString("R", writeThis.R.ToString());
            writer.WriteElementString("G", writeThis.G.ToString());
            writer.WriteElementString("B", writeThis.B.ToString());
            writer.WriteEndElement();
        }

        public static void WritePoint(this XmlWriter writer, String name, Point writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteElementString("X", writeThis.X.ToString());
            writer.WriteElementString("Y", writeThis.Y.ToString());
            writer.WriteEndElement();
        }

        public static void WriteBool(this XmlWriter writer, string name, bool writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteAttributeString("value", writeThis.ToString());
            writer.WriteEndElement();
        }

        public static void WriteGuid(this XmlWriter writer, string name, Guid newSerial)
        {
            writer.WriteElementString(name, newSerial.ToString());
        }

    }

}
