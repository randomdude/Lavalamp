namespace netGui.RuleItemOptionForms
{
    using System;
    using System.Management;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;
    using ruleEngine.ruleItems.windows.WMI;

    public partial class frmWMIOptions : Form, IOptionForm
    {
        private WMIOptions _options;
        public frmWMIOptions(IFormOptions options)
        {
            this.InitializeComponent();
            _options = (WMIOptions)options;
            this.cboComputer.Text = _options.computer;
            this.txtUsername.Text = _options.username;
            this.txtPassword.Text = _options.password;
            this.Closing += this.formClosing;
            this._options = (WMIOptions)_options.Clone();
            this._options.onScopeOptsChanged += this.getControls;

        }

        private void getControls(ManagementScope conn)
        {
            this.tblCustom.Controls.Clear();
            try
            {
                Control[] ctls = this._options.getCustomControls();

                int ctlnum = 0;
                if (ctls.Length > 0)
                {
                    for (int i = 0; i < ctls.Length / 2; i++)
                    {
                        this.tblCustom.Controls.Add(ctls[ctlnum++], 0, i);
                        this.tblCustom.Controls.Add(ctls[ctlnum++], 1, i);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Could not get WMI options for this rule item on " + this._options.computer, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void formClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
                return;

            if (this.cboComputer.Text == "")
                this.cboComputer.Text = "localhost";
            this._options.onScopeOptsChanged -= this.getControls;
            this._options.computer = this.cboComputer.Text;
            if (this.checkBox1.Checked)
            {
                this._options.username = this.txtUsername.Text;
                this._options.password = this.txtPassword.Text;
            }
            else
            {
                this._options.username = "";
            }
            this._options.setCustomValues();
            _options.setChanged();
        }
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.authBox.Enabled = this.checkBox1.Checked;
            if (!this.checkBox1.Checked)
            {
                this._options.username = "";
                this._options.password = "";
                this._options.InvokeScopeOptionsChanged(sender,e);
            }

        }

        private void frmWMIOptions_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._options.username))
                 this.authBox.Enabled = this.checkBox1.Checked = true;
            this.getControls(null);
            //TODO get networked computers and load them into the combobox too.

        }


        public IFormOptions SelectedOptions()
        {
            return this._options;
        }

        private void cboComputer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboComputer.Text == "")
                this.cboComputer.Text = "localhost";
            this._options.computer = this.cboComputer.Text;
            this._options.InvokeScopeOptionsChanged(sender , e);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (this.txtUsername.Text == "")
            {
                MessageBox.Show(this, "Username is blank or not set");
                return;
            }
            if (this.txtPassword.Text == "")
            {
                MessageBox.Show(this, "Password is blank or not set");
                return;
            }

            this._options.password = this.txtPassword.Text;
            this._options.username = this.txtUsername.Text;
            this._options.InvokeScopeOptionsChanged(sender,e);
        }
    }
}
