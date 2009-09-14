using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using netGui.Properties;

namespace netGui.RuleEngine.ruleItems.windows
{
    [RuleEngine.ToolboxRule]
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
            this.pinStates.evaluate = new evaluateDelegate(evaluate);
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
            pinList.Add("output1", new pin { name = "output1", description = "process is running", direction = pinDirection.output });

            return pinList;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = base.addMenus(mnuParent);

            return control.addMenus(toRet);
        }
 
        public override void evaluate()
        {
            bool trigger = (bool)pinStates["trigger"];

            bool newState = false;

            if (trigger && lastState != trigger)
            {
                foreach (Process runningProcess in Process.GetProcesses())
                {
                    if (runningProcess.ProcessName.ToUpper().Trim() == control.processName.ToUpper().Trim())
                    {
                        newState = true;
                        break;
                    }
                }
            }

            lastState = trigger;

            if ((bool)pinStates["output1"] != newState)
                pinStates["output1"] = newState;
        }

    }
}