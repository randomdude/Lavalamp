using System;
using System.Windows.Forms;

namespace netGui.RuleEngine
{
    public partial class FrmAskName : Form
    {
        public String result;
        public bool cancelled;

        public FrmAskName()
        {
            commonConstructorStuff();
        }

        public FrmAskName(string caption)
        {
            commonConstructorStuff();
            this.lblCaption.Text = caption;
        }

        public FrmAskName(string caption, string text)
        {
            commonConstructorStuff();
            this.lblCaption.Text = caption;
            this.textBox1.Text = text;
        }

        public void commonConstructorStuff()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            result = this.textBox1.Text;
            cancelled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
        }
    }
}
