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
        public Options MyOptions;

        public FrmGeneralOptions()
        {
            InitializeComponent();
            cancelled = false;
            if ( null == MyOptions )
                MyOptions = new Options();
        }

        public FrmGeneralOptions(Options oldOptions)
        {
            InitializeComponent();
            cancelled = false;
            MyOptions = oldOptions;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DestroyHandle();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
            DestroyHandle();
        }

        private void FrmGeneralOptions_Load(object sender, EventArgs e)
        {
            cboPort.Text = MyOptions.portname;
        }

        private void cboPort_TextChanged(object sender, EventArgs e)
        {
            MyOptions.portname = ((ComboBox)sender).Text;
        }

    }
}