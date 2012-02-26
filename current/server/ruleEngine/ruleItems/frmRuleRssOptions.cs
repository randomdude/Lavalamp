using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.ServiceModel.Syndication;

namespace ruleEngine.ruleItems.windows
{
    internal partial class frmRuleRssOptions : Form
    {
        private readonly rssOptions _options;
        internal frmRuleRssOptions(rssOptions options)
        {
            InitializeComponent();
            CenterToParent();
            _options = options;
            txtFeed.Text = options.url;

            //check if the user has entered a feed or not before checking if its valid.
            if (!string.IsNullOrEmpty(txtFeed.Text))
            {
                string error;
                if (!isVaildFeed(out error))
                    lblWarning.Text = error;
            }
        }

        internal rssOptions getOptions() { return _options; }
        
        private void btnCheck_Click(object sender, EventArgs e)
        {
            string error;
            lblWarning.Text = !isVaildFeed(out error) ? error : "";
        }

        /// <summary>
        /// Checks that the address of the field entered is a valid RSS feed
        /// </summary>
        /// <param name="error">A human readable error message</param>
        /// <returns>TRUE if there is an error, FALSE if not</returns>
        private bool isVaildFeed(out string error)
        {
            Uri url;
            XmlReader reader = null;
            StreamReader responseStream = null;
            error = "";

            if (!Uri.TryCreate(txtFeed.Text, UriKind.Absolute, out url))
            {
                error = "Invalid format of feed entered";
                return false;
            }

            try
            {
                switch (url.Scheme)
                {
                    case "file":
                        if (!File.Exists(url.LocalPath))
                            error = "File doesn't currently exist";
                        reader = XmlReader.Create(File.OpenRead(url.LocalPath));
                        break;
                    case "https":
                    case "http":
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                            responseStream = new StreamReader(response.GetResponseStream());
                        else
                        {
                            error = "Couldn't get stream";
                            return false;
                        }
                        reader = XmlReader.Create(responseStream);
                        break;
                    default:
                        error = url.Scheme + " not currently supported by this reader";
                        return false;
                }

                SyndicationFeed feed = SyndicationFeed.Load(reader);
                if (feed == null)
                {
                    error = "Couldn't read the selected feed";
                    return false;
                }
                lblFeedTitle.Text = _options.title = feed.Title.Text;
                lblFeedDescription.Text = feed.Description.Text;

                List<string> authorArray = new List<string>();
                foreach (SyndicationPerson author in feed.Authors)
                {
                    if (!String.IsNullOrEmpty(author.Name) &&
                        !String.IsNullOrEmpty(author.Email)   )
                    {
                        authorArray.Add(author.Name + " (" + author.Email + " )");
                    }
                    else if (!String.IsNullOrEmpty(author.Name))
                    {
                        authorArray.Add(author.Name);
                    }
                    else if (!String.IsNullOrEmpty(author.Email))
                    {
                        authorArray.Add(author.Email);
                    }
                    else
                    {
                        authorArray.Add("unknown");
                    }
                }

                lblFeedAuthors.Text = string.Join(", ", authorArray.ToArray());
                if (feed.Links.Count > 0)
                    _options.website = feed.Links[0].Uri.ToString();

                return true;
            }
            catch (Exception)
            {
                error = "Couldn't read the selected feed";
                return false;
            }
            finally
            {
                if (reader != null) 
                    reader.Close();
                if (responseStream != null)
                    responseStream.Close();
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string error;
            if (!isVaildFeed(out error))
            {
                if (DialogResult.No == MessageBox.Show(this, error + " do you want to continue?", "Warning", MessageBoxButtons.YesNo))
                {
                    lblWarning.Text = error;
                    return;
                }
            }
            _options.url = txtFeed.Text;
            _options.title = lblFeedTitle.Text;
        }
        /// <summary>
        /// Opens the feed when the user clicks the feed title in the options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedTitleClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(_options.website))
                    Process.Start(_options.website);
        }
    }
}
