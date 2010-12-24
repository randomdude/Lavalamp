using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ruleEngine.ruleItems.itemControls
{
    public partial class ctlRunFile : UserControl
    {
        private Color normalBackground;
        public string filename = "";
        public string username = "";
        public string password;
        public ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal;
        public bool doImpersonate = false;

        private ruleItem_runexe.executeItNowDelegate executeIt = null;

        public ctlRunFile(ruleItem_runexe.executeItNowDelegate testFunc)
        {
            commonConstructorStuff();
            executeIt = testFunc;
        }

        public ctlRunFile()
        {
            commonConstructorStuff();
        }

        private void commonConstructorStuff()
        {
            InitializeComponent();
            normalBackground = this.BackColor;
            this.pictureIcon.Image = Properties.Resources.Shortcut.ToBitmap();
        }

        public ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while (mnuParent.Items.Count > 0)
                toRet.Items.Add(mnuParent.Items[0]);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            while(contextMenuStrip1.Items.Count > 0)
                toRet.Items.Add(contextMenuStrip1.Items[0]);

            return toRet;
        }

        private void browseToexecutableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            dlg.Title = "Locate file to run";
            dlg.Filter = "Executable files|*.exe; *.bat; *.com|All files|*.*";
            if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
            {
                this.filename = dlg.FileName;
                this.lbl.Text = dlg.SafeFileName;
                try
                {
                    Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(filename);
                    this.pictureIcon.Image = appIcon.ToBitmap() ;
                    this.pictureIcon.Size = appIcon.Size;
                } 
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load icon from file (" + ex.Message + ")");
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRunExeOptions optionsForm = new frmRunExeOptions(this.username, this.password,
                                                                this.doImpersonate, this.windowStyle);

            DialogResult res = optionsForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.username = optionsForm.username;
                this.password = optionsForm.password;
                this.doImpersonate = optionsForm.impersonate;
                this.windowStyle = optionsForm.windowStye;
                if (optionsForm.pictureIcon != null)
                    this.pictureIcon.Image = optionsForm.pictureIcon.ToBitmap();
            }
        }

        private void testByRunningSpecifiedApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != executeIt)
                executeIt.Invoke();
        }
    }
}
