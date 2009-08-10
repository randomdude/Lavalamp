using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public partial class Rule
    {
        public void ReadLineChainDictionary(XmlReader reader, delegatePack delegates)
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
                    ReadAndAddLineChain(reader, delegates);
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        private static void ReadAndAddLineChain(XmlReader reader, delegatePack delegates)
        {
            lineChain toRet = new lineChain(delegates);
            toRet.ReadXml(reader);
            delegates.AddLineChainToGlobalPool(toRet);
        }

        public void ReadRuleItemDictionary(XmlReader reader, delegatePack delegates)
        {
            String parentTag = reader.Name.ToLower();

            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "ruleitems" && reader.NodeType == XmlNodeType.Element)
                {
                    readRuleItems(reader, delegates);
                    keepGoing = false;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        private ruleItemBase MakeRuleItem(ruleItemInfo info)
        {
            // Make new ruleItem control of this RuleItem type
            ruleItemBase newRuleItem;
            if (info.itemType == ruleItemType.RuleItem)
            {
                ConstructorInfo constr = info.RuleItemBaseType.GetConstructor(new Type[0]);
                newRuleItem = (ruleItemBase) constr.Invoke(new object[0] { });
            }
            else if (info.itemType == ruleItemType.PythonFile)
            {
                newRuleItem = new ruleItem_python(new pythonEngine(info.pythonFileName));
            }
            else
                throw new Exception("eh? Unrecognised file type?");
            return newRuleItem;
        }

        private void readRuleItems(XmlReader reader, delegatePack delegates)
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

                if (xmlName == "type" && reader.NodeType == XmlNodeType.Element)
                {
                    thisTypeName = reader.ReadElementContentAsString();
                    

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

                    // todo: support python load/saving!

                    ruleItemInfo myInfo = new ruleItemInfo();
                    myInfo.itemType = ruleItemType.RuleItem;
                    myInfo.RuleItemBaseType = thisType;

                    ruleItemBase newRuleItem = MakeRuleItem(myInfo);
                    newRuleItem.serial = new ruleItemGuid(thisSerial);
                    newRuleItem.location = location;
                    delegates.AddRuleItemToGlobalPool(newRuleItem);

                    thisSerial = null;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
        }

        public void ReadPinDictionaryInToRule( XmlReader reader, delegatePack delegates)
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
                    thisPin.direction = (pinDirection) Enum.Parse(typeof (pinDirection), reader.ReadElementContentAsString(), true);
                    inhibitNextRead = true;
                }
                if (xmlName == "pin" && reader.NodeType == XmlNodeType.EndElement)
                {
                    delegates.AddPinToGlobalPool(thisPin);
                    thisPin = new pin();
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
            }
        }

        public string serialise()
        {
            // Pass deserialisation on to ISerializable classes.
            StringWriter myWriter = new StringWriter();
            XmlSerializer mySer = new XmlSerializer(this.GetType());
            mySer.Serialize(myWriter, this);
            return myWriter.ToString();
        }
    }
}
