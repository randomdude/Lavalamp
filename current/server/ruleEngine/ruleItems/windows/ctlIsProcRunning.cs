using System;
using System.Windows.Forms;
using ruleEngine;

namespace ruleEngine.ruleItems.windows
{
    public partial class ctlIsProcRunning : UserControl
    {
        private string _processName;
        public String processName { get { return _processName; } set { _processName = value; this.lbl.Text = "Is process '" + _processName + "' running?"; } }

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
            frmQuestion question = new frmQuestion("Name of process to check for:", processName);

            if (question.ShowDialog() == DialogResult.Cancel)
                return;

            processName = question.result;
        }
    }
}