using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
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

        [XmlElement("lastRead")]
        public DateTime _lastRead;

        private Label _lblFeedTitle;
        private PictureBox _imgFeed;
        private Dictionary<string,bool> _readFeedItems = new Dictionary<string,bool>();
        

        public ruleItemRss()
        {
            _imgFeed = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(39, 33), Location = new Point(19, 3),Margin = new Padding(3,3,3,3)};
            controls.Add(_imgFeed);
            _lblFeedTitle = new Label(){Size = new Size(73, 37),Location = new Point(3, 39),TextAlign = ContentAlignment.MiddleCenter};
            controls.Add(_lblFeedTitle);
            loadRuleItemDetails(_options);
        }
        public override string ruleName()
        {
            return "Feed Reader";
        }

        public override Form ruleItemOptions()
        {
            frmRuleRssOptions options = new frmRuleRssOptions(_options);
            options.Closed += options_Closed;
            return options;
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
            _lastRead = DateTime.MinValue;
            _readFeedItems.Clear();
        }
        private void loadRuleItemDetails(rssOptions options)
        {
            string feedText = String.IsNullOrEmpty(_options.title) ? "RSS Feed Reader" : _options.title;
            if (_lblFeedTitle.Text == feedText)
                return;
            _lblFeedTitle.Text = feedText;
            _lastRead = DateTime.MinValue;
            if (!String.IsNullOrEmpty(_options.imageUrl))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(_options.imageUrl));
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if   (responseStream != null)
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
                _imgFeed.Image = Resources.rss;
            }
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string,pin> pins = new Dictionary<string, pin>();
            pins.Add("feedTitle",
                     new pin
                         {
                             name = "feed title",
                             description = "Title",
                             direction = pinDirection.output,
                             valueType = typeof(pinDataTypes.pinDataString)

                         });
            pins.Add("feedContent",
                     new pin
                     {
                         name = "feed Content",
                         description = "Content", 
                         direction = pinDirection.output,
                         valueType = typeof(pinDataTypes.pinDataString)
                     });
            pins.Add("trigger",
                     new pin
                         {
                             name = "trigger",
                             description = "set when you want to check for new info in your feed",
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_options.url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream feedStream = response.GetResponseStream();
                if (feedStream == null || response.StatusCode != HttpStatusCode.OK)
                {
                    _imgFeed.Image = setImageToInvaild(_imgFeed.Image);
                    throw new WebException("Cannot access feed");
                }
            
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(feedStream));
                // order by ascending date all the feed items which have not been seen or have been updated and return the first. 
                // If the feed items do not have dates in it we can't filter it
                SyndicationItem feedItem = feed.Items.Where(f => f.PublishDate > _lastRead || f.LastUpdatedTime > _lastRead ||
                                                           (f.PublishDate == DateTime.MinValue && f.LastUpdatedTime == DateTime.MinValue && !_readFeedItems.ContainsKey(f.Title.Text)  ))
                                                     .OrderBy(f => f.LastUpdatedTime).ThenBy(f => f.PublishDate)
                                                     .FirstOrDefault();
                // if null no new feed items are present
                if (feedItem == null)
                {
                    _lastRead = DateTime.Now;
                    return;
                }
                _readFeedItems.Add(feedItem.Title.Text,true);
                //regex removes all HTML tags from the feed.
                Regex removeTags = new Regex("<(.|\n)*?>");
                _lastRead = feedItem.PublishDate.LocalDateTime;
                pinInfo["feedTitle"].value.data = removeTags.Replace(feedItem.Title.Text, string.Empty);
                pinInfo["feedContent"].value.data = removeTags.Replace(feedItem.Summary.Text, string.Empty);
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["feedTitle"].value as pinDataString)));
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["feedContent"].value as pinDataString)));
         
        }
        catch (Exception e)
        {
            _imgFeed.Image = setImageToInvaild(_imgFeed.Image);
            errorHandler(e);
        }

     }

        public Bitmap setImageToInvaild(Image i)
        {
            Bitmap bmp = i as Bitmap;
            Debug.Assert(bmp != null, "bmp != null");
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int rgb = (c.R + c.G + c.B) / 3;
                    bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return bmp;
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
