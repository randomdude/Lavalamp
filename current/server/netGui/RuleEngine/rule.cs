﻿using System;
using System.Collections.Generic;
using System.IO;
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

            // Stop each ruleItem, then and set all pins to false.
            foreach (ruleItemBase anItem in ruleItems.Values)
                if (!anItem.isDeleted) 
                    anItem.stop();
            
            foreach (ruleItemBase anItem in ruleItems.Values)
            {
                if (anItem.isDeleted)
                    continue;

                foreach (pin thisPin in anItem.pinInfo.Values)
                {
                    thisPin.value.setToDefault();
                    thisPin.updateUI();
                }
            }
        }

        public bool isRunning
        {
            get { return state == ruleState.running; }
        }

        public ruleItemBase addRuleItem(ruleItemInfo info)
        {
            // Make new ruleItem control of this RuleItem type. 
            ruleItemBase newRuleItem;
            if (info.itemType == ruleItemType.RuleItem)
            {
                // .net ruleItems are loaded via reflection. We find the parameterless
                // constructor, and then call it.
                ConstructorInfo constr = info.ruleItemBaseType.GetConstructor(new Type[0]);
                newRuleItem = (ruleItemBase)constr.Invoke(new object[0] { });
            }
            else if (info.itemType == ruleItemType.scriptFile)
            {
                // Script items are loaded by their own constructor, as we need information
                // at runtime.
                newRuleItem = new ruleItem_script(info.pythonFileName);
            }
            else
                // This should only happen if a ruleItem is loaded of an unsupported
                // type - ie, never.
                throw new Exception("Unrecognised file type");

            // Initialise the Pins on the control. This will generate a new guid for each pin.
            // FIXME: It's kind of messy that we have to do this here.
            newRuleItem.initPins();

            generateDelegates().AddRuleItemToGlobalPool(newRuleItem);
            return newRuleItem;
        }

        public IEnumerable<ruleItemBase> getRuleItems()
        {
            return ruleItems.Values;
        }

        /// <summary>
        /// Return a new collection, containing only non-deleted lineChains.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<lineChain> getNonDeletedLineChains()
        {
            IEnumerable<lineChain> lineChains = getLineChains();
            return lineChains.Where(lineChain => !lineChain.deleted).ToList();
        }

        public IEnumerable<lineChain> getLineChains()
        {
            return lineChains.Values;
        }

        public static rule deserialise(string serialised)
        {
            // Create a new rule, and deserialise in to it.
            XmlSerializer mySer = new XmlSerializer(typeof(rule));
            Encoding ascii = Encoding.BigEndianUnicode;
            Stream stream = new MemoryStream(ascii.GetBytes(serialised));
            return (rule)mySer.Deserialize(stream);
        }

        public void deleteRuleItem(ruleItemBase toDelete)
        {
            // Remove any lineChains attatched to the to-delete item
            foreach (pin thisPin in toDelete.pinInfo.Values)
            {
                if (thisPin.isConnected)
                {
                    lineChain tonuke = GetLineChainFromGuid(thisPin.parentLineChain);
                    tonuke.deleteSelf();
                }
            }

            // mark as deleted.
            toDelete.isDeleted = true;
        }

        public void deleteCtlRuleItem(ctlRuleItemWidget toDelete)
        {
            ctlRuleItems.Remove(toDelete.serial.id.ToString());
        }

        public IEnumerable<pin> getPins()
        {
            return pins.Values;
        }
    }
}
