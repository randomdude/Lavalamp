using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ruleEngine
{
    partial class lineChain : IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            bool keepGoing = true;
            bool inhibitNextRead = false;

            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == "linechains" && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;


                if (xmlName == "id" && reader.NodeType == XmlNodeType.Element)
                {
                    serial = new lineChainGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "start" && reader.NodeType == XmlNodeType.Element)
                {
                    start = reader.ReadContentAsPoint();
                    inhibitNextRead = true;
                }
                if (xmlName == "end" && reader.NodeType == XmlNodeType.Element)
                {
                    end = reader.ReadContentAsPoint();
                    inhibitNextRead = true;
                }
                if (xmlName == "col" && reader.NodeType == XmlNodeType.Element)
                {
                    col = reader.ReadContentAsColor();
                    inhibitNextRead = true;
                }
                if (xmlName == "points" && reader.NodeType == XmlNodeType.Element)
                {
                    midPoints = reader.ReadContentAsPointsCollection();
                }
                if (xmlName == "deleted" && reader.NodeType == XmlNodeType.Element)
                {
                    deleted = bool.Parse(reader.GetAttribute("value"));
                }
                if (xmlName == "isdrawnbackwards" && reader.NodeType == XmlNodeType.Element)
                {
                    isdrawnbackwards = bool.Parse(reader.GetAttribute("value"));
                }
                if (xmlName == "sourcepin" && reader.NodeType == XmlNodeType.Element)
                {
                    sourcePin = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "destpin" && reader.NodeType == XmlNodeType.Element)
                {
                    destPin = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteGuid("id", serial.id);
            writer.WriteGuid("destPin", destPin.id);
            writer.WriteGuid("sourcePin", sourcePin.id);
            writer.WritePoint("start", start);
            writer.WritePoint("end", end);
            writer.WriteColor("col", col);
            writer.WriteBool("deleted", deleted);
            writer.WriteBool("isdrawnbackwards", isdrawnbackwards);
            writer.WriteStartElement("points");
            foreach (Point thisPoint in midPoints)
                writer.WritePoint("point", thisPoint);
            writer.WriteEndElement();
        }
    }
}
