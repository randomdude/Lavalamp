using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using netbridge;
using netGui.nodeEditForms;
using netGui.RuleEngine;

namespace netGui
{
    public partial class FrmMain : Form
    {
        private transmitterDriver _mydriver = null;   
        public Options MyOptions = new Options();
        public delegate void saveRuleDelegate(Rule saveThis, string serialisedRule);

        public transmitterDriver getMyDriver()
        {
            if ( (null == _mydriver) || (!_mydriver.portOpen()) )
            {
                DialogResult response =
                    MessageBox.Show("To do this, the transmitter must be connected. Connect to transmitter now?",
                                    "Connect to transmitter?", MessageBoxButtons.YesNo);
                if ((response == DialogResult.No) || (response == DialogResult.None))
                    throw new cantOpenPortException();

                _mydriver = new transmitterDriver(MyOptions.portname, MyOptions.myKey.keyArray);
            } 

            return _mydriver;  
        }

        public FrmMain()
        {
            InitializeComponent();

            // Initialise icons in the node list
            ImageList iconSmall = new ImageList();
            iconSmall.Images.Add(Properties.Resources.gearSmall );
            lstNodes.SmallImageList = iconSmall;

            ImageList iconLarge = new ImageList();
            iconSmall.Images.Add(Properties.Resources.gearSmall);
            lstNodes.LargeImageList = iconLarge;
        }


        #region node interaction
        public void setMyDriver(transmitterDriver toThis)
        {
            _mydriver = toThis;
        }

        public void connectToTransmitter()
        {
            try
            {
                setMyDriver(new transmitterDriver(MyOptions.portname, MyOptions.myKey.keyArray  ));
            }
            catch (badPortException)
            {
                MessageBox.Show("Bad port name");
                return;
            }
            catch (cantOpenPortException)
            {
                MessageBox.Show("Can't open port, please make sure it is valid and unused");
                return;
            }

            MessageBox.Show("Port opened.");
        }

        public void disconnectFromTransmitter()
        {
            try
            {
                if (null == getMyDriver())
                    return;
            }
            catch (cantOpenPortException)
            {
                return;
            }

            _mydriver.Dispose();
            _mydriver = null;
            lstNodes.Clear();
            MessageBox.Show("Port closed.");
        }

        private void MnuItemConnectToTrans_Click(object sender, EventArgs e)
        {
            if (_mydriver == null)
            {
                connectToTransmitter();
            }
            else
            {
                if (_mydriver.portOpen())
                    MessageBox.Show("Transmitter is already connected");
                else
                    connectToTransmitter();
            }
        }

        private void MnuItemDisconnectFromTrans_Click(object sender, EventArgs e)
        {
            disconnectFromTransmitter();
        }

        private void cmnuAddNode_Click(object sender, EventArgs e)
        {
            FrmAddNode AddNodeForm = new FrmAddNode();

            AddNodeForm.ShowDialog(this);
            if (!AddNodeForm.cancelled)
            {
                addNewNode(AddNodeForm.NewNode);
            }

            AddNodeForm.Dispose();
        }

        private void addNewNode(Node newNode)
        {
            newNode.ownerWindow = this;
            // Connect to node and fill fields
            try
            {
                newNode.mydriver = getMyDriver();
                newNode.fillProperties();
            }
            catch (badPortException)
            {
                MessageBox.Show("Bad port name");
                return;
            }
            catch (cantOpenPortException)
            {
                MessageBox.Show("Can't open port, please make sure it is valid and unused");
                return;
            }
            catch (commsCryptoException)
            {
                MessageBox.Show("Crypto failure talking to node. Please retry, and ensure network keys are set correctly.");
                return;
            }
            catch (commsTimeoutException )
            {
                MessageBox.Show("Timeout attempting to talk to node. Check node power and operation, and network keys.");
                return;
            }
            catch (InternalErrorException )
            {
                MessageBox.Show("An internal error occured when attempting to talk to the node. Please retry.");
                return;
            }

            // Add node to node list
            String id = newNode.id.ToString();
            String caption = newNode.name + " (id " + id + ")"; 
            String name = newNode.name;
            String sensorCount = newNode.sensors.Count.ToString();

            ListViewItem listItem = new ListViewItem(new[] { caption, id, name, sensorCount  }, 0);
            listItem.Tag = newNode;

            lstNodes.Items.Add(listItem);            
        }

        private void mnuItemGeneralOpts(object sender, EventArgs e)
        {
            Options tmpOptions = new Options(MyOptions);
            FrmGeneralOptions options = new FrmGeneralOptions(tmpOptions);
            options.ShowDialog(this);
            if (!options.cancelled)
            {
                MyOptions = tmpOptions; // If OK'ed, apply new settings
            }

            options.Dispose();
            this.BringToFront();
        }

