using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ruleEngine;
using ruleEngine.pinDataTypes;
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

        public override Size preferredSize() { return new Size( 150, 75 ); }

        public desktopMessageOptions myOptions = new desktopMessageOptions();

        private string _lastMessage = "";

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

        public override void start()
        {
            _lastMessage = "";
        }

        public override void evaluate()
        {
            IPinData inputData = pinInfo["trigger"].value;
            
            if (_lastMessage == inputData.ToString())
                return;

            _lastMessage = inputData.ToString();
            // Swap out the placeholder with the new message.
            string messageToShow = myOptions.message.Replace("$message", _lastMessage);

            showIt(messageToShow);
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip toAddTo)
        {
            return base.addMenus(toAddTo);
        }

        public override Form ruleItemOptions()
        {
            FrmDesktopMessageOptions myOptForm = new FrmDesktopMessageOptions(myOptions);
            myOptForm.Closed += delegate { if (myOptForm.DialogResult == DialogResult.OK)
                                                myOptions = myOptForm.currentOptions; };
            return myOptForm;
        }

        public void showIt(string messageToShow)
        {
            // Mind the threading trickery!
            // We make a new Timer, thus creating a new thread.
            // On this, we make a messageLoop for it, and show the new form.. 
            System.Threading.Timer myTimer = new System.Threading.Timer(timercallback, messageToShow, 1, Timeout.Infinite);
        }

        private void timercallback(object state)
        {
            string messageToShow = (string) state;
            // make our form
            frmDesktopMessage messageForm = new frmDesktopMessage(myOptions, messageToShow);
            messageForm.Visible = false;

            // Kick up a message loop and show the form.
            Application.DoEvents();
            Application.Run(messageForm);
        }
    }
}
