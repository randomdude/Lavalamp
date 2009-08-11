using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace netGui.RuleEngine
{
    public class pin : IXmlSerializable
    {
        public pinDirection direction;
        public String name;
        public PictureBox imageBox;
        public String description = "(unknown)";

        public pinGuid serial = new pinGuid() { id = Guid.NewGuid() };
        public pinGuid linkedTo = new pinGuid();
        public lineChainGuid parentLineChain = new lineChainGuid() ;
        public ruleItemGuid parentRuleItem = new ruleItemGuid() ;

        public bool isConnected
        {
            get 
            {
                if (linkedTo == null)
                    return false;
                if (linkedTo.id == Guid.Empty)
                    return false;

                return true;
            }
        }

        public void disconnect()
        {
            linkedTo.id = Guid.Empty;
            linkedTo.id = Guid.Empty;
        }

        public void connectTo(lineChainGuid newTargetChain, pinGuid newTargetPin)
        {
            parentLineChain.id = newTargetChain.id;
            linkedTo.id = newTargetPin.id;
        }


        #region XML serialisation
        public XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                // todo - read 'name' properly, and make sure that the pin is always deserialised in to the target pin of the same name

                if (xmlName == "serial" && reader.NodeType == XmlNodeType.Element)
                {
                    this.serial.id = new Guid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "linkedto" && reader.NodeType == XmlNodeType.Element)
                {
                    this.linkedTo.id = new Guid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "parentLineChain" && reader.NodeType == XmlNodeType.Element)
                {
                    this.parentLineChain.id = new Guid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "parentRuleItem" && reader.NodeType == XmlNodeType.Element)
                {
                    this.parentRuleItem.id = new Guid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("pin");
            writer.WriteAttributeString("name", name);
            writer.WriteElementString("serial", this.serial.ToString());
            writer.WriteElementString("linkedTo", this.linkedTo.id.ToString());
            writer.WriteElementString("parentLineChain", this.parentLineChain.id.ToString());
            writer.WriteElementString("parentRuleItem", this.parentRuleItem.id.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }

    public class ruleItemGuid 
    {
        public Guid id = Guid.Empty;

        public ruleItemGuid() {}

        public ruleItemGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }

    public class pinGuid 
    {
        public Guid id = Guid.Empty;

        public pinGuid() {}

        public pinGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }

    public class lineChainGuid
    {
        public Guid id = Guid.NewGuid();

        public lineChainGuid() { }

        public lineChainGuid(string newGuid)
        {
            id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return id.ToString();
        }
    }

    public class ctlRuleItemWidgetGuid
    {
        public Guid id = Guid.Empty;

        public ctlRuleItemWidgetGuid() {}

        public ctlRuleItemWidgetGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }

    public enum pinDirection { input, output }
}
