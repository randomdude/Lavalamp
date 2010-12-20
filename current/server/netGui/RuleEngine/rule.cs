using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    public partial class rule : IXmlSerializable 
    {
        public String name;

        private ruleState _state;

        /// <summary>
        /// The state of our rule - running, stopped, etc. 
        /// </summary>
        public ruleState state
        {
            get { return _state; }
            set
            {
                _state = value;

                // If there's a delegate hander, notify it that the status has changed
                if (onStatusUpdate != null)
                    onStatusUpdate.Invoke(this);
            }
        }


        /// <summary>
        /// RuleItems are items in the rule - ie, the blocks that discretely do things. 'at start of run' etc.
        /// </summary>
        public Dictionary<String, ruleItems.ruleItemBase> ruleItems = new Dictionary<string, ruleItemBase>();

        /// <summary>
        /// Each ruleItem has a control which represents it in the UI.
        /// </summary>
        public Dictionary<String, ctlRuleItemWidget> ctlRuleItems = new Dictionary<string, ctlRuleItemWidget>();

        /// <summary>
        /// The 'wires' that connect ruleItems
        /// </summary>
        public Dictionary<String, lineChain> lineChains = new Dictionary<string, lineChain>();

        /// <summary>
        /// The input or output ports attached to ruleItems
        /// </summary>
        public Dictionary<String, pin> pins = new Dictionary<string, pin>();

        /// <summary>
        /// This delegate gets called on updates to the rule's status. Obviously.
        /// </summary>
        public onStatusUpdateDelegate onStatusUpdate = null;
        public delegate void onStatusUpdateDelegate(rule updating);


        // See comments at definition of these. They're an abomination to OO.
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

        public rule()
        {
            name = "New rule";
            this._state = ruleState.stopped;
        }

        public rule(String newName)
        {
            this.name = newName;
            this._state = ruleState.stopped;
        }

        new public String ToString()
        {
            return name;
        }

        #region "global pool stuff"

        // Here we have manipulation functions for our collections. 'global' in this sense means to the rule.
        // Really, needing this functions is a symptom of the code not being wonderfully OO. Wish I'd thought
        // ahead a bit more when I wrote them..
        //
        // TODO/FIXME: Remove neccesity for these functions

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
            ctlRuleItems.Add(addThis.serial.id.ToString() , addThis);
        }

        public ctlRuleItemWidget GetctlRuleItemWidgetFromGuid(ctlRuleItemWidgetGuid connection)
        {
            return ctlRuleItems[connection.id.ToString()];
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
            throw new pinNotFoundException();
        }
        #endregion
        public delegatePack generateDelegates()
        {
            return new delegatePack(GetLineChainFromGuid, GetRuleItemFromGuid, GetPinFromGuid, AddLineChainToGlobalPool, AddRuleItemToGlobalPool, AddPinToGlobalPool, AddctlRuleItemWidgetToGlobalPool, GetPinFromName, GetctlRuleItemWidgetFromGuid);
        }

        /// <summary>
        /// Write a new XML file describing the rule to disk.
        /// </summary>
        /// <param name="path">file to write</param>
        public void saveToDisk(string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.Write(this.serialise());
            }
        }

        /// <summary>
        /// Start the rule running!
        /// </summary>
        public void start()
        {
            // mark this as running..
            state = ruleState.running;

            // Reset each ruleItem to non-errored
            foreach (ruleItemBase anItem in ruleItems.Values )
                anItem.clearErrors();

            // And start each one.
            foreach (ruleItemBase anItem in ruleItems.Values )
                anItem.start();
        }

        public void stop()
        {
            // mark this as not running..
            state = ruleState.stopped;

            // Stop each ruleItem, and set all pins to false
            foreach (ruleItemBase anItem in ruleItems.Values)
                anItem.stop();
            
            foreach (ruleItemBase anItem in ruleItems.Values)
            {
                foreach (pin thisPin in anItem.pinInfo.Values)
                {
                    thisPin.value.setToDefault();
                    thisPin.updateUI();
                }
            }

        }
    }
}
