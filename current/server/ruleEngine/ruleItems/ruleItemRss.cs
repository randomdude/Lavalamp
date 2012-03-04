using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ruleEngine.Properties;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Internet")]
    public class ruleItemRss : ruleItemBase
    {
        [XmlElement("rssOptions")]
        public rssOptions _options = new rssOptions();

        private Label _lblFeedTitle;
        private PictureBox _imgFeed;
        private Dictionary<string, bool> _readFeedItems = new Dictionary<string, bool>();

        public ruleItemRss()
        {
            _imgFeed = new PictureBox
                           {
                               SizeMode = PictureBoxSizeMode.StretchImage,
                               Size = new Size(39, 33),
                               Location = new Point((preferredSize().Width/2) - (39/2),
                                   (preferredSize().Height / 3) - (33 / 2)),
                               Margin = new Padding(3, 3, 3, 3)
                           };
            controls.Add(_imgFeed); 
            _lblFeedTitle = new Label()
            {
                Size = new Size( preferredSize().Width, preferredSize().Height - 15),
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.BottomCenter
            };
            controls.Add(_lblFeedTitle);
            loadRuleItemDetails(_options);
        }

        public override string ruleName()
        {
            return "RSS reader";
        }

        public override Form ruleItemOptions()
        {
            frmRuleRssOptions options = new frmRuleRssOptions(_options);
            options.Closed += options_Closed;
            return options;
        }

        public override Size preferredSize()
        {
            return new Size(150, 105);
        }

        void options_Closed(object sender, EventArgs e)
        {
            frmRuleRssOptions options = (frmRuleRssOptions) sender;

            if (options.DialogResult == DialogResult.OK)
            {
                loadRuleItemDetails(_options);
            }
        }

        public void resetReader()
        {
            _readFeedItems.Clear();
        }

        private void loadRuleItemDetails(rssOptions options)
        {
            string feedText = String.IsNullOrEmpty(_options.title) ? "RSS reader" : "RSS: " + _options.title;
            if (_lblFeedTitle.Text == feedText)
                return;
            _lblFeedTitle.Text = feedText;

            if (!String.IsNullOrEmpty(_options.imageUrl))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(_options.imageUrl));
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            _imgFeed.Image = Image.FromStream(responseStream);
                        else
                            errorHandler(new Exception("Invalid feed url"));
                    }
                }
                catch(Exception ex)
                {
                    errorHandler(ex);
                }
               
            }
            else
            {
                _imgFeed.Image = Resources.ruleItem_rss;
            }
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string,pin> pins = new Dictionary<string, pin>();
            pins.Add("feed Title",
                     new pin
                         {
                             name = "feed Title",
                             description = "Title",
                             direction = pinDirection.output,
                             valueType = typeof(pinDataString)

                         });
            pins.Add("feed Content",
                     new pin
                     {
                         name = "feed Content",
                         description = "Content", 
                         direction = pinDirection.output,
                         valueType = typeof(pinDataString)
                     });
            pins.Add("trigger",
                     new pin
                         {
                             name = "trigger",
                             description = "Poll",
                             direction = pinDirection.input
                         });
            return pins;
        }

        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;

            try
            {
                WebRequest request = WebRequest.Create(_options.url);
                WebResponse response = request.GetResponse();
                Stream feedStream = response.GetResponseStream();
                if (feedStream == null || response.ContentLength <= 0)
                    throw new WebException("Cannot access feed");
                if (response is HttpWebResponse)
                    if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                        throw new WebException("Cannot access feed (" + ((HttpWebResponse)response).StatusDescription + ")");

                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(feedStream));
                // order by ascending date all the feed items which have not been seen or have been updated and return the first. 
                // If the feed items do not have dates in it we can't filter it
                SyndicationItem feedItem =
                    feed.Items.Where(f => !_readFeedItems.ContainsKey(getItemID(f)))
                        .OrderBy(f => f.LastUpdatedTime).ThenBy(f => f.PublishDate)
                        .FirstOrDefault();
                // if null no new feed items are present
                if (feedItem == null)
                    return;

                _readFeedItems.Add(getItemID(feedItem), true);

                if (feedItem.Content != null || feedItem.Summary != null)
                {
                    string newSummary = getItemContent(feedItem);
                    Regex replace = new Regex("<foo type=\"[\\w]+\" xmlns=\"bar\">");
                    newSummary = replace.Replace(newSummary , string.Empty);
                    newSummary = newSummary.Replace("</foo>", "");
                    newSummary = unXMLEscape(newSummary);
                    onRequestNewTimelineEvent(
                        new timelineEventArgs(new pinDataString(newSummary, this, pinInfo["feed Content"])));
                }
                if (feedItem.Title != null)
                {
                    string newTitle = unXMLEscape(feedItem.Title.Text);

                    onRequestNewTimelineEvent(
                        new timelineEventArgs(new pinDataString(newTitle, this, pinInfo["feed Title"])));
                }
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
        }

        private string getItemContent(SyndicationItem feedItem)
        {
            // Since the only way to get to the 'content' node is to write it to an XMLWriter, we
            // end up doing this the long way - we write it, take it as text, then strip off the
            // XML tags we added when we first wrote. :-/
            //
            // TODO: What will happen if the feedItem.Content.Type isn't HTML? Surely this will
            // break.
            if (feedItem.Content != null)
            {
                TextWriter tw = new StringWriter();
                XmlWriter writer = new XmlTextWriter(tw);
                feedItem.Content.WriteTo(writer , "foo" , "bar");
                return tw.ToString();
            }
            else if (feedItem.Summary != null)
            {
                return feedItem.Summary.Text;
            }
            return null;
        }

        private string getItemID(SyndicationItem feedItem)
        {
            // We should return something which is unique for each feedItem. Some feeds provide an 'id' field, but
            // unfortunately some don't, so fall back on the 'content' data if it doesn't. If there's no content 
            // then try the summary, then the title.
            if (feedItem.Id != null)
                return feedItem.Id;

            if (feedItem.Content != null)
            {
                string content = getItemContent(feedItem);
                if (!String.IsNullOrEmpty(content))
                    return content;
            }

            if (feedItem.Summary != null)
                return feedItem.Summary.Text;

            if (feedItem.Title != null)
                return feedItem.Title.Text;

            throw new Exception("Unable to find item ID");
        }

        private string unXMLEscape(string data)
        {
            return HttpUtility.HtmlDecode(data);
        }
    }

    [Serializable]
    public class rssOptions
    {
        [XmlElement]
        public string imageUrl;
        [XmlElement]
        public string url { get; set; }
        [XmlElement]
        public string title { get; set;}
        [XmlElement]
        public string website { get; set; }
    }
}
