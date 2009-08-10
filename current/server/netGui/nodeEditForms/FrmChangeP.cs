using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui.nodeEditForms
{
    public partial class FrmChangeP : Form
    {
        private Node toChange;

        public FrmChangeP(Node target)
        {
            InitializeComponent();
            this.lblNodeName.Text = target.name;
            toChange = target;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    toChange.doSetNodeP(key.parseKey( txtNewP.Text  ));
                    MessageBox.Show("P changed OK");
                    this.Close();
                } catch (FormatException)
                {
                    MessageBox.Show("Please enter a new value of 6 characters long, containing only the digits 0 through 9 and A through F.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
