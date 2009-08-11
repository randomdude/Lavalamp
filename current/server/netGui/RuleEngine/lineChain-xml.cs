using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace netGui.RuleEngine
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
                    start = readPoint(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "end" && reader.NodeType == XmlNodeType.Element)
                {
                    end = readPoint(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "col" && reader.NodeType == XmlNodeType.Element)
                {
                    col = readColour(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "points" && reader.NodeType == XmlNodeType.Element)
                {
                    points = readPointsCollection(ref reader);
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
            writeGuid(writer, "id", serial.id);
            writeGuid(writer, "destPin", destPin.id);
            writeGuid(writer, "sourcePin", sourcePin.id);
            writePoint(writer, "start", start);
            writePoint(writer, "end", end);
            writeColour(writer, "col", col);
            writeBool(writer, "deleted", deleted);
            writeBool(writer, "isdrawnbackwards", isdrawnbackwards);
            writer.WriteStartElement("points");
            foreach (Point thisPoint in points)
                writePoint(writer, "point", thisPoint);
            writer.WriteEndElement();
        }
    }
}
