using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ruleEngine;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Notifiers")]
    public class ruleItem_desktopMessage : ruleItemBase
    {
        public override string ruleName() { return "Show desktop message"; }

        // TODO: allow dynamically-changing captions 
        public override string caption() { return "Show message"; }

        public override Size preferredSize() { return new Size( 150,75 ); }

        public desktopMessageOptions myOptions = new desktopMessageOptions();

        private bool lastState;
        private string _lastMessage;

        // Every ruleItem requires a parameterless constructor. It is used by the toolbox
        // routines via reflection.
// ReSharper disable UnusedMember.Global
        public ruleItem_desktopMessage() { }
// ReSharper restore UnusedMember.Global

        public ruleItem_desktopMessage(desktopMessageOptions newOptions)
        {
            myOptions = newOptions;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("trigger", new pin { name = "trigger", description = "trigger to show notifier", direction = pinDirection.input, valueType = typeof(pinDataString) });

            return pinList;
        }

        public override void evaluate()
        {
            IpinData inputData = pinInfo["trigger"].value;
            bool newState = inputData.asBoolean();

            // if we have a message swap out the placeholder with the new message.
            if (inputData.getDataType() == typeof(string))
            {
                myOptions.message = myOptions.message.Replace("$message", pinInfo["trigger"].value.ToString());
                _lastMessage = myOptions.message;
            }
            if ( (newState != lastState || myOptions.message != _lastMessage) && (newState == true))
                showIt();

            lastState = newState;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip toAddTo)
        {
            ToolStripItem optionsItem = new ToolStripMenuItem("&Options");
            optionsItem.Click += showOptionsDialog;
            toAddTo.Items.Add(optionsItem);

            return base.addMenus(toAddTo);
        }

        private void showOptionsDialog(object sender, EventArgs e)
        {
            FrmDesktopMessageOptions myOptForm = new FrmDesktopMessageOptions(myOptions);
            if (myOptForm.ShowDialog() == DialogResult.OK)
                myOptions = new desktopMessageOptions(myOptForm.currentOptions);
        }

        public void showIt()
        {
            // Mind the threading trickery!
            // We make a new Timer, thus creating a new thread.
            // On this, we make a messageLoop for it, and show the new form.. 
            System.Threading.Timer myTimer = new System.Threading.Timer(timercallback, null, 1, Timeout.Infinite  );
        }

        private void timercallback(object state)
        {
            // make our form
            frmDesktopMessage messageForm = new frmDesktopMessage(myOptions);
            messageForm.Visible = false;

            // Kick up a message loop and show the form.
            Application.DoEvents();
            Application.Run(messageForm);
        }

        public override void stop()
        {
            base.stop();
        }
    }
}
