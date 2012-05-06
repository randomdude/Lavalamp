namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public partial class frmRunExeOptions : Form, IOptionForm
    {
       
        public Icon pictureIcon = null;
        RunExeOptions _options = new RunExeOptions(); 
        public frmRunExeOptions()
        {
            this.InitializeComponent();
            this.CenterToParent();
        }

        public frmRunExeOptions(IFormOptions opts)
        {
            _options = opts as RunExeOptions;
            this.InitializeComponent();
            this.CenterToParent();
            this.chkImpersonate.Checked = _options.doImpersonate;
            if (_options.doImpersonate)
            {
                this.txtUsername.Text = _options.username;
                this.txtPassword1.Text = _options.password.ToString();
                this.txtPassword2.Text = _options.password.ToString();
            }

            foreach (String enumString in Enum.GetNames(typeof(ProcessWindowStyle)))
                this.cmbWindowStyle.Items.Add(enumString);

            this.cmbWindowStyle.Text = Enum.GetName(typeof(ProcessWindowStyle), _options.windowStyle); 
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (this.chkImpersonate.Checked)
            {
                if (this.txtUsername.Text.Trim() == "")
                {
                    MessageBox.Show(
                        "Please enter a username to run the program as, or clear the 'run as Different user' checkbox if not desired.");
                    return;
                }
                if (this.txtPassword1.Text != this.txtPassword2.Text)
                {
                    MessageBox.Show(
                        "The two passwords supplied are not the same. Please enter the same password twice, once in each box.");
                    return;
                }

                _options.username = this.txtUsername.Text;
                _options.password = this.txtPassword1.Text;
            }

            _options.doImpersonate = this.chkImpersonate.Checked;

            _options.windowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), this.cmbWindowStyle.Text);
            _options.setChanged();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void chkImpersonate_CheckedChanged(object sender, EventArgs e)
        {
            this.txtUsername.Enabled = this.chkImpersonate.Checked;
            this.txtPassword1.Enabled = this.chkImpersonate.Checked;
            this.txtPassword2.Enabled = this.chkImpersonate.Checked;

            this.cmbWindowStyle.Enabled = !this.chkImpersonate.Checked;
        }

        public IFormOptions SelectedOptions()
        {
            return _options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
