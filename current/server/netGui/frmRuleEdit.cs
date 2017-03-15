
namespace netGui
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using ruleEngine;
    using ruleEngine.ruleItems;

    public partial class frmRuleEdit : Form
    {
        private bool isClosing = false;

        public delegate void saveRuleDelegate(rule saveThis);
        public delegate void closeRuleDelegate(rule closeThis);

        public saveRuleDelegate saveCallback;
        public closeRuleDelegate closeCallback;

        public frmRuleEdit()
        {
            this.InitializeComponent();
        }

        public frmRuleEdit(saveRuleDelegate onSaveRule, closeRuleDelegate onCloseRuleEditorDialog)
        {
            this.saveCallback = onSaveRule;
            this.closeCallback = onCloseRuleEditorDialog;
            this.InitializeComponent();
        }

        private void frmRuleEdit_Load(object sender, EventArgs e)
        {
            this.populateToolboxFromAssembly(Assembly.GetAssembly(typeof(ToolboxRule)));
            this.tvToolbox.Sort();
            this.GiveFeedback += this.OnGiveFeedback;

        }

        private void OnGiveFeedback(object sender , GiveFeedbackEventArgs giveFeedbackEventArgs)
        {
            giveFeedbackEventArgs.UseDefaultCursors = giveFeedbackEventArgs.Effect != DragDropEffects.Copy;
        }


        private void populateToolboxFromPythonFile(string filename)
        {
            try
            {
                ruleItem_script jake = new ruleItem_script(filename);

                ruleItemInfo itemInfo = new ruleItemInfo
                    {
                        itemType = ruleItemType.scriptFile,
                        pythonFileName = filename,
                        pythonCategory = jake.getCategory()
                    };

                this.addRuleItemObjectToToolbox(jake, itemInfo);
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
                    if (thisType.IsDefined(typeof(ToolboxRule),false))
                    {
                        try
                        {
                            // Instantiate a new object just so we can pluck the name from it
                            ConstructorInfo constr = thisType.GetConstructor(new Type[0]);
                            Object newRuleItem = constr.Invoke(new object[0]);
                             ruleItemInfo itemInfo = new ruleItemInfo();

                            itemInfo.itemType = ruleItemType.RuleItem;
                            itemInfo.ruleItemBaseType = thisType;

                            this.addRuleItemObjectToToolbox((ruleItemBase) newRuleItem, itemInfo);
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
            TreeNode newTreeItem = new TreeNode((newRuleItem).ruleName()) { Tag = itemInfo };

            // check if rule item is already added.
            // TODO Strong names
            foreach(TreeNode n in tvToolbox.Nodes)
            {
                if (n.Text == (newRuleItem).ruleName())
                    return;
                foreach (TreeNode child in n.Nodes)
                {
                    if (child.Text == (newRuleItem).ruleName())
                        return;
                }

            }

            string catName;
            if (itemInfo.itemType == ruleItemType.RuleItem && itemInfo.ruleItemBaseType.IsDefined(typeof(ToolboxRuleCategoryAttribute), false))
            {
                Object[] attrs = itemInfo.ruleItemBaseType.GetCustomAttributes(typeof(ToolboxRuleCategoryAttribute), false);
                catName = ((ToolboxRuleCategoryAttribute) attrs[0]).name;
            }
            else
            {
                catName = "";
            }
            if (itemInfo.itemType == ruleItemType.scriptFile)
                catName = itemInfo.pythonCategory;


            if (catName != "")
            {
                bool foundIt = false;
                foreach (TreeNode cat in this.tvToolbox.Nodes)
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
                    this.tvToolbox.Nodes.Add(daddy);
                }
            }
            else
            {
                this.tvToolbox.Nodes.Add(newTreeItem);
            }
        }

        private void tvToolbox_DoubleClick(object sender, EventArgs e)
        {
            if ( (((TreeView)sender).SelectedNode ==null) || ((TreeView)sender).SelectedNode.Tag == null)
                return; // this will happen for category headers

            // ask the rule control to add an item
            this.ctlRuleEditor.addRuleItem(((ruleItemInfo)((TreeView)sender).SelectedNode.Tag));
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.startRule();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.stopRule();
        }

        public void stopRule()
        {
            this.btnStop.Enabled = false;
            this.btnRun.Enabled = true;
            this.ctlRuleEditor.stop();
        }

        public void startRule()
        {
            this.btnStop.Enabled = true;
            this.btnRun.Enabled = false;
            this.ctlRuleEditor.start();
        }

        private void saveRule()
        {
            this.stopRule();

            if (this.saveCallback == null)
            {
                MessageBox.Show("Unable to save rule!");
                return;
            }
            rule r = this.ctlRuleEditor.getRule();
            r.preferredHeight = this.Height;
            r.preferredWidth = this.Width;
            //String serialised = ctlRule1.serialiseRule();
            //Clipboard.SetText(serialised);
            this.saveCallback.Invoke(r);
        }

        private void loadFromnetAssemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = true,
                    Title = "Locate assembly to import",
                    Filter = "All loadable files (*.dll, *.py)|*.dll; *.py|DLL files|*.dll|All files|*.*"
                };
            if (DialogResult.OK == dlg.ShowDialog())
            {
                foreach (String thisFile in dlg.FileNames)
                {
                    if (thisFile.EndsWith("pyc",true,CultureInfo.CurrentCulture) ||
                        thisFile.EndsWith("py",true,CultureInfo.CurrentCulture))
                    {
                        try
                        {
                            this.populateToolboxFromPythonFile(thisFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to load python from file (" + ex.Message + ")");
                        }
                    }
                    else
                    {
                        try
                        {
                             this.populateToolboxFromAssembly(Assembly.LoadFile(thisFile));
                        }
                        catch (Exception ex)
                        {
                                MessageBox.Show("Unable to load assembly from file (" + ex.Message + ")");
                        }
                    }
                }
                this.tvToolbox.Sort();
            }
        }

        public void loadRule(string serialisedData)
        {
            string utf8Xml = Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(serialisedData));
            StringReader myReader = new StringReader(utf8Xml);

            XmlSerializer mySer = new XmlSerializer(typeof(rule));
            this.ctlRuleEditor.onRuleItemLoaded += this.loadAssemblyForRuleItem;
            rule loadingRule = (rule) mySer.Deserialize(myReader);
            this.ctlRuleEditor.loadRule(loadingRule);
            this.Text += " - " + loadingRule.name;
        }

        private void loadAssemblyForRuleItem(ruleItemBase item)
        {
            if (item.GetType().Assembly.FullName != Assembly.GetExecutingAssembly().FullName)
            {
                this.populateToolboxFromAssembly(Assembly.Load(item.GetType().Assembly.FullName));
                this.tvToolbox.Refresh();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("This will destroy any unsaved changes! Are you sure?", "Really close rule?", MessageBoxButtons.YesNo);

            if (response == DialogResult.No)
                return;

            this.closeRule();
        }

        /// <summary>
        /// Close this dialog, after firing the events as necessary.
        /// </summary>
        private void closeRule()
        {
            if (this.ctlRuleEditor != null)
                this.closeCallback.Invoke(this.ctlRuleEditor.getRule());

            this.stopRule();

            this.isClosing = true;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.saveRule();
            MessageBox.Show("Rule saved OK");            
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            this.saveRule();

            this.closeRule();
        }

        private void frmRuleEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isClosing)
                return;

            DialogResult response = MessageBox.Show("Save changes to rule?", "Save changes?", MessageBoxButtons.YesNoCancel);

            if (response == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            if (response == DialogResult.Yes)
                this.saveRule();

            this.closeRule();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.ctlRuleEditor.advanceDelta();
        }

        private void snapToGridToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.ctlRuleEditor.snapAllToGrid();
        }

        private void alwaysSnapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.alwaysSnapToGridToolStripMenuItem.Checked = !this.alwaysSnapToGridToolStripMenuItem.Checked;

            this.ctlRuleEditor.setGridSnapping(this.alwaysSnapToGridToolStripMenuItem.Checked);
        }



        private void tvToolbox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView view = ((TreeView)sender);
            TreeNode dragNode = (TreeNode)e.Item;
            if (dragNode.Tag == null)
                return; // this will happen for category headers

            ruleItemInfo info = (ruleItemInfo)dragNode.Tag;
            view.SelectedNode = dragNode;
            this.DoDragDrop(new DataObject(info) , DragDropEffects.Copy);
            
        }



    }
}
