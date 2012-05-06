namespace netGui
{
    using System;
    using System.Windows.Forms;
    using ruleEngine.nodes;

    public partial class FrmAddNode : Form
    {
        public FrmAddNode()
        {
            InitializeComponent();
        }

        public bool cancelled = true;
        public Node newNode = null;

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            Int16? parsedNodeId = null;

            try
            {
                parsedNodeId  = (Int16)Int32.Parse(txtId.Text);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FormatException)  ||
                    ex.GetType() == typeof(OverflowException) )
                {   
                    parsedNodeId  = null;       // simply an error parsing.
                }
                else
                {
                    throw (ex);
                }
            }

            if (!parsedNodeId.HasValue ||
                 parsedNodeId  > 255    ||
                 parsedNodeId  < 1        )
            {
                MessageBox.Show("The new node ID must be a numeric value, in the range of 1-255.");
                txtId.Focus();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.newNode = new Node((Int16)Int32.Parse(txtId.Text));

            this.cancelled = false;
            this.Close();
        }

    }
}
