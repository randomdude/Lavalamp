﻿using System;
using System.Windows.Forms;
using ruleEngine.nodes;

namespace netGui.nodeEditForms
{
    public partial class FrmChangeKey : Form
    {
        private Node toChange;

        public FrmChangeKey(Node target)
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
                key parsed = new key();
                try
                {
                    parsed.setKey(this.txtNewKey.Text);
                    toChange.doSetNodeKey(parsed);
                    MessageBox.Show("Node key has been changed. Note that this node will be inaccesible under the old key. You must change the network key in the Options menu to access this node.");
                    this.Close();
                } catch (FormatException)
                {
                    MessageBox.Show("Please enter a network key of 32 characters long, containing only the digits 0 through 9 and A through F.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
