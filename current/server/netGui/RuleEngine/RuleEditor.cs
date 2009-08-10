using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public partial class frmRuleEdit : Form
    {
        private int ctlRule1BorderX;
        private int ctlRule1BorderY;
        private bool isClosing = false;

        public FrmMain.saveRuleDelegate saveCallback ;

        public frmRuleEdit()
        {
            InitializeComponent();
        }

        private void frmRuleEdit_Load(object sender, EventArgs e)
        {
            populateToolbox();
        }

        private void populateToolbox()
        {
            tvToolbox.Nodes.Clear();
            populateToolboxFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void populateToolboxFromPythonFile(string filename)
        {
            try
            {
                pythonEngine myEng = new pythonEngine(filename);
                ruleItem_python jake = new ruleItem_python(myEng);

                ruleItemInfo itemInfo = new ruleItemInfo();
                itemInfo.itemType = ruleItemType.PythonFile;
                itemInfo.pythonFileName = filename;
                itemInfo.pythonCategory = jake.category;

                addRuleItemObjectToToolbox(jake, itemInfo);
            }
            catch (Exception e)
            {
                // unable to load this file!
                MessageBox.Show("Unable to load file '" + filename +"', exception message: '" + e.Message + "'");
            }
        }

        private void populateToolboxFromAssembly(Assembly loadThis)
        {
            foreach (Module myMod in loadThis.GetModules())                 // pull modules out of myAss
            {
                foreach (Type thisType in myMod.GetTypes())                 // pull types out of the modules
                {
                    if (thisType.IsDefined(typeof (ToolboxRule), false))
                    {
                        try
                        {
                            // Instantiate a new object just so we can pluck the name from it
                            ConstructorInfo constr = thisType.GetConstructor(new Type[0]);
                            Object newRuleItem = constr.Invoke(new object[0]);

                            ruleItemInfo itemInfo = new ruleItemInfo();
                            itemInfo.itemType = ruleItemType.RuleItem;
                            itemInfo.RuleItemBaseType = thisType;

                            addRuleItemObjectToToolbox((ruleItemBase) newRuleItem, itemInfo);
                        }
                        catch (Exception e)
                        {
                            // unable to load this file!
                            MessageBox.Show("Unable to load ruleItem '" + thisType.Name + "' from assembly, exception message: '" + e.Message + "'");
                        }
                    }
                }
            }
        }

        private void addRuleItemObjectToToolbox(ruleItemBase newRuleItem, ruleItemInfo itemInfo)
        {
            TreeNode newTreeItem = new TreeNode((newRuleItem).ruleName());
            newTreeItem.Tag = itemInfo;

            string catName;
            if (itemInfo.itemType == ruleItemType.RuleItem && itemInfo.RuleItemBaseType.IsDefined(typeof(ToolboxRuleCategoryAttribute), false))
            {
                Object[] attrs = itemInfo.RuleItemBaseType.GetCustomAttributes(typeof(ToolboxRuleCategoryAttribute), false);
                catName = ((ToolboxRuleCategoryAttribute) attrs[0]).name;
            }
            else
            {
                catName = "";
            }
            if (itemInfo.itemType == ruleItemType.PythonFile)
                catName = itemInfo.pythonCategory;


            if (catName != "")
            {
                bool foundIt = false;
                foreach (TreeNode cat in tvToolbox.Nodes)
                {
                    if (cat.Text == catName)
                    {
                        cat.Nodes.Add(newTreeItem);
                        foundIt = true;
                        break;
                    }
                }
                if (!foundIt)
                {
                    TreeNode daddy = new TreeNode(catName);
                    daddy.Nodes.Add(newTreeItem);
                    tvToolbox.Nodes.Add(daddy);
                }
            }
            else
            {
                tvToolbox.Nodes.Add(newTreeItem);
            }
        }

        private void tvToolbox_DoubleClick(object sender, EventArgs e)
        {
            if ( (((TreeView)sender).SelectedNode ==null) || ((TreeView)sender).SelectedNode.Tag == null)
                return; // this will happen for category headers

            // ask the rule control to add an item
            ctlRule1.addRuleItem(((ruleItemInfo)((TreeView)sender).SelectedNode.Tag));
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnRun.Enabled = false;
            ctlRule1.start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopRule();
        }

        private void stopRule()
        {
            btnStop.Enabled = false;
            btnRun.Enabled = true;
            ctlRule1.stop();
        }

        private void saveRule()
        {
            stopRule();
            if (saveCallback == null)
            {
                MessageBox.Show("Unable to save rule!");
                return;
            }
            else
            {
                String serialised = ctlRule1.SerialiseRule();
                Clipboard.SetText(serialised);
                saveCallback.Invoke(ctlRule1.targetRule, serialised);
            }
        }

        private void loadFromnetAssemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Multiselect = true;
            dlg.Title = "Locate assembly to import";
            dlg.Filter = "All loadable files (*.dll, *.py)|*.dll; *.py|DLL files|*.dll|All files|*.*";
            if (DialogResult.OK == dlg.ShowDialog())
            {
                foreach (String thisFile in dlg.FileNames)
                {
                    if (thisFile.ToUpper().Substring(thisFile.Length - 3, 3) == "DLL")
                    {
                        try
                        {
                            populateToolboxFromAssembly(Assembly.LoadFile(thisFile));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to load assembly from file (" + ex.Message + ")");
                        }
                    }
                    else
                    {
                        try
                        {
                            populateToolboxFromPythonFile(thisFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to load python from file (" + ex.Message + ")");
                        }
                    }
                }
            }
        }

        public void loadRule(string serialisedData)
        {
            string utf8Xml = Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(serialisedData));
            StringReader myReader = new StringReader(utf8Xml);

            XmlSerializer mySer = new XmlSerializer(typeof(Rule));
            ctlRule1.targetRule = (Rule)mySer.Deserialize(myReader);
            ctlRule1.addRuleItemControlsAfterDeserialisation();
            ctlRule1.targetRule.state = ruleState.stopped;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("This will destroy any unsaved changes! Are you sure?", "Really close rule?", MessageBoxButtons.YesNo);

            if (response == DialogResult.No)
                return;

            stopRule();
            isClosing = true;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveRule();
            MessageBox.Show("Rule saved OK");            
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            saveRule();
            MessageBox.Show("Rule saved OK");
            isClosing = true;
            Close();
        }

        private void frmRuleEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosing)
                return;

            DialogResult response = MessageBox.Show("Save changes to rule?", "Save changes?", MessageBoxButtons.YesNoCancel);

            if (response == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }


            if (response == DialogResult.Yes)
                saveRule();

            stopRule();
        }

        private void frmRuleEdit_ResizeEnd(object sender, EventArgs e)
        {
            ctlRule1.Width = this.Width - ctlRule1.Left - ctlRule1BorderX;
            ctlRule1.Height = this.Height - ctlRule1.Top - ctlRule1BorderY;

            // We also set the toolbox to end at the same point as the rule control.
            tvToolbox.Height = (ctlRule1.Top + ctlRule1.Height) - tvToolbox.Top;
        }

        private void frmRuleEdit_ResizeBegin(object sender, EventArgs e)
        {
            ctlRule1BorderX = this.Width - (ctlRule1.Width + ctlRule1.Left);
            ctlRule1BorderY = this.Height - (ctlRule1.Height + ctlRule1.Top);
        }
    }
}
