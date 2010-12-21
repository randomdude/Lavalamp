using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems
{
    public partial class frmRunExeOptions : Form
    {
        public string filename;
        public string username;
        public string password;
        public bool impersonate;
        public ProcessWindowStyle windowStye;
        public Icon pictureIcon = null;

        public frmRunExeOptions()
        {
            InitializeComponent();
        }

        public frmRunExeOptions(string username, string password, bool impersonate, ProcessWindowStyle style)
        {
            InitializeComponent();

            this.chkImpersonate.Checked = impersonate;
            if (impersonate)
            {
                txtUsername.Text = username;
                txtPassword1.Text = password.ToString();
                txtPassword2.Text = password.ToString();
            }

            foreach (String enumString in Enum.GetNames(typeof(ProcessWindowStyle)))
                cmbWindowStyle.Items.Add(enumString);

            cmbWindowStyle.Text = Enum.GetName(typeof(ProcessWindowStyle), style); 
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (chkImpersonate.Checked)
            {
                if (txtUsername.Text.Trim() == "")
                {
                    MessageBox.Show(
                        "Please enter a username to run the program as, or clear the 'run as Different user' checkbox if not desired.");
                    return;
                }
                if (txtPassword1.Text != txtPassword2.Text)
                {
                    MessageBox.Show(
                        "The two passwords supplied are not the same. Please enter the same password twice, once in each box.");
                    return;
                }

                this.username = txtUsername.Text;
                this.password  = txtPassword1.Text;
            }

            this.impersonate = chkImpersonate.Checked;

            this.windowStye = (ProcessWindowStyle) Enum.Parse(typeof(ProcessWindowStyle), cmbWindowStyle.Text);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void chkImpersonate_CheckedChanged(object sender, EventArgs e)
        {
            this.txtUsername.Enabled = chkImpersonate.Checked;
            this.txtPassword1.Enabled = chkImpersonate.Checked;
            this.txtPassword2.Enabled = chkImpersonate.Checked;

            this.cmbWindowStyle.Enabled = !chkImpersonate.Checked;
        }

    }
}
