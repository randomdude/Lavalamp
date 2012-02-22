using System;
using System.Collections.Generic;
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

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            strip1.Items.Add("&Options", null, (sender, e) => ruleItemOptions().ShowDialog());
            return base.addMenus(strip1);
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

        private void loadRuleItemDetails(rssOptions options)
        {
            string feedText = String.IsNullOrEmpty(_options.title) ? "feed" : _options.title;
            if (_lblFeedTitle.Text == feedText)
                return;
            _lblFeedTitle.Text = feedText;
            _lastRead = DateTime.MinValue;
            if (!String.IsNullOrEmpty(_options.imageUrl))
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(_options.imageUrl));
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                _imgFeed.Image = Image.FromStream(responseStream);
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
                             description = "connect to feed for title of the currently read feed",
                             direction = pinDirection.output,
                             valueType = typeof(pinDataTypes.pinDataString),

                         });
            pins.Add("feedContent",
                     new pin
                     {
                         name = "feed Content",
                         description = "connect to feed for the content of the currently read feed",
                         direction = pinDirection.output,
                         valueType = typeof(pinDataTypes.pinDataString),

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
                    _imgFeed.Image = SetImageToInvaild(_imgFeed.Image);
                    throw new WebException("Cannot access feed");
                }
            
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(feedStream));
                // order by ascending date all the feed items which have not been seen or have been updated and return the first.
                SyndicationItem feedItem = feed.Items.Where(f => f.PublishDate > _lastRead || f.LastUpdatedTime > _lastRead)
                                                     .OrderBy(f => f.LastUpdatedTime).ThenBy(f => f.PublishDate)
                                                     .FirstOrDefault();
                // if null no new feed items are present
                if (feedItem == null)
                {
                    _lastRead = DateTime.Now;
                    return;
                }
                //regex removes all html tags from the feed.
                Regex removeTags = new Regex("<(.|\n)*?>");
                _lastRead = feedItem.PublishDate.LocalDateTime;
                pinInfo["feedTitle"].value.data = removeTags.Replace(feedItem.Title.Text, string.Empty);
                pinInfo["feedContent"].value.data = removeTags.Replace(feedItem.Summary.Text, string.Empty);
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["feedTitle"].value as pinDataString)));
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["feedContent"].value as pinDataString)));
         
        }
        catch (Exception e)
        {
            _imgFeed.Image = SetImageToInvaild(_imgFeed.Image);
            errorHandler(e);
            return;
        }

     }

        public Bitmap SetImageToInvaild(Image i)
        {
            Bitmap Bmp = i as Bitmap;
           
            int rgb;
            Color c;

            for (int y = 0; y < Bmp.Height; y++)
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)((c.R + c.G + c.B) / 3);
                    Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return Bmp;

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
