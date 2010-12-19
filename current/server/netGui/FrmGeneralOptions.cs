using System;
using System.Windows.Forms;

namespace netGui
{
    public partial class FrmGeneralOptions : Form
    {
        public bool cancelled;
        public options MyOptions;

        public FrmGeneralOptions()
        {
            InitializeComponent();
            cancelled = false;
            if ( null == MyOptions )
                MyOptions = new options();

            txtKey.Enabled = chkUseEncryption.Checked;
        }

        public FrmGeneralOptions(options oldOptions)
        {
            InitializeComponent();
            cancelled = false;
            MyOptions = oldOptions;
            txtKey.Enabled = chkUseEncryption.Checked;
        }


        private void cmdOK_Click(object sender, EventArgs e)
        {
            MyOptions.portname = cboPort.Text;
            MyOptions.rulesPath = txtRulePath.Text;
            MyOptions.useEncryption = chkUseEncryption.Checked;

            try
            {
                MyOptions.myKey.setKey(txtKey.Text);
                DestroyHandle();
            } catch (FormatException) {
                MessageBox.Show(this, "Your network key must be a 32-character value, containing values from zero through nine, and 'A' through 'F'.");
                return;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
            DestroyHandle();
        }

        private void FrmGeneralOptions_Load(object sender, EventArgs e)
        {
            cboPort.Text = MyOptions.portname;
            txtKey.Text = MyOptions.myKey.ToString();
            chkUseEncryption.Checked = MyOptions.useEncryption;
        }

        private void chkUseEncryption_CheckedChanged(object sender, EventArgs e)
        {
            this.txtKey.Enabled = chkUseEncryption.Checked;
        }
    }
}