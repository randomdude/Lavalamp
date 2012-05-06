using System;
using System.Windows.Forms;

namespace ruleEngine
{
    public partial class frmQuestion : Form
    {
        public String result;

        public frmQuestion()
        {
            commonConstructorStuff();
        }

        public frmQuestion(string caption)
        {
            commonConstructorStuff();
            this.lblCaption.Text = caption;
        }

        public frmQuestion(string caption, string text)
        {
            commonConstructorStuff();
            this.lblCaption.Text = caption;
            this.textBox1.Text = text;
        }

        public void commonConstructorStuff()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            result = this.textBox1.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
