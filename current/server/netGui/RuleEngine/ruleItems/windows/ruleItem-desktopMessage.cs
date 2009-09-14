using System;
using System.Collections.Generic;
using System.Windows.Forms;
using netGui.RuleEngine.ruleItems.windows;

namespace netGui.RuleEngine.ruleItems 
{
    [RuleEngine.ToolboxRule]
    [RuleEngine.ToolboxRuleCategory("Notifiers")]
    public class ruleItem_desktopMessage : ruleItemBase
    {
        public override string ruleName() { return "Show desktop message"; }

        public desktopMessageOptions myOptions = new desktopMessageOptions();

        private bool lastState;

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("trigger", new pin { name = "trigger", description = "trigger to show notifier", direction = pinDirection.input });

            return pinList;
        }

        public override void evaluate()
        {
            bool newState = (bool)pinStates["input1"];

            if (newState != lastState && newState == true)
                showIt();

            lastState = newState;
        }

        private System.Threading.Timer myTimer = null;

        public override System.Windows.Forms.ContextMenuStrip addMenus(System.Windows.Forms.ContextMenuStrip toAddTo)
        {
            ToolStripItem optionsItem = new ToolStripMenuItem("&Options");
            optionsItem.Click += showOptionsDialog;
            toAddTo.Items.Add(optionsItem);

            return base.addMenus(toAddTo);
        }

        private void showOptionsDialog(object sender, EventArgs e)
        {
            FrmDesktopMessageOptions myOptForm = new FrmDesktopMessageOptions();
            myOptForm.options = myOptions;
            myOptForm.ShowDialog();
        }

        public void showIt()
        {
            frmDesktopMessage messageForm = new frmDesktopMessage(myOptions, "Test message");
            messageForm.showIt();
        }

    }

    public class desktopMessageOptions
    {
        // These are in hundreds of milliseconds
        public int fadeInSpeed = 5;
        public int holdSpeed = 40;
        public int fadeOutSpeed = 5;        
    }
}
