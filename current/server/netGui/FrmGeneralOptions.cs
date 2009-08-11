using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
        }

        public FrmGeneralOptions(options oldOptions)
        {
            InitializeComponent();
            cancelled = false;
            MyOptions = oldOptions;
        }


        private void cmdOK_Click(object sender, EventArgs e)
        {
            MyOptions.portname = cboPort.Text;
            MyOptions.rulesPath = txtRulePath.Text;

            try
            {
                MyOptions.myKey.setKey(txtKey.Text);
                DestroyHandle();
            } catch (FormatException) {
                MessageBox.Show("Your network key must be a 32-character value, containing values from zero through nine, and 'A' through 'F'.");
                return;
            }
            MessageBox.Show("Please note that some changes will not take effect until the port is next re-opened.");

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
        }
    }
}