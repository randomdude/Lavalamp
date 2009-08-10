using System;
using System.Windows.Forms;
using System.Collections.Generic;
using netbridge;

namespace netGui
{
    public partial class FrmMain : Form
    {
        private transmitterDriver _mydriver = null;   
        public Options MyOptions = new Options();
        public  List<Node> NodeList = new List<Node>();

        public transmitterDriver myDriver
        {
            get 
            {
                return _mydriver; 
            }
            set 
            {
                _mydriver = value; 
            }
        }

        public FrmMain()
        {
            InitializeComponent();
        }


        private void MnuItemConnectToTrans_Click(object sender, EventArgs e)
        {
            try
            {
                myDriver = new transmitterDriver(MyOptions.portname);
            } catch (badPortException)
            {
                MessageBox.Show("Bad port name");
                return;
            } catch (cantOpenPortException)
            {
                MessageBox.Show("Can't open port, please make sure it is valid and unused");
                return;
            }

            MessageBox.Show("Port opened.");
            MnuItemConnectToTrans.Enabled = false;
            MnuItemDisconnectFromTrans.Enabled = true;
        }

        private void MnuItemDisconnectFromTrans_Click(object sender, EventArgs e)
        {
            myDriver.Dispose();
            MessageBox.Show("Port closed.");
            MnuItemConnectToTrans.Enabled = true;
            MnuItemDisconnectFromTrans.Enabled = false;
        }

        private void cmnuAddNode_Click(object sender, EventArgs e)
        {
            FrmAddNode AddNodeForm = new FrmAddNode();

            AddNodeForm.ShowDialog(this);
            if (!AddNodeForm.cancelled)
            {
                NodeList.Add(AddNodeForm.NewNode);
                lstNodes.Items.Add("wtf");
            }

            AddNodeForm.Dispose();
        }

        private void mnuItemGeneralOpts(object sender, EventArgs e)
        {
            Options tmpOptions = MyOptions.CopyOf;
            FrmGeneralOptions options = new FrmGeneralOptions(tmpOptions);
            options.ShowDialog(this);
            if (!options.cancelled)
                MyOptions = tmpOptions;         // If OK'ed, apply new settings

            options.Dispose();
        }

    }
}