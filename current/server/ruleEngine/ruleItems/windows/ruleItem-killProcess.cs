﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems.windows
{
    [ToolboxRule]
    [ToolboxRuleCategory("Windows tools")]
    public class ruleItem_killProcess : ruleItemBase
    {
        [XmlElement("name")] 
        public string name = "(not set)";
        private bool _lastInput;

        public override string ruleName() { return "Close a process"; }

        public override System.Drawing.Image background()
        {
            System.Drawing.Bitmap img = Properties.Resources.delete;
        
            return img;
        }

        public override string caption()
        {
            return "Kill process\n'" + name + "'";
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "trigger to close", direction = pinDirection.input });

            return pinList;
        }

        public override void evaluate()
        {
            bool input1 = (bool)pinInfo["input1"].value.asBoolean();

            if (input1 && !_lastInput)
                killProcess();

            _lastInput = input1;

        }

        private void killProcess()
        {
            Process[] toKill = Process.GetProcessesByName(name);

            foreach (Process thisProcess in toKill)
                thisProcess.Kill();
        }


        public override System.Windows.Forms.ContextMenuStrip addMenus(System.Windows.Forms.ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = base.addMenus(strip1);

            while (strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            return toRet;
        }

        public override IFormOptions setupOptions()
        {
            PickProcessOptions picker = new PickProcessOptions(name);
            return picker;
        }

        public override Size preferredSize() { return new Size(87, 80); }
    }

    public class PickProcessOptions : BaseOptions
    {
        [XmlElement("ProcessName")]
        public string pName;

        public PickProcessOptions()
        {
        }

        public PickProcessOptions(string name)
        {
            pName = name;
        }

        public override string displayName
        {
            get
            {
                return "Choose a process";
            }
        }

        public override string typedName
        {
            get
            {
               return "PickProcess";
            }
        }

    }
}
