using System;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems.windows
{
    public partial class ctlIsProcRunning : UserControl
    {
        public String processName = "";

        public ctlIsProcRunning()
        {
            InitializeComponent();
        }

        public ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while (mnuParent.Items.Count > 0)
                toRet.Items.Add(mnuParent.Items[0]);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            while (contextMenuStrip1.Items.Count > 0)
                toRet.Items.Add(contextMenuStrip1.Items[0]);

            return toRet;
        }

        private void setprocessNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAskName askName = new FrmAskName("Name of process to check for:", processName);

            askName.ShowDialog();

            if (askName.cancelled)
                return;

            this.lbl.Text = "Is process '" + askName.result + "' running?";
            processName = askName.result;
        }
    }
}