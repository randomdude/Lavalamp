namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;
    using ruleEngine.ruleItems.Starts;

    public partial class frmCheckEmailOptions : Form, IOptionForm
    {
        public emailOptions options;

        private bool hasBeenTested = false;
        public bool cancelled = true;

        public frmCheckEmailOptions(IFormOptions newOptions)
        {
            this.InitializeComponent();
            this.CenterToParent();
            this.options = (emailOptions)newOptions;
        }

        private void chkSSL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSSL.Checked)
                this.txtPort.Text = "993";
            else
                this.txtPort.Text = "443";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!this.validateControls())
                return;

            try
            {
                imapChecker myChecker = new imapChecker(this.options);
                if (myChecker.newMail)
                    MessageBox.Show("Able to connect to mail server OK (detected new mail).");
                else
                    MessageBox.Show("Able to connect to mail server OK (detected no new mail).");
                this.hasBeenTested = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to mail server! The problem reported was '" + ex.Message + "'.");
                this.hasBeenTested = false;
            }
        }

        private bool validateControls()
        {
            this.txtUsername.Text = this.txtUsername.Text.Trim();
            this.txtPort.Text = this.txtPort.Text.Trim();
            this.cmbServer.Text = this.cmbServer.Text.Trim();

            if (this.txtUsername.Text == "")
            {
                MessageBox.Show("You must supply a username.");
                return false;
            }
            if (this.txtPassword.Text == "")
            {
                MessageBox.Show("You must supply a password.");
                return false;
            }
            if (this.cmbServer.Text == "")
            {
                MessageBox.Show("You must specify the server to connect to.");
                return false;
            }
            this.options = new emailOptions();
            int.TryParse(this.txtPort.Text, out this.options.portNum);
            if (this.options.portNum == 0)
            {
                MessageBox.Show("You must specify the port to connect to numerically (normally 443, or 993 if using SSL).");
                return false;
            }

            this.options.username = this.txtUsername.Text;
            this.options.password = this.txtPassword.Text;
            this.options.serverName = this.cmbServer.Text;
            this.options.useSSL = this.chkSSL.Checked;

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.validateControls())
                return;

            if (!this.hasBeenTested)
            {
                if (MessageBox.Show("You have not tested these settings sucessfully; testing is recommended. Do you want to continue anyway?", "Are you sure?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            this.DialogResult = DialogResult.OK;
            this.cancelled = false;
            options.setChanged();
            this.Close();
        }

        public IFormOptions SelectedOptions()
        {
            return options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
