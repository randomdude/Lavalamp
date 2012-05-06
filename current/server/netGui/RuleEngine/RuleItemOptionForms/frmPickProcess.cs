namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;
    using ruleEngine.ruleItems.windows;

    public partial class frmPickProcess : Form, IOptionForm
    {
        public bool cancelled = true;
        public string name = "";

        private PickProcessOptions opts;
        public frmPickProcess()
        {
            this.InitializeComponent();
            this.CenterToParent();
        }

        public frmPickProcess(IFormOptions proccessOpts)
        {
             opts = proccessOpts as PickProcessOptions;
            this.InitializeComponent();
            this.CenterToParent();
            this.cboProcessName.Text = this.name = opts.pName;

            // add running processes to process list
            foreach (Process thisProc in Process.GetProcesses())
                this.cboProcessName.Items.Add(thisProc.ProcessName);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.name = this.cboProcessName.Text;
            opts.setChanged();
            this.cancelled = false;
            this.Close();
        }

        public IFormOptions SelectedOptions()
        {
           return  opts;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
        }
    }
}
