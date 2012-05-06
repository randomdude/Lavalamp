namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public partial class frmFileWriteOptions : Form, IOptionForm
    {
        public frmFileWriteOptions(IFormOptions opts)
        {
            FileWriteOptions options = (FileWriteOptions)opts;
            this.InitializeComponent();
            this.cboFormat.DataSource = Enum.GetNames(typeof (FileFormat));
            this.cboFormat.SelectedItem = options.fileFormat.ToString();
            this.txtFileName.Text = options.filePath ?? "New Document.txt";
            this.txtPublishURI.Text = options.publishURI == null ? "" : options.publishURI.ToString();
            this.txtAdditonalData.Text = options.additionalInfo;
            this.txtDescription.Text = options.description;
            this.configureFormOpts(options.fileFormat);
        }

        private void configureFormOpts(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.Text:
                case FileFormat.Bin:
                    this.lblAdditionalTitle.Visible = false;
                    this.txtAdditonalData.Visible = false;
                    this.txtPublishURI.Visible = false;
                    this.lblPublishURI.Visible = false;
                    this.lblDescription.Visible = false;
                    this.txtDescription.Visible = false;
                    this.Height = 135;
                    break;
                case FileFormat.RSS:
                    this.Height = 311;
                    this.lblAdditionalTitle.Visible = true;
                    this.txtAdditonalData.Visible = true;
                    this.txtPublishURI.Visible = true;
                    this.lblPublishURI.Visible = true;
                    this.lblDescription.Visible = true;
                    this.txtDescription.Visible = true;
                    this.lblAdditionalTitle.Text = "Feed Title";
                    break;
            }
        }


        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtFileName.Text.Contains("."))
                this.txtFileName.Text = this.txtFileName.Text.Substring(0, this.txtFileName.Text.LastIndexOf('.'));
            FileFormat format = (FileFormat) Enum.Parse(typeof (FileFormat) , (string)this.cboFormat.SelectedValue);
            this.configureFormOpts(format);
            switch (format)
            {
                case FileFormat.RSS:
                    this.txtAdditonalData.Text = this.txtFileName.Text;
                    this.txtFileName.Text += ".rss";
                    Uri uri;
                    if(!Uri.TryCreate(this.txtFileName.Text,UriKind.RelativeOrAbsolute,out uri))
                        uri = new Uri(Directory.GetCurrentDirectory() + this.txtFileName.Text);
                    this.txtPublishURI.Text = uri.ToString();
                    break;
                case FileFormat.Text:
                    this.txtFileName.Text += ".txt";
                    break;
                case FileFormat.Bin:
                    this.txtFileName.Text += ".bin";
                    break;
            }
        }
        public FileWriteOptions SelectedOptions{ get; set; }

        private void btnDir_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog {CheckFileExists = false , FileName = this.txtFileName.Text};
            if (dlg.ShowDialog(this) == DialogResult.OK)
                this.txtFileName.Text = dlg.FileName;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFileName.Text))
            {
                MessageBox.Show(this, "Filename is required");
                return;
            }

            this.SelectedOptions = new FileWriteOptions()
            {
                additionalInfo = this.txtAdditonalData.Text,
                fileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), (string)this.cboFormat.SelectedValue),
                filePath = this.txtFileName.Text
            };

            if (this.txtAdditonalData.Visible)
            {
                if (string.IsNullOrEmpty(this.txtAdditonalData.Text))
                {
                    MessageBox.Show(this, "No publish uri set - required");
                    return;
                }
            }
            if (this.txtPublishURI.Visible)
            {
                if (string.IsNullOrEmpty(this.txtPublishURI.Text))
                {
                    MessageBox.Show(this, "No publish uri set - required");
                    return;
                }
                Uri uri;
                if (!Uri.TryCreate(this.txtPublishURI.Text,UriKind.RelativeOrAbsolute, out uri ))
                {
                    MessageBox.Show(this, "uri entered invaild");
                    return;
                }
                this.SelectedOptions.publishURI = uri;
            }
            SelectedOptions.setChanged();
            this.Close();
        }

        IFormOptions IOptionForm.SelectedOptions()
        {
            return SelectedOptions;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {

        }
    }
}
