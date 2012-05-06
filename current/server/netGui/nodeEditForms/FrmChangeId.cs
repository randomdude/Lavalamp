using System;
using System.Windows.Forms;
using ruleEngine.nodes;

namespace netGui.nodeEditForms
{
    public partial class FrmChangeId : Form
    {
        private Node toChange;

        public FrmChangeId(Node target)
        {
            InitializeComponent();
            this.lblNodeID.Text  = target.id.ToString();
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
                Int16 parsed = Int16.Parse(txtNewId.Text);
                if (parsed == 0)
                {
                    MessageBox.Show("You can't set a node to ID zero - this is reserved for the controller.");
                }
                else if (parsed > 255)
                {
                    MessageBox.Show("You can't set a node to ID greater than 255.");
                }
                else if (parsed < 0)
                {
                    MessageBox.Show("Please specify a numeric ID between 1 and 255.");
                }
                else
                {
                    toChange.doSetNodeId(parsed);
                    toChange.id = parsed;

                    this.Close();
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Please specify a numeric ID between 1 and 255.");
            }
        }
    }
}
