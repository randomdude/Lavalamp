using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using ruleEngine.Properties;

namespace ruleEngine.ruleItems.windows
{
    [ToolboxRule]
    [ToolboxRuleCategory("Windows tools")]
    public class ruleItem_isProcessRunning : ruleItemBase
    {

        [XmlElement("options")]
        public PickProcessOptions options = new PickProcessOptions();

        public string processName { get { return options.pName; } set { options.pName = value; } }

        public override string caption()
        {
            return "Is " + processName + " Running?";
        }

        public override string ruleName() { return "Is process running?"; }

        public override System.Drawing.Image background()
        {
            return Resources.Gear; 
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("trigger", new pin { name = "trigger", description = "trigger to check for process ", direction = pinDirection.input });
            pinList.Add("output1", new pin { name = "output1", description = "process is running", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }


        public override void evaluate()
        {
            bool trigger = this.pinInfo["trigger"].value.asBoolean();

            if (!trigger)
            {
                return;
            }

            tristate newState = tristate.no;
            foreach (Process runningProcess in Process.GetProcesses())
            {
                if (runningProcess.ProcessName.ToUpper().Trim() == processName.ToUpper().Trim())
                {
                    newState = tristate.yes;
                    break;
                }
            }

            pinInfo["output1"].value.data = newState;
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTristate(pinInfo["output1"].value)));
        }

    }
}