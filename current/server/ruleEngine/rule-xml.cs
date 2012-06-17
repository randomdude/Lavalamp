using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    // XML serialisation and deserialisation routines for the rule class.

    partial class rule
    {
        /// <summary>
        /// Convenience method to pass to ISerializable
        /// </summary>
        /// <returns>A serialised rule</returns>
        public string serialise()
        {
            // Pass deserialisation on to ISerializable classes.
            StringWriter myWriter = new StringWriter();
            XmlSerializer mySer = new XmlSerializer(this.GetType());
            mySer.Serialize(myWriter , this);
            return myWriter.ToString();
        }

        #region IXmlSerializable methods

        public XmlSchema GetSchema()
        {
            XmlSchema schema;

            using (
                StreamReader reader =
                    new StreamReader(File.Open(@"C:\inetpub\wwwroot\lavalamp\service\rule-schema.xsd" , FileMode.Open)))
            {
                schema = XmlSchema.Read(reader , null);
            }
            return schema;
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

                if (xmlName == "preferredheight" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    preferredHeight = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "preferredwidth" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    preferredWidth = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "name" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    name = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }
                if (xmlName == "state" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    state = (ruleState) Enum.Parse(state.GetType() , reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "linechains" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    readLineChainDictionary(reader);
                    inhibitNextRead = true;
                }

                if (xmlName == "ruleitems" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    readRuleItemDictionary(reader);
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
            // for backward compatibility with rules with out preferred sizes and sanity checks.
            if (preferredHeight == 0) // abortary defaults if 0
                preferredHeight = 334;
            else if (preferredHeight > SystemInformation.PrimaryMonitorSize.Height)
                preferredHeight = SystemInformation.PrimaryMonitorSize.Height;
            if (preferredWidth == 0)
                preferredWidth = 613;
            else if (preferredWidth > SystemInformation.PrimaryMonitorSize.Width)
                preferredWidth = SystemInformation.PrimaryMonitorSize.Width;
            hookPinConnectionsUp(ruleItems.Values);

        }
        private void hookPinConnectionsUp   (IEnumerable<ruleItemBase> itemsToConnect )
        {
            foreach (var itemToConnectPins in itemsToConnect)
            {
                itemToConnectPins.hookPinConnectionsUp(itemsToConnect);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("name", name);
            writer.WriteElementString("preferredHeight", preferredHeight.ToString());
            writer.WriteElementString("preferredWidth", preferredWidth.ToString());
            writer.WriteElementString("state", state.ToString());
            writer.WriteElementRuleItemDictionary("ruleItems", ruleItems);  // this _must_ be before the others! TODO: Is this still the case? Check if it _does_ need to be before the others.
            writer.WriteElementLineChainDictionary("lineChains", lineChains);
            
        }

        #endregion

        // And now a squillion helper methods
        private void readLineChainDictionary(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "linechain" && reader.NodeType == XmlNodeType.Element)
                {
                    readAndAddLineChain(reader);
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        private void readAndAddLineChain(XmlReader reader)
        {
            lineChain toRet = new lineChain();
            toRet.ReadXml(reader);
            AddLineChainToGlobalPool(toRet);
        }

        private void readRuleItemDictionary(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "ruleitems" && reader.NodeType == XmlNodeType.Element)
                {
                    readRuleItems(reader);
                    keepGoing = false;
                }

                if (keepGoing)
                    keepGoing = reader.Read();
            }
        }

        private ruleItemBase makeRuleItem(ruleItemInfo info)
        {
            // Make new ruleItem control of this RuleItem type
            ruleItemBase newRuleItem;
            if (info.itemType == ruleItemType.RuleItem)
            {
                ConstructorInfo constr = info.ruleItemBaseType.GetConstructor(new Type[0]);
                newRuleItem = (ruleItemBase)constr.Invoke(new object[0] { });
            }
            else if (info.itemType == ruleItemType.scriptFile)
            {
                newRuleItem = new ruleItem_script(info.pythonFileName);
            }
            else
                throw new Exception("eh? Unrecognised file type?");
            return newRuleItem;
        }

        private void readRuleItems(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();
            String thisSerial = null;
            String thisTypeName = null;
            Point location = new Point();
            Dictionary<string , pin> pinInfo = null;
            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                inhibitNextRead = false;
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "x" && reader.NodeType == XmlNodeType.Element)
                {
                    location.X = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "y" && reader.NodeType == XmlNodeType.Element)
                {
                    location.Y = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "ruleitem" && reader.NodeType == XmlNodeType.Element)
                {
                    thisSerial = reader["serial"];
                }

                if (xmlName == "pins" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    pinInfo = readPinDictionaryInToRule(reader);
                    foreach (pin p in pinInfo.Values)
                    {
                        p.parentRuleItem = new ruleItemGuid(thisSerial);
                    }
                    inhibitNextRead = true;
                }

                if (xmlName == "config" && reader.NodeType == XmlNodeType.Element)
                {
                    string thisAssemblyName = reader["assembly"];
                    thisTypeName = reader["type"];
                    // Find the type of our new RuleItem
                    // prefer our assembly and check it for the type first to avoid conflict with the asp.net referenced assembly.
                    // else check the assembly included (hopefully) in the file
                    Type thisType = Assembly.GetExecutingAssembly().GetType(thisTypeName, false)
                                    ?? Assembly.LoadFile(thisAssemblyName).GetType(thisTypeName,false);

                    if (thisType == null )
                        throw new ruleLoadException("unable to create ruleItem of type " + thisTypeName);
      
                    // Instantiate the requested type, and load the config for this particular item.
                    XmlSerializer mySer = new XmlSerializer(thisType);
                    reader.Read();
                    var ruleItemObj = mySer.Deserialize(reader);
                    ruleItemBase newRuleItem = (ruleItemBase)ruleItemObj;
                    // todo: support python load/saving!
                    // will the following few lines be useful when we do?
//                    ruleItemInfo myInfo = new ruleItemInfo();
//                    myInfo.itemType = ruleItemType.RuleItem;
//                    myInfo.ruleItemBaseType = thisType;
//
//                    ruleItemBase newRuleItem = makeRuleItem(myInfo);

                    // Propogate the stuff we've read in to it
                    newRuleItem.serial = new ruleItemGuid(thisSerial);
                    newRuleItem.location = location;
                    newRuleItem.pinInfo = pinInfo;
                    AddRuleItemToGlobalPool(newRuleItem);

                    thisSerial = null;

                    // Notify the ruleItem that we have finished loading it
                    newRuleItem.onAfterLoad();
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
        }

        private Dictionary<string,pin> readPinDictionaryInToRule(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();
            Dictionary<string,pin> pinInfo = new Dictionary<string , pin>();
            pin thisPin = new pin();

            bool keepGoing = true;
            bool inhibitNextRead;
            while (keepGoing)
            {
                inhibitNextRead = false;
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "serial" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.serial = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "linkedto" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.linkedTo = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "parentlinechain" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.parentLineChain = new lineChainGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "description" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.description = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }
                if (xmlName == "name" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.name = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }
                if (xmlName == "direction" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.direction = (pinDirection)Enum.Parse(typeof(pinDirection), reader.ReadElementContentAsString(), true);
                    inhibitNextRead = true;
                }
                if (xmlName == "datatype" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.valueType = Type.GetType(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "pin" && reader.NodeType == XmlNodeType.EndElement)
                {
                    pinInfo.Add(thisPin.name,thisPin);
                    thisPin = new pin();
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
            return pinInfo;
        }

    }
}
