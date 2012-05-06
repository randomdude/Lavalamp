namespace netGui.RuleEngine
{
    using System;
    using System.Windows.Forms;

    public partial class frmException : Form
    {
        public frmException()
        {
            this.InitializeComponent();
        }

        public frmException(Exception why)
        {
            this.InitializeComponent();
            this.lblException.Text = why.Message;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
