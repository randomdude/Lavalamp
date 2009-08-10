using System;
using System.Windows.Forms;

namespace netGui.RuleEngine
{
    public partial class frmException : Form
    {
        public frmException()
        {
            InitializeComponent();
        }

        public frmException(Exception why)
        {
            InitializeComponent();
            lblException.Text = why.Message;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
