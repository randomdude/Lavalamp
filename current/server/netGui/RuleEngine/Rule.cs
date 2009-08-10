using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    [Serializable]
    [XmlRootAttribute("rule") ]
    public partial class Rule : IXmlSerializable 
    {
        public String name;
        public ruleState state;
        public Dictionary<String, ruleItems.ruleItemBase> ruleItems = new Dictionary<string, ruleItemBase>();
        public Dictionary<String, lineChain> lineChains = new Dictionary<string, lineChain>();
        public Dictionary<String, pin> pins = new Dictionary<string, pin>();
        public Dictionary<String, ctlRuleItemWidget> ctlRuleItemWidgets = new Dictionary<string, ctlRuleItemWidget>();

        public delegate lineChain GetLineChainFromGuidDelegate(lineChainGuid connection);
        public delegate ruleItems.ruleItemBase GetRuleItemFromGuidDelegate(ruleItemGuid connection);
        public delegate pin PinFromGuidDelegate(pinGuid connection);
        public delegate ctlRuleItemWidget GetctlRuleItemWidgetFromGuidDelegate(ctlRuleItemWidgetGuid connection);
        public delegate void AddLineChainToGlobalPoolDelegate(lineChain connection);
        public delegate void AddRuleItemToGlobalPoolDelegate(ruleItemBase addThis);
        public delegate void AddPinToGlobalPoolDelegate(pin addThis);
        public delegate void AddctlRuleItemWidgetToGlobalPoolDelegate(ctlRuleItemWidget addThis);
        public delegate List<lineChain> GetAllWiresDelegate();
        public delegate pin GetPinFromNameDelegate(string pinName);

        #region serialisation methods

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

                if (xmlName == "name" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    this.name = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }
                if (xmlName == "state" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    this.state = (ruleState) Enum.Parse(state.GetType(), reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "linechains" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    ReadLineChainDictionary(reader, this.generateDelegates());
                    inhibitNextRead = true;
                }
                if (xmlName == "pins" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    ReadPinDictionaryInToRule(reader, this.generateDelegates());
                    inhibitNextRead = true;
                }
                if (xmlName == "ruleitems" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    ReadRuleItemDictionary(reader, generateDelegates());
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead )
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("name", name);
            writer.WriteElementString("state", state.ToString());

            writer.WriteElementRuleItemDictionary("ruleItems", ruleItems);  // this _must_ be before the others!
            writer.WriteElementLineChainDictionary("lineChains", lineChains);
            writer.WriteElementPinDictionary("pins", pins);
        }

        #endregion

        public Rule()
        {
            name = "New rule";
            commonConstructorStuff();
        }

        public Rule(String newName)
        {
            this.name = newName;
            commonConstructorStuff();
        }

        private void commonConstructorStuff()
        {
            this.state = ruleState.stopped;
        }

        new public String ToString()
        {
            return name;
        }

        public void AddLineChainToGlobalPool(lineChain addThis)
        {
            lineChains.Add(addThis.serial.id.ToString(), addThis);
        }

        public void AddRuleItemToGlobalPool(ruleItems.ruleItemBase addThis)
        {
            ruleItems.Add(addThis.serial.id.ToString(), addThis);            
        }

        public void AddPinToGlobalPool(pin addThis)
        {
            if (!pins.ContainsKey(addThis.serial.id.ToString()))
                pins.Add(addThis.serial.id.ToString(), addThis);
        }

        public void AddctlRuleItemWidgetToGlobalPool(ctlRuleItemWidget addThis)
        {
            ctlRuleItemWidgets.Add(addThis.serial.id.ToString() , addThis);
        }

        public ctlRuleItemWidget GetctlRuleItemWidgetFromGuid(ctlRuleItemWidgetGuid connection)
        {
            return ctlRuleItemWidgets[connection.id.ToString()];
        }

        public lineChain GetLineChainFromGuid(lineChainGuid connection)
        {
            return (lineChains[connection.id.ToString()]);
        }

        public ruleItems.ruleItemBase GetRuleItemFromGuid(ruleItemGuid connection)
        {
            return ruleItems[connection.id.ToString()];
        }

        public pin GetPinFromGuid(pinGuid connection)
        {
            return pins[connection.id.ToString()];
        }

        public pin GetPinFromName(string pinName)
        {
            // sloowww
            foreach (pin thisPin in pins.Values)
            {
                if (thisPin.name == pinName)
                    return thisPin;
            }
            throw new PinNotFoundException();
        }

        public delegatePack generateDelegates()
        {
            return new delegatePack(GetLineChainFromGuid, GetRuleItemFromGuid, GetPinFromGuid, AddLineChainToGlobalPool, AddRuleItemToGlobalPool, AddPinToGlobalPool, AddctlRuleItemWidgetToGlobalPool, GetPinFromName, GetctlRuleItemWidgetFromGuid);
        }
    }

    [Serializable]
    public enum ruleState
    {
        stopped, running
    }

    // TODO: WHAT IN THE HOLY NAME OF FUCK
    public class delegatePack
    {
        // bwahhaha silly labels!
        public delegatePack(Rule.GetLineChainFromGuidDelegate a, Rule.GetRuleItemFromGuidDelegate b, Rule.PinFromGuidDelegate c, Rule.AddLineChainToGlobalPoolDelegate d, Rule.AddRuleItemToGlobalPoolDelegate e, Rule.AddPinToGlobalPoolDelegate f, Rule.AddctlRuleItemWidgetToGlobalPoolDelegate g, Rule.GetPinFromNameDelegate h, Rule.GetctlRuleItemWidgetFromGuidDelegate i)
        {
            GetLineChainFromGuid = a;
            GetRuleItemFromGuid=b;
            GetPinFromGuid=c;
            AddLineChainToGlobalPool=d;
            AddRuleItemToGlobalPool=e;
            AddPinToGlobalPool=f;
            AddctlRuleItemWidgetToGlobalPool = g;
            GetPinFromName = h;
            GetCtlRuleFromGuid = i;
        }
        public Rule.GetLineChainFromGuidDelegate GetLineChainFromGuid;
        public Rule.GetRuleItemFromGuidDelegate GetRuleItemFromGuid;
        public Rule.PinFromGuidDelegate GetPinFromGuid;
        public Rule.GetctlRuleItemWidgetFromGuidDelegate GetCtlRuleFromGuid;

        public Rule.AddLineChainToGlobalPoolDelegate AddLineChainToGlobalPool;
        public Rule.AddRuleItemToGlobalPoolDelegate AddRuleItemToGlobalPool;
        public Rule.AddPinToGlobalPoolDelegate AddPinToGlobalPool;
        public Rule.AddctlRuleItemWidgetToGlobalPoolDelegate AddctlRuleItemWidgetToGlobalPool;

        public Rule.GetPinFromNameDelegate GetPinFromName;
    }

}
