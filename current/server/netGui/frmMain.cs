using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using netGui.nodeEditForms;
using ruleEngine;
using transmitterDriver;

namespace netGui
{
    public partial class FrmMain : Form
    {
        private ITransmitter _mydriver = null;
        private options MyOptions = new options();
      //  private List<ruleItemCustom> _customToolbox = new List<ruleItemCustom>();
 
        public ITransmitter getMyDriver()
        {
            if ( (null == _mydriver) || (!_mydriver.portOpen()) )
            {
                DialogResult response =
                    MessageBox.Show("To do this, the transmitter must be connected. Connect to transmitter now?",
                                    "Connect to transmitter?", MessageBoxButtons.YesNo);
                if ((response == DialogResult.No) || (response == DialogResult.None))
                    throw new cantOpenPortException();

                _mydriver = new _transmitter(MyOptions.portname, MyOptions.useEncryption, MyOptions.myKey.keyArray);
                generalToolStripMenuItem.Enabled = false;
            } 

            return _mydriver;  
        }

        public FrmMain()
        {
            InitializeComponent();
        }

        #region node interaction
        public void setMyDriver(ITransmitter toThis)
        {
            _mydriver = toThis;
        }

        public void connectToTransmitter()
        {
            try
            {
                setMyDriver(new _transmitter(MyOptions.portname, MyOptions.useEncryption, MyOptions.myKey.keyArray));
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
            generalToolStripMenuItem.Enabled = false;
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
            generalToolStripMenuItem.Enabled = true;
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
            newNode.OwnerWindow = this;
            // Connect to node and fill fields
            try
            {
                newNode.Mydriver = getMyDriver();
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
                MessageBox.Show("An internal error occurred when attempting to talk to the node. Please retry.");
                return;
            }

            // Add node to node list
            String id = newNode.id.ToString();
            String caption = newNode.name + " (id " + id + ")"; 
            String name = newNode.name;
            String sensorCount = newNode.sensors.Count.ToString();

            ListViewItem listItem = new ListViewItem(new[] { caption, id, name, sensorCount  }, 0);
            listItem.ImageIndex = 0;
            listItem.Tag = newNode;

            lstNodes.Items.Add(listItem);            
        }

        private void mnuItemGeneralOpts(object sender, EventArgs e)
        {
            options tmpOptions = new options(MyOptions);
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
            Close();
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
            saveAllRules();
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
            ruleEngine.frmRuleEdit editor = new ruleEngine.frmRuleEdit(onSaveRule, onCloseRuleEditorDialog);
            editor.Show();
        }

        #endregion

        #region rule stuff

        private void newRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (true)
            {
                frmQuestion newname = new frmQuestion("New rule name", "UnnamedRule");

                if (newname.ShowDialog(this) == DialogResult.Cancel)
                    return;

                if (findRuleItem(newname.result) != null)
                {
                    MessageBox.Show("A rule with that name already exists");
                }
                else
                {
                    addNewRule(new rule(newname.result));
                    return;
                }
            }

        }

        private void addNewRule(rule toAdd)
        {
            // if the rule is toolbox promoted it shouldn't appear in the list of runnable rules instead add it to the custom toolbox list
            // which will be added to the rule editors box
            if (toAdd.toolboxPromoted)
            {
             //   _customToolbox.Add(new ruleItemCustom(toAdd));
            }
            else
            {
                // Create a new rule, and add it to our listView.
                // Add the new rule's state and name as columns, and the rule object itself as a tag.
                ListViewItem newItem = new ListViewItem();

                toAdd.onStatusUpdate += updateRuleIcon;
                newItem.SubItems.Add(toAdd.name);
                newItem.SubItems.Add(false.ToString());
                newItem.Tag = toAdd;

                lstRules.Items.Add(newItem);

                updateRuleIcon(toAdd);
            }

        }

        private void deleteRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count < 1)
                return;
            ListViewItem toRemove = lstRules.SelectedItems[0];
            rule r = (rule) toRemove.Tag;
            DialogResult sureness = MessageBox.Show("Are you sure you want to delete rule '" + r.name + "'?", "Confirm delete", MessageBoxButtons.YesNo);

