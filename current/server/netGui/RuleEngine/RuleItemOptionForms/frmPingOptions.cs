using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui.RuleEngine.RuleItemOptionForms
{
    using netGui.RuleItemOptionForms;

    using ruleEngine.ruleItems;

    public partial class frmPingOptions : Form, IOptionForm
    {
        private pingOptions options;
        public frmPingOptions()
        {
            InitializeComponent();
        }


        public frmPingOptions(IFormOptions opts)
        {
            InitializeComponent();
            options = (pingOptions)opts;
            txtAddress.Text = options.addressToPing;
            cboInfo.Items.AddRange(Enum.GetNames(typeof(pingOptions.PingInfomation)).Select(i => (object)i ).ToArray());
            cboInfo.SelectedItem = options.pingInfo;
            
        }


        public IFormOptions SelectedOptions()
        {
            return options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            options.addressToPing = txtAddress.Text;
            options.pingInfo = (pingOptions.PingInfomation)Enum.Parse(typeof(pingOptions.PingInfomation), cboInfo.SelectedItem.ToString());
            options.setChanged();
            this.Close();
        }
    }
}
