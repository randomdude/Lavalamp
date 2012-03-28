using System;
using System.Management;
using System.Windows.Forms;

namespace ruleEngine.ruleItems.windows.WMI
{
    public partial class frmWMIOptions : Form
    {
        private WMIOptions _options;
        public frmWMIOptions(WMIOptions options)
        {
            InitializeComponent();
           
            cboComputer.Text = options.computer;
            txtUsername.Text = options.username;
            txtPassword.Text = options.password;
            Closing += frmWMIOptions_Closing;
            _options = (WMIOptions) options.Clone();
            _options.onScopeOptsChanged += getControls;

        }

        private void getControls(ManagementScope conn)
        {
            tblCustom.Controls.Clear();
            try
            {
                Control[] ctls = _options.getCustomControls();

                int ctlnum = 0;
                if (ctls.Length > 0)
                {
                    for (int i = 0; i < ctls.Length / 2; i++)
                    {
                        tblCustom.Controls.Add(ctls[ctlnum++], 0, i);
                        tblCustom.Controls.Add(ctls[ctlnum++], 1, i);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Could not get WMI options for this rule item on " + _options.computer, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void frmWMIOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cboComputer.Text == "")
                cboComputer.Text = "localhost";
            _options.onScopeOptsChanged -= getControls;
            _options.computer = cboComputer.Text;
            if (checkBox1.Checked)
            {
                _options.username = txtUsername.Text;
                _options.password = txtPassword.Text;
            }
            else
            {
                _options.username = "";
            }
            _options.setCustomValues();

        }
        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            authBox.Enabled = checkBox1.Checked;
            if (!checkBox1.Checked)
            {
                _options.username = "";
                _options.password = "";
                _options.InvokeScopeOptionsChanged(sender,e);
            }

        }

        private void frmWMIOptions_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.username))
                 authBox.Enabled = checkBox1.Checked = true;
            getControls(null);
            //TODO get networked computers and load them into the combobox too.

        }


        public WMIOptions SelectedOptions()
        {
            return _options;
        }

        private void cboComputer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboComputer.Text == "")
                cboComputer.Text = "localhost";
            _options.computer = cboComputer.Text;
            _options.InvokeScopeOptionsChanged(sender , e);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show(this, "Username is blank or not set");
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show(this, "Password is blank or not set");
                return;
            }

            _options.password = txtPassword.Text;
            _options.username = txtUsername.Text;
            _options.InvokeScopeOptionsChanged(sender,e);
        }
    }
}
