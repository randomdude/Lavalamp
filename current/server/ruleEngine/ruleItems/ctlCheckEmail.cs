using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.Properties;

namespace ruleEngine.ruleItems
{
    public partial class ctlCheckEmail : UserControl
    {
        public emailOptions options = new emailOptions();

        public ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal;

        public ctlCheckEmail()
        {
            InitializeComponent();
            commonConstructorStuff();
        }

        private void commonConstructorStuff()
        {
            this.pictureIcon.Image = Resources.email.ToBitmap();
        }

        public ctlCheckEmail(emailOptions newOptions)
        {
            InitializeComponent();
            commonConstructorStuff();
            this.options = newOptions;
        }

        public ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while (mnuParent.Items.Count > 0)
                toRet.Items.Add(mnuParent.Items[0]);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            while (contextMenuStrip1.Items.Count > 0)
                toRet.Items.Add(contextMenuStrip1.Items[0]);

            return toRet;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCheckEmailOptions optionsForm = new frmCheckEmailOptions(options);

            DialogResult res = optionsForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                options = optionsForm.options;
            }
        }

    }

    [Serializable]
    public class emailOptions
    {
        [XmlElement]
        public string serverName = "imap.gmail.com";
        [XmlElement]
        public int portNum = 993;
        [XmlElement]
        public bool useSSL = true; 
        [XmlElement]
        public string username = "username";
        //todo: crypt password
        [XmlElement]
        public string password = "password";
           
    }
}