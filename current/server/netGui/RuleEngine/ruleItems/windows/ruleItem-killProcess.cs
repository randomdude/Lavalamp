using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace netGui.RuleEngine.ruleItems.windows
{
    [ToolboxRule]
    [ToolboxRuleCategory("Windows tools")]
    public class ruleItem_killProcess : ruleItemBase
    {
        [XmlElement("name")] public string name = "(not set)";
        private bool lastInput;
        private Label lblCaption;

        public override string ruleName() { return "Close a process"; }

        public override System.Drawing.Image background()
        {
            return netGui.Properties.Resources.delete;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input1", new pin { name = "input1", description = "trigger to close", direction = pinDirection.input });

            return pinList;
        }

        public override void evaluate()
        {
            bool input1 = (bool) pinStates["input1"].getData();

            if (input1 && !lastInput)
                killProcess();

            lastInput = input1;

        }

        private void killProcess()
        {
            Process[] toKill = Process.GetProcessesByName(name);

            foreach (Process thisProcess in toKill)
                thisProcess.Kill();
        }

        public ruleItem_killProcess()
        {
            lblCaption = new Label();
            lblCaption.AutoSize = false;
            lblCaption.Width = preferredSize().Width;
            lblCaption.Height = 40;
            lblCaption.Left = 0;
            lblCaption.Top = preferredSize().Height - lblCaption.Height;
            lblCaption.Text = "Kill process '" + name + "'";
            lblCaption.Visible = true;
            lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            controls.Add(lblCaption);
        }

        public override System.Windows.Forms.ContextMenuStrip addMenus(System.Windows.Forms.ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = base.addMenus(strip1);

            while (strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            ToolStripSeparator newItem1 = new ToolStripSeparator();
            toRet.Items.Add(newItem1);

            ToolStripMenuItem newItem2 = new ToolStripMenuItem("Choose &Process..");
            newItem2.Click += new EventHandler(pickProcess);
            toRet.Items.Add(newItem2);

            return toRet;
        }

        private void pickProcess(object sender, EventArgs e)
        {
            frmPickProcess picker = new frmPickProcess(name);
            picker.ShowDialog();

            if (!picker.cancelled)
            {
                name = picker.name;
                lblCaption.Text = "Kill process '" + name + "'";
            }
        }
    }
}

namespace netGui.RuleEngine
{
    internal class saveThisAttribute : Attribute
    {
    }
}