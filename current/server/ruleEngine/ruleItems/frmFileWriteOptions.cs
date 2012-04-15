using System;
using System.IO;
using System.Windows.Forms;

namespace ruleEngine.ruleItems
{
    public partial class frmFileWriteOptions : Form
    {
        public frmFileWriteOptions(FileWriteOptions options)
        {
            InitializeComponent();
            cboFormat.DataSource = Enum.GetNames(typeof (FileFormat));
            cboFormat.SelectedItem = options.fileFormat.ToString();
            txtFileName.Text = options.filePath ?? "New Document.txt";
            txtPublishURI.Text = options.publishURI == null ? "" : options.publishURI.ToString();
            txtAdditonalData.Text = options.additionalInfo;
            txtDescription.Text = options.description;
            configureFormOpts(options.fileFormat);
        }

        private void configureFormOpts(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.Text:
                case FileFormat.Bin:
                    lblAdditionalTitle.Visible = false;
                    txtAdditonalData.Visible = false;
                    txtPublishURI.Visible = false;
                    lblPublishURI.Visible = false;
                    lblDescription.Visible = false;
                    txtDescription.Visible = false;
                    Height = 135;
                    break;
                case FileFormat.RSS:
                    Height = 311;
                    lblAdditionalTitle.Visible = true;
                    txtAdditonalData.Visible = true;
                    txtPublishURI.Visible = true;
                    lblPublishURI.Visible = true;
                    lblDescription.Visible = true;
                    txtDescription.Visible = true;
                    lblAdditionalTitle.Text = "Feed Title";
                    break;
            }
        }


        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtFileName.Text.Contains("."))
                txtFileName.Text = txtFileName.Text.Substring(0, txtFileName.Text.LastIndexOf('.'));
            FileFormat format = (FileFormat) Enum.Parse(typeof (FileFormat) , (string)cboFormat.SelectedValue);
            configureFormOpts(format);
            switch (format)
            {
                case FileFormat.RSS:
                    txtAdditonalData.Text = txtFileName.Text;
                    txtFileName.Text += ".rss";
                    Uri uri;
                    if(!Uri.TryCreate(txtFileName.Text,UriKind.RelativeOrAbsolute,out uri))
                        uri = new Uri(Directory.GetCurrentDirectory() + txtFileName.Text);
                    txtPublishURI.Text = uri.ToString();
                    break;
                case FileFormat.Text:
                    txtFileName.Text += ".txt";
                    break;
                case FileFormat.Bin:
                    txtFileName.Text += ".bin";
                    break;
            }
        }

        private void frmFileWriteOptions_FormClosing(object sender, FormClosingEventArgs e)
        {

         
        }

        public FileWriteOptions SelectedOptions{ get; set; }

        private void btnDir_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog {CheckFileExists = false , FileName = txtFileName.Text};
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtFileName.Text = dlg.FileName;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                MessageBox.Show(this, "Filename is required");
                return;
            }

            SelectedOptions = new FileWriteOptions()
            {
                additionalInfo = txtAdditonalData.Text,
                fileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), (string)cboFormat.SelectedValue),
                filePath = txtFileName.Text
            };

            if (txtAdditonalData.Visible)
            {
                if (string.IsNullOrEmpty(txtAdditonalData.Text))
                {
                    MessageBox.Show(this, "No publish uri set - required");
                    return;
                }
            }
            if (txtPublishURI.Visible)
            {
                if (string.IsNullOrEmpty(txtPublishURI.Text))
                {
                    MessageBox.Show(this, "No publish uri set - required");
                    return;
                }
                Uri uri;
                if (!Uri.TryCreate(txtPublishURI.Text,UriKind.RelativeOrAbsolute, out uri ))
                {
                    MessageBox.Show(this, "uri entered invaild");
                    return;
                }
                SelectedOptions.publishURI = uri;
            }
            Close();
        }
    }
}
