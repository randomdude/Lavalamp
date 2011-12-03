using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ruleEngine.ruleItems.windows;

namespace ruleEngine
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
        public IpinData value;

        public delegate void pinChanged(pin changed, EventArgs e); 

        /// <summary>
        /// Event which fires when the pin changes
        /// </summary>
        public event pinChanged OnPinChange; 

        public void createValue(ruleItems.ruleItemBase parentRuleItem)
        {
            // Set the .value of our pin to an object of type according to .valueType.
            // Call the appropriate constructor, finding it via reflection.
            // We pass the constructor the parent ruleItemBase, and the parent pin.

            // Find the constructor
            ConstructorInfo pinValueTypeConstructor = valueType.GetConstructor(new Type[] { typeof(ruleItems.ruleItemBase), typeof(pin) });

            // Call the constructor, storing the new object.
            value = (IpinData) pinValueTypeConstructor.Invoke(new object[] {parentRuleItem, this});
        }

        /// <summary>
        /// Invoke all the delegates in the changeHandlers. TODO: Move to proper Event style.
        /// </summary>
        public void invokeChangeHandlers()
        {
            if (OnPinChange != null)
                OnPinChange.Invoke(this,null);
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

        public void Disconnected(object sender,EventArgs e)
        {
            linkedTo.id = Guid.Empty;
            linkedTo.id = Guid.Empty;
            OnPinChange = null;
        }

        public void connectTo(lineChainGuid newTargetChain, pin newTargetPin)
        {
            parentLineChain.id = newTargetChain.id;
            linkedTo.id = newTargetPin.serial.id;
            OnPinChange += newTargetPin.StateChanged;
        }

        public void StateChanged(pin changed, EventArgs e)
        {
            value.data = changed.value.data;
        }

        // todo - this should be on an onUIUpdate event or suchlike.
        public void updateUI()
        {
            // And now the aesthetic bit - set the pin background.
            if (imageBox != null)   // Will be during tests or when UI-less
                imageBox.BackColor = value.getColour();
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

        public bool isPriority()
        {
            throw new NotImplementedException();
        }
    }

    public enum pinDirection { input, output }
}