            if (sureness == System.Windows.Forms.DialogResult.Yes)
                lstRules.Items.Remove(toRemove);
            if(File.Exists(MyOptions.rulesPath + @"\" + r.name + ".rule"))
                File.Delete(MyOptions.rulesPath + @"\" + r.name + ".rule");
        }

        private void lstRules_ItemActivate(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count == 0 ) return;

            editRuleItem( lstRules.SelectedItems[0] );
        }

        private void editRuleItem(ListViewItem ruleItem)
        {
            if (ruleItem.SubItems[2].Text == true.ToString() )
            {
                // todo: being open rule editor window to foregroound
                MessageBox.Show("This rule is already open.");
                return;
            }

            rule rule = (rule) ruleItem.Tag;

            frmRuleEdit newForm = new frmRuleEdit(onSaveRule, onCloseRuleEditorDialog);
            // We serialise the rule before we pass it to the rule edit form. This is to ease the transition
            // to a client-server style rule engine / rule editor kind of situations later on
            newForm.loadRule(rule.serialise());
            newForm.ctlRuleEditor.getRule().onStatusUpdate += updateRuleIcon;
            newForm.Show();

            // Mark this rule as being open in the editor
            ruleItem.SubItems[2].Text = true.ToString();
        }

        private void onCloseRuleEditorDialog(rule closeThis)
        {
            // Flag the rule as no longer open in an editor.
            // Find the Rule in the listView
            ListViewItem ruleItem = findRuleItem(closeThis);
            if (ruleItem == null)
            {
                MessageBox.Show("Unable to mark rule as not-being-edited - can't find it in listView control");
                return;
            }

            // mark the listView item as not being open in the editor any more
            ruleItem.SubItems[2].Text = false.ToString();
        }

        private void onSaveRule(rule saveThis)
        {
            // Find the Rule in the listView
            ListViewItem ruleItem = findRuleItem(saveThis);
            if (ruleItem == null)
            {
                DialogResult result;
                frmQuestion frm = new frmQuestion("New Rule");
                if ((result = frm.ShowDialog(this)) != DialogResult.OK)
                {
                    MessageBox.Show("Unable to save rule");
                    return;
                }
                while (findRuleItem(frm.result) != null && result == DialogResult.OK)
                    result = frm.ShowDialog(this);
                if (result == DialogResult.Cancel)
                    return;
                saveThis.name = frm.result;
                addNewRule(saveThis);
            }

            // mark the listView item as not being open in the editor any more
            ruleItem.SubItems[2].Text = false.ToString();

            // Stash our rule object in the listViewItem.
            ruleItem.Tag = saveThis;
        }


        private ListViewItem findRuleItem(string findName)
        {
            // Pull item out of listView
            // todo: Is there a better way of doing this?
            foreach (ListViewItem thisListViewItem in lstRules.Items)
            {
                if (thisListViewItem.SubItems[1].Text == findName)
                    return thisListViewItem;
            }
            return null;
        }

        private ListViewItem findRuleItem(rule toFind)
        {
            // Pull item out of listView
            // todo: Is there a better way of doing this?
            foreach (ListViewItem thisListViewItem in lstRules.Items)
            {
                if (thisListViewItem.SubItems[1].Text == toFind.name)
                    return thisListViewItem;
            }
            return null;
        }

        /// <summary>
        /// Update a rows 'status' and status icon
        /// </summary>
        /// <param name="toUpdate">The rule to update</param>
        private void updateRuleIcon(rule toUpdate)
        {
            ListViewItem itemToUpdate = findRuleItem(toUpdate);

            if (itemToUpdate == null)
            {
                MessageBox.Show("Unable to update rule '" + toUpdate.name + "'");
                return;
            }

            itemToUpdate.Text = toUpdate.state.ToString();
            switch (toUpdate.state)
            {
                case ruleState.stopped:
                    itemToUpdate.ImageKey = "Pause.bmp";
                    break;
                case ruleState.running:
                    itemToUpdate.ImageKey = "Run.bmp";
                    break;
                case ruleState.errored:
                    itemToUpdate.ImageKey = "Critical.bmp";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid rule state");
            }
        }

        private void saveAllRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAllRules();
        }

        private void loadAllRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadAllRules();
        }

        private void loadAllRules()
        {
            DirectoryInfo rulesDir;
            FileInfo[] fileList;
            try
            {
                rulesDir = new DirectoryInfo(MyOptions.rulesPath);
                fileList = rulesDir.GetFiles();
            } catch {
                MessageBox.Show("Unable to read rule files from " + MyOptions.rulesPath);
                return;
            }

            lstRules.Items.Clear();
            foreach (FileInfo thisFile in fileList)
            {
                try
                {
                    StreamReader thisFileReader;
                    XmlSerializer mySer = new XmlSerializer(typeof(rule));
                    using (thisFileReader = new StreamReader(thisFile.FullName))
                    {
                        // Add our  rule name to our listView, with a .tag() set to the rule object itself.
                         addNewRule((rule)mySer.Deserialize(thisFileReader));
                    }
                } catch {
                    MessageBox.Show("Unable to read rule file '" + thisFile.FullName + "'" );
                }
            }

        }
        #endregion

        private void runRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count == 0)
                return;

            foreach (ListViewItem thisItemToStart in lstRules.SelectedItems)
            {
                rule thisRuleToStart = (rule) ((ListViewItem)thisItemToStart).Tag;
                if (thisItemToStart.SubItems[2].Text == true.ToString())
                {
                    MessageBox.Show("Cannot start rule '" + thisRuleToStart.name + "' - rule is open in editor.");
                }
                else
                {
                    thisRuleToStart.start();
                    updateRuleIcon(thisRuleToStart);
                }
            }
        }

        private void stopRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count == 0)
                return;

            foreach (ListViewItem thisItemToStart in lstRules.SelectedItems)
            {
                rule thisRuleToStop = (rule)((ListViewItem)thisItemToStart).Tag;
                if (thisItemToStart.SubItems[2].Text == true.ToString())
                { 
                    MessageBox.Show("Cannot stop rule '" + thisRuleToStop.name + "' - rule is open in editor.");
                }
                else
                {
                    thisRuleToStop.stop();
                    updateRuleIcon(thisRuleToStop);
                }
            }

        }

        public void saveAllRules()
        {     
            // Check we can get to the output path, to avoid throwing a messageBox for each failed file if we can't
            DirectoryInfo rulesDir;
            try
            {
                rulesDir = new DirectoryInfo(MyOptions.rulesPath);
                rulesDir.GetFiles();
            }
            catch
            {
                MessageBox.Show("Unable to read rule files from " + MyOptions.rulesPath);
                return;
            }

            // Now save each rule in turn.
            foreach (ListViewItem thisItem in lstRules.Items)
            {
                try
                {
                    rule thisRule = (rule)thisItem.Tag;
                    thisRule.saveToDisk(MyOptions.rulesPath + @"\" + thisRule.name + ".rule");
                }
                catch
                {
                    MessageBox.Show("Unable to save rule file '" + thisItem.Text + "'");
                }
            }
            
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            loadAllRules();
        }

        private void editRuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRules.SelectedItems.Count == 0) return;

            editRuleItem(lstRules.SelectedItems[0]);
        }

    }
}