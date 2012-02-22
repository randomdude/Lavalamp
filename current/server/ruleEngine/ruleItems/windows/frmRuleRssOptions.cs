using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.Win32;

namespace ruleEngine.ruleItems.windows
{
    internal partial class frmRuleRssOptions : Form
    {
        private rssOptions _options;
        internal frmRuleRssOptions(rssOptions options)
        {
            InitializeComponent();
            _options = options;
            
            txtFeed.Text = options.url;
            string error;
            if (!isVaildFeed(out error))
                lblWarning.Text = error;
        }

        internal rssOptions getOptions() { return _options;}
        /// <summary>
        /// vaildates that the file is a vaild feed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {
            string error;
            if (!isVaildFeed(out error))
                lblWarning.Text = error;
            else
                lblWarning.Text = "";
        }

        private bool isVaildFeed(out string error)
        {
            HttpWebRequest request;
            HttpWebResponse response;
             Uri url;
            if (!Uri.TryCreate(txtFeed.Text, UriKind.Absolute, out url))
            {
                error = "Invaild format of feed entered";
                return false;
            }

            XmlReader reader;
            error = "";
            switch (url.Scheme)
            {
                case "file":
                    if (!File.Exists(url.LocalPath))
                        error = "File doesn't currently exist";
                    reader = XmlReader.Create(File.OpenRead(url.LocalPath));
                    break;
                case "https":
                case "http":
                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    if (response.StatusCode != HttpStatusCode.OK || responseStream == null)
                    {
                        error = "Couldn't get stream";
                        return false; 
                    }
                    reader = XmlReader.Create(responseStream);
                    break;
                default:
                    throw new NotSupportedException(url.Scheme + " not currently supported by this reader");
            }
            try
            {

                SyndicationFeed feed = SyndicationFeed.Load(reader);
                if (feed == null)
                {
                    error = "Couldn't read the selected feed";
                    return false;
                }
                lblFeedTitle.Text = _options.title = feed.Title.Text;
                lblFeedDescription.Text = feed.Description.Text;
                lblFeedAuthors.Text = string.Join(", ", feed.Authors.Select(s => s.Name + " (" + s.Email + " )").ToArray());
                if (feed.Links.Count > 0)
                {
                    _options.website = feed.Links[0].Uri.ToString();
                }
                if(feed.ImageUrl != null)
                {
                    _options.imageUrl = feed.ImageUrl.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = "Couldn't read the selected feed";
                return false;
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string error;
            if (!isVaildFeed(out error))
            {
                if(DialogResult.No == MessageBox.Show(this, error + " do you want to continue?","Warning",MessageBoxButtons.YesNo))
                    return;

            }
            _options.url = txtFeed.Text;
            _options.title = lblFeedTitle.Text;
        }

        private void feedTitleClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(_options.website))
                    Process.Start(_options.website);
        }

       
    }
}
