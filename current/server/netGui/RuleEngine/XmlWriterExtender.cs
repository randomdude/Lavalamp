using System;
using System.Collections.Generic;
using System.Xml;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public static class XmlWriterWriterExtender
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
    }

}
