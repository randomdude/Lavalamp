using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;
using netGui.RuleEngine.ruleItems.windows;

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

        /// <summary>
        /// The type of data which is present on the pin
        /// </summary>
        public Type valueType = typeof (pinDataBool);

        /// <summary>
        /// The data which is present on the pin
        /// </summary>
        public pinData value;

        /// <summary>
        /// Delegates which are fired when this pin changes. TODO: Move to proper Event style.
        /// </summary>
        public readonly List<ruleItemBase.changeNotifyDelegate> changeHandlers = new List<ruleItemBase.changeNotifyDelegate>();

        /// <summary>
        /// Invoke all the delegates in the changeHandlers. TODO: Move to proper Event style.
        /// </summary>
        public void invokeChangeHandlers()
        {
            foreach (ruleItemBase.changeNotifyDelegate thisDelegate in changeHandlers)
            {
                thisDelegate.Invoke();
            }
        }

        public void addChangeHandler(ruleItemBase.changeNotifyDelegate target)
        {
            changeHandlers.Add(target);
        }

        public void removeAllPinHandlers()
        {
            changeHandlers.Clear();
        }

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

    public enum pinDirection { input, output }
}
