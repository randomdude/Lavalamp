using System;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            mySer.Serialize(myWriter, this);
            return myWriter.ToString();
        }

        #region ISerializable methods
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
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

                if (xmlName == "name" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    name = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }
                if (xmlName == "state" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    state = (ruleState)Enum.Parse(state.GetType(), reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "linechains" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    readLineChainDictionary(reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "pins" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    readPinDictionaryInToRule(reader);
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

            claimPinsPostDeSer();
        }

        public void claimPinsPostDeSer()
        {

            foreach (ruleItemBase thisRuleItem in ruleItems.Values)
            {
                thisRuleItem.claimPinsPostDeSer(pins);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("name", name);
            writer.WriteElementString("state", state.ToString());

            writer.WriteElementRuleItemDictionary("ruleItems", ruleItems);  // this _must_ be before the others! TODO: Is this still the case? Check if it _does_ need to be before the others.
            writer.WriteElementLineChainDictionary("lineChains", lineChains);
            writer.WriteElementPinDictionary("pins", pins);
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

                if (xmlName == "config" && reader.NodeType == XmlNodeType.Element)
                {
                    thisTypeName = reader["type"];

                    // Find the type of our new RuleItem
                    Assembly thisAss = Assembly.GetExecutingAssembly();
                    Type thisType;
                    try
                    {
                        // Pull type out of Ass
                        thisType = thisAss.GetType(thisTypeName);
                    }
                    catch (Exception)
                    {
                        throw new ruleLoadException("unable to create ruleItem of type " + thisTypeName);
                    }

                    // Instantiate the requested type, and load the config for this particular item.
                    XmlSerializer mySer = new XmlSerializer(thisType);
                    reader.Read();
                    ruleItemBase newRuleItem  = mySer.Deserialize(reader) as ruleItemBase;

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
                    AddRuleItemToGlobalPool(newRuleItem);

                    thisSerial = null;

                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
        }

        private void readPinDictionaryInToRule(XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

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
                if (xmlName == "parentruleitem" && reader.NodeType == XmlNodeType.Element)
                {
                    thisPin.parentRuleItem = new ruleItemGuid(reader.ReadElementContentAsString());
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
                    AddPinToGlobalPool(thisPin);
                    thisPin = new pin();
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
        }

    }
}
