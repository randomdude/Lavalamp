using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ruleEngine.ruleItems.windows.WMI
{
    public partial class frmWMIOptions : Form
    {
        private WMIOptions _options;
        public frmWMIOptions(WMIOptions options)
        {
            InitializeComponent();
            Control[] ctls = options.getCustomControls();
            int ctlnum = 0;
            if (ctls.Length > 0)
            {
                for (int i = 0; i < ctls.Length / 2; i++)
                {
                    tblCustom.Controls.Add(ctls[ctlnum++], 0, i);
                    tblCustom.Controls.Add(ctls[ctlnum++], 1, i);
                }
            }
            cboComputer.Text = options.computer;
            txtUsername.Text = options.username;
            _options = options;

            //TODO get networked computers and load them into the combobox too.
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            authBox.Enabled = checkBox1.Checked;
        }

        
    }
}
