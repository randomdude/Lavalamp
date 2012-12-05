using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    [Serializable]
    [XmlRootAttribute("rule") ]
    public partial class rule : IXmlSerializable, IRule
    {
        public String name { get;  set; }

        public string details
        {
            get
            {
                return String.Join("\n", this._errorList.Select(s => s.Message).ToArray());
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool toolboxPromoted;
        private ruleState _state;

        /// <summary>
        /// The state of our rule - running, stopped, etc. 
        /// </summary>
        public ruleState? state
        {
            get { return _state; }
            set
            {
                if (value.HasValue)
                    _state = value.Value;
                else
                {
                    _state = ruleState.stopped;
                }
                // If there's a delegate hander, notify it that the status has changed
                if (onStatusUpdate != null)
                    onStatusUpdate.Invoke(this);
            }
        }

        /// <summary>
        /// RuleItems are items in the rule - ie, the blocks that discretely do things. 'at start of run' etc.
        /// </summary>
        public Dictionary<String, ruleItemBase> ruleItems = new Dictionary<string, ruleItemBase>();

        /// <summary>
        /// The 'wires' that connect ruleItems
        /// </summary>
        private Dictionary<String, lineChain> lineChains = new Dictionary<string, lineChain>();

        /// <summary>
        /// This delegate gets called on updates to the rule's status.
        /// </summary>
        public onStatusUpdateDelegate onStatusUpdate = null;
        public delegate void onStatusUpdateDelegate(rule updating);

        private timeline _timeline = new timeline();

        private List<Exception> _errorList = new List<Exception>();

        public bool isErrored
        { 
            get
            {
                return _errorList.Any();
            } 
        }

        public int lineChainCount { get { return lineChains.Count; } }
        public int ruleItemCount { get { return ruleItems.Count; } }

        public rule()
        {
            name = "New rule";
            this._state = ruleState.stopped;

            _timeline.eventOccuring += handleTimelineEvent;
            _timeline.timelineAdvance += handleTimelineAdvance;
        }

        public rule(String newName)
        {
            this.name = newName;
            this._state = ruleState.stopped;

            _timeline.eventOccuring += handleTimelineEvent;
            _timeline.timelineAdvance += handleTimelineAdvance;
        }

        new public String ToString()
        {
            return name;
        }

        /// <summary>
        /// Take an event and pass it to the relevant pin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleTimelineEvent(timeline sender, timelineEvent e)
        {
            e.pinValue.performUpdate();
        }

        /// <summary>
        /// Move the timeline forward one delta
        /// </summary>
        /// <param name="sender"></param>
        private void handleTimelineAdvance(timeline sender)
        {
            foreach (ruleItemBase thisRuleItem in ruleItems.Values)
            {
                thisRuleItem.evaluate(false);
            }
        }

        /// <summary>
        /// Make a new timeline event in the future
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="timebeforeevent">Deltas until event should fire</param>
        private void serviceNewTimelineEventInFuture(ruleItemBase sender, timelineEventArgs e, int timebeforeevent)
        {
            _timeline.addEventAtDelta(e, timebeforeevent);
        }

        /// <summary>
        /// Make a new timeline event at the next delta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceNewTimelineEvent(ruleItemBase sender, timelineEventArgs e)
        {
            _timeline.addEventAtNextDelta(e);
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

        public void AddRuleItemToGlobalPool(ruleItemBase addThis)
        {
            addThis.newTimelineEvent += serviceNewTimelineEvent;
            addThis.newTimelineEventInFuture += serviceNewTimelineEventInFuture;
            ruleItems.Add(addThis.serial.id.ToString(), addThis);            
        }

        public lineChain GetLineChainFromGuid(lineChainGuid connection)
        {
            return (lineChains[connection.id.ToString()]);
        }

        public ruleItemBase GetRuleItemFromGuid(ruleItemGuid connection)
        {
            return (ruleItems[connection.id.ToString()]);
        }

        #endregion

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

        public void changeName(string newName)
        {
            //TODO Repository change to save the new name to disk
            throw new NotImplementedException();
        }

        public void changeName(string path, string newName)
        {
            if (!string.IsNullOrEmpty(path))
                path += @"\";

            if(File.Exists(path + newName + ".rule"))
                throw new IOException(newName + " already exists!");
            // if the rule was saved to disk we delete the old file name and recreate it else we just rename the object in memory
            string oldName = name;
            name = newName;
            try
            {
                if (File.Exists(path + oldName + ".rule"))
                {
                    File.Delete(path + oldName + ".rule");
                    saveToDisk(path + newName + ".rule");
                }
            } // if for some reason this fails we roll back to the previous state
            catch (Exception)
            {
                name = oldName;
                if (File.Exists(path + newName + ".rule"))
                {
                    File.Delete(path + newName + ".rule");
                    if (!File.Exists(path + oldName + ".rule"))
                        saveToDisk(path + oldName + ".rule");
                }
                throw;
            }
            
        }

        /// <summary>
        /// Start the rule running!
        /// </summary>
        public void start()
        {
            // Reset the timeline
            _timeline.reset();

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
                    //thisPin.value.setToDefault();
                    thisPin.updateUI();
                }
            }
        }

        public void advanceDelta(object sender, EventArgs e)
        {
            advanceDelta();
        }

        /// <summary>
        /// Move one delta step forward
        /// </summary>
        public void advanceDelta()
        {
            _timeline.advanceDelta();
        }

        public bool isRunning
        {
            get { return state == ruleState.running; }
        }

        public int preferredHeight { get;set;}

        public int preferredWidth { get; set;}

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

            AddRuleItemToGlobalPool(newRuleItem);
            return newRuleItem;
        }

        public IEnumerable<IRuleItem> getRuleItems()
        {
            return ruleItems.Values.Select(r => r as IRuleItem);
        }

        /// <summary>
        /// Return a new collection, containing only non-deleted lineChains.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<lineChain> getNonDeletedLineChains()
        {
            IEnumerable<lineChain> lineChains = getLineChains();
            return lineChains.Where(lineChain => !lineChain.isDeleted).ToList();
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
                    tonuke.requestDelete();
                }
            }

            // mark as deleted.
            toDelete.isDeleted = true;
            //actually remove the item from the list.
            ruleItems.Remove(toDelete.serial.ToString());
        }

        public void addError(Exception s)
        {
            _errorList.Add(s);
        }

        public Exception getError()
        {
            return this.isErrored ? this._errorList[0] : null;
        }
    }
}
