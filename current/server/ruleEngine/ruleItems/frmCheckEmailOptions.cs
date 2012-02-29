using System;
using System.Windows.Forms;
using ruleEngine.ruleItems.Starts;

namespace ruleEngine.ruleItems
{
    public partial class frmCheckEmailOptions : Form
    {
        public emailOptions options;

        private bool hasBeenTested = false;
        public bool cancelled = true;

        public frmCheckEmailOptions(emailOptions newOptions)
        {
            InitializeComponent();
            CenterToParent();
            options = newOptions;
        }

        private void chkSSL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSSL.Checked)
                txtPort.Text = "993";
            else
                txtPort.Text = "443";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!this.validateControls())
                return;

            try
            {
                imapChecker myChecker = new imapChecker(options);
                if (myChecker.newMail)
                    MessageBox.Show("Able to connect to mail server OK (detected new mail).");
                else
                    MessageBox.Show("Able to connect to mail server OK (detected no new mail).");
                hasBeenTested = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to mail server! The problem reported was '" + ex.Message + "'.");
                hasBeenTested = false;
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
            options = new emailOptions();
            int.TryParse(this.txtPort.Text, out options.portNum);
            if (options.portNum == 0)
            {
                MessageBox.Show("You must specify the port to connect to numerically (normally 443, or 993 if using SSL).");
                return false;
            }

            options.username = txtUsername.Text;
            options.password = txtPassword.Text;
            options.serverName = cmbServer.Text;
            options.useSSL = chkSSL.Checked;

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!validateControls())
                return;

            if (!hasBeenTested)
            {
                if (MessageBox.Show("You have not tested these settings sucessfully; testing is recommended. Do you want to continue anyway?", "Are you sure?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            DialogResult = DialogResult.OK;
            cancelled = false;

            this.Close();
        }
    }
}