        private void lstNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstNodes.SelectedItems.Count == 1)
                loadNodeInfoPanel((Node)lstNodes.SelectedItems[0].Tag);
        }

        private void loadNodeInfoPanel(Node loadThis)
        {
            lblNodeId.Text = loadThis.id.ToString();
            lblNodeName.Text = loadThis.name;
            lblSensorCount.Text = loadThis.sensors.Count.ToString();
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstNodes.View = View.Details;

            detailsToolStripMenuItem.Checked = true;
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = false;
        }

        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstNodes.View = View.LargeIcon ;

            detailsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem.Checked = true;
            smallIconsToolStripMenuItem.Checked = false;
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstNodes.View = View.SmallIcon;

            detailsToolStripMenuItem.Checked = false;
            largeIconsToolStripMenuItem.Checked = false;
            smallIconsToolStripMenuItem.Checked = true;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstNodes.SelectedItems.Count == 1)
                lstNodes.SelectedItems[0].Remove();
        }

        private void refreshThisNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem thisItem in  lstNodes.SelectedItems )
            {
                try
                {
                    ((Node) thisItem.Tag).fillProperties();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to communicate with node (" + ex.Message + ")");
                    thisItem.Remove();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void lstNodes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstNodes.SelectedItems.Count == 1)
            {
                frmNodeSensors sensorbar = new frmNodeSensors();
                sensorbar.Show();
                sensorbar.Visible = false;
                sensorbar.loadNode((Node)lstNodes.SelectedItems[0].Tag);
                sensorbar.Visible = true;
            }
        }

        private void lstNodes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == (char) 13 )
                lstNodes_MouseDoubleClick(sender, new MouseEventArgs(MouseButtons.XButton1,2,1,1,0));
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyOptions.save();
        }

        private void changeIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem thisItem in lstNodes.SelectedItems)
            {
                FrmChangeId idChangeForm = new FrmChangeId((Node)thisItem.Tag );
                idChangeForm.Show(this);
            }
        }

        private void changeKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem thisItem in lstNodes.SelectedItems)
            {
                FrmChangeKey keyChangeForm = new FrmChangeKey((Node)thisItem.Tag);
                keyChangeForm.txtNewKey.Text = MyOptions.myKey.ToString();
                keyChangeForm.Show(this);
            }
        }

        private void changePadvancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem thisItem in lstNodes.SelectedItems)
            {
                FrmChangeP pChangeForm = new FrmChangeP((Node)thisItem.Tag);
                pChangeForm.Show(this);
            }
        }

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void showRuleEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RuleEngine.frmRuleEdit editor = new RuleEngine.frmRuleEdit();
            editor.Show();
        }

        #endregion

        #region rule stuff

        private List<String> openRules = new List<String>();

        private void newRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAskName newname = new FrmAskName("New rule name", "UnnamedRule");
            newname.ShowDialog(this);

            if (newname.cancelled)
                return;

            if (lstRules.Items.ContainsKey(newname.result))
            {
                MessageBox.Show("A rule with that name already exists");
                return;
            }

            // Create a new rule
            ListViewItem newItem = new ListViewItem(newname.result);

            lstRules.Items.Add(newname.result, newname.result, 0);

            lstRules.Items[newname.result].Tag = new Rule(newname.result);
        }

        private void deleteRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem toRemove = lstRules.SelectedItems[0];
            DialogResult sureness = MessageBox.Show("Are you sure you want to delete rule '" + toRemove.Text + "'?");

            if (sureness == System.Windows.Forms.DialogResult.Yes)
                lstRules.Items.Remove(toRemove);
        }

        private void lstRules_ItemActivate(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count == 0 ) return;

            editRuleItem((Rule) lstRules.SelectedItems[0].Tag);
        }

        private void editRuleItem(Rule rule)
        {
            if (openRules.Contains(rule.name))
            {
                MessageBox.Show("This rule is already open.");
                return;
            }

            frmRuleEdit newForm = new frmRuleEdit();
            newForm.saveCallback = new saveRuleDelegate(saveRule);
            newForm.loadRule(rule.serialise());
            newForm.Show();
        }

        private void saveRule(Rule saveThis, string ruleSerialised)
        {
            openRules.Remove(saveThis.name);
            ListViewItem editedItem = lstRules.Items[saveThis.name];
            editedItem.Tag = saveThis;
        }

        #endregion

        private void saveAllRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem thisItem in lstRules.Items )
            {
                Rule thisRule = (Rule)thisItem.Tag;
                StreamWriter outputFile = new StreamWriter(MyOptions.rulesPath + @"\" + thisItem.Text);
                outputFile.Write(thisRule.serialise());
                outputFile.Close();
            }
        }

        private void loadAllRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInfo rulesDir = new DirectoryInfo(MyOptions.rulesPath);
            foreach (FileInfo thisFile in rulesDir.GetFiles())
            {
                StreamReader thisFileReader = new StreamReader(thisFile.FullName);
                // fixme/todo: fix this bodge!
                frmRuleEdit newEditor = new frmRuleEdit();
                newEditor.loadRule(thisFileReader.ReadToEnd());
                ListViewItem newItem = new ListViewItem(thisFile.Name, thisFile.Name);
                newItem.Tag = newEditor.ctlRule1.targetRule;
                lstRules.Clear();
                lstRules.Items.Add(newItem);
                thisFileReader.Close();
            }

        }

    }
}