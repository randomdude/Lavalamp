using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
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
    class ruleItemRss : ruleItemBase
    {
        
        [XmlElement("rssOptions")]
        rssOptions _options = new rssOptions();

        [XmlElement("lastRead")]
        DateTime _lastRead;

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

            _lblFeedTitle.Text = String.IsNullOrEmpty(_options.title) ? "feed" : _options.title;
            if (_options.image != null)
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_options.image);
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
            pins.Add("feed",
                     new pin
                         {
                             name = "feed",
                             description = "connect to feed for the rss",
                             direction = pinDirection.output,
                             valueType = typeof (pinDataTypes.pinDataString),
                            
                         });
            pins.Add("newData",
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
            if(!pinInfo["newData"].value.asBoolean())
                return;
            
            if (String.IsNullOrEmpty(_options.url))
            {
                _imgFeed.Image = SetImageToInvaild(_imgFeed.Image);
                return;
            }
               

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_options.url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream feedStream = response.GetResponseStream();
            if (feedStream == null || response.StatusCode != HttpStatusCode.OK)
            {
                _imgFeed.Image = SetImageToInvaild(_imgFeed.Image);
                return;
            }
            try
            {
                
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(feedStream));
                if (feed.LastUpdatedTime < _lastRead)
                {
                    _lastRead = DateTime.Now;
                    return;
                }
                StringBuilder rtn = new StringBuilder(feed.Items.Count(f => f.LastUpdatedTime > _lastRead) * 10);

                foreach (SyndicationItem feedItem in feed.Items)
                {
                    if (feedItem.LastUpdatedTime < _lastRead)
                        continue;
                    Regex removeTags = new Regex("<(.|\n)*?>");
                    rtn.Append(removeTags.Replace(feedItem.Title.Text,string.Empty) + removeTags.Replace(feedItem.Summary.Text,string.Empty));
                }
                _lastRead = DateTime.Now;
                pinInfo["feed"].value.data = rtn.ToString();
                onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(pinInfo["feed"].value as pinDataString)));
         
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
    internal class rssOptions
    {
        [XmlElement()]
        public Uri image;
        [XmlElement]
        public string url { get; set; }
        [XmlElement]
        public string title { get; set;}
        [XmlElement]
        public string website { get; set; }
    }
}
