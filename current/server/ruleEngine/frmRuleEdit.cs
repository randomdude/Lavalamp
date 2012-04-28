using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    public partial class frmRuleEdit : Form
    {
        private bool isClosing = false;

        public delegate void saveRuleDelegate(rule saveThis);
        public delegate void closeRuleDelegate(rule closeThis);

        public saveRuleDelegate saveCallback;
        public closeRuleDelegate closeCallback;

        public frmRuleEdit()
        {
            InitializeComponent();
        }

        public frmRuleEdit(saveRuleDelegate onSaveRule, closeRuleDelegate onCloseRuleEditorDialog)
        {
            saveCallback = onSaveRule;
            closeCallback = onCloseRuleEditorDialog;
            InitializeComponent();
        }

        private void frmRuleEdit_Load(object sender, EventArgs e)
        {
            populateToolboxFromAssembly(Assembly.GetExecutingAssembly());
            tvToolbox.Sort();
            GiveFeedback += OnGiveFeedback;

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

                ruleItemInfo itemInfo = new ruleItemInfo();
                itemInfo.itemType = ruleItemType.scriptFile;
                itemInfo.pythonFileName = filename;
                itemInfo.pythonCategory = jake.getCategory();

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
                            itemInfo.ruleItemBaseType = thisType;

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
            ctlRuleEditor.addRuleItem(((ruleItemInfo)((TreeView)sender).SelectedNode.Tag));
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            startRule();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopRule();
        }

        public void stopRule()
        {
            btnStop.Enabled = false;
            btnRun.Enabled = true;
            ctlRuleEditor.stop();
        }

        public void startRule()
        {
            btnStop.Enabled = true;
            btnRun.Enabled = false;
            ctlRuleEditor.start();
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
                rule r = ctlRuleEditor.getRule();
                r.preferredHeight = Height;
                r.preferredWidth = Width;
                //String serialised = ctlRule1.serialiseRule();
                //Clipboard.SetText(serialised);
                saveCallback.Invoke(r);
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
                    if (thisFile.EndsWith("pyc",true,CultureInfo.CurrentCulture) ||
                        thisFile.EndsWith("py",true,CultureInfo.CurrentCulture))
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
                    else
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
                }
                tvToolbox.Sort();
            }
        }

        public void loadRule(string serialisedData)
        {
            string utf8Xml = Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(serialisedData));
            StringReader myReader = new StringReader(utf8Xml);

            XmlSerializer mySer = new XmlSerializer(typeof(rule));
            ctlRuleEditor.onRuleItemLoaded += loadAssemblyForRuleItem;
            rule loadingRule = (rule) mySer.Deserialize(myReader);
            ctlRuleEditor.loadRule(loadingRule);
            this.Text += " - " + loadingRule.name;
        }

        private void loadAssemblyForRuleItem(ruleItemBase item)
        {
            if (item.GetType().Assembly.FullName != Assembly.GetExecutingAssembly().FullName)
            {
                populateToolboxFromAssembly(Assembly.Load(item.GetType().Assembly.FullName));
                tvToolbox.Refresh();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("This will destroy any unsaved changes! Are you sure?", "Really close rule?", MessageBoxButtons.YesNo);

            if (response == DialogResult.No)
                return;

            closeRule();
        }

        /// <summary>
        /// Close this dialog, after firing the events as neccesary.
        /// </summary>
        private void closeRule()
        {
            if (ctlRuleEditor != null)
                closeCallback.Invoke(ctlRuleEditor.getRule());

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

            closeRule();
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

            closeRule();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ctlRuleEditor.advanceDelta();
        }

        private void snapToGridToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ctlRuleEditor.snapAllToGrid();
        }

        private void alwaysSnapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alwaysSnapToGridToolStripMenuItem.Checked = !alwaysSnapToGridToolStripMenuItem.Checked;

            ctlRuleEditor.setGridSnapping(alwaysSnapToGridToolStripMenuItem.Checked);
        }



        private void tvToolbox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView view = ((TreeView)sender);
            TreeNode dragNode = (TreeNode)e.Item;
            if (dragNode.Tag == null)
                return; // this will happen for category headers

            ruleItemInfo info = (ruleItemInfo)dragNode.Tag;
            view.SelectedNode = dragNode;
            DoDragDrop(new DataObject(info) , DragDropEffects.Copy);
            
        }



    }
}
