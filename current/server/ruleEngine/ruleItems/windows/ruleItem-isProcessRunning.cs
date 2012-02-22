using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine.pinDataTypes;
using ruleEngine.Properties;
using ruleEngine;

namespace ruleEngine.ruleItems.windows
{
    [ToolboxRule]
    [ToolboxRuleCategory("Windows tools")]
    public class ruleItem_isProcessRunning : ruleItemBase
    {
        private readonly ctlIsProcRunning control = new ctlIsProcRunning();
        private bool lastState;

        public override System.Drawing.Size preferredSize()
        {
            return new Size ( control.Width, control.Height);
        }

        public ruleItem_isProcessRunning()
        {
            this.controls.Add(control);
        }

        public override string ruleName() { return "Is process running?"; }

        public override System.Drawing.Image background()
        {
            return Resources.Gear; 
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("trigger", new pin { name = "trigger", description = "trigger to check for process ", direction = pinDirection.input });
            pinList.Add("output1", new pin { name = "output1", description = "process is running", direction = pinDirection.output, valueType = typeof(pinDataTristate) });

            return pinList;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = base.addMenus(mnuParent);

            return control.addMenus(toRet);
        }

        public override void evaluate()
        {
            bool trigger = (bool)pinInfo["trigger"].value.asBoolean();

            if (!trigger || (lastState == trigger))
            {
                lastState = trigger;
                return;
            }
            lastState = true;

            tristate newState = tristate.no;
            foreach (Process runningProcess in Process.GetProcesses())
            {
                if (runningProcess.ProcessName.ToUpper().Trim() == control.processName.ToUpper().Trim())
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