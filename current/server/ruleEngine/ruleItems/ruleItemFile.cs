using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ruleEngine.Properties;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    public enum FileFormat
    {
        Text,
        RSS,
        Bin
    //    Database
    }
    [ToolboxRule]
    [ToolboxRuleCategory("Misc")]
    public class ruleItemFile : ruleItemBase
    {
        private string _caption;
        private string _lastVal;
        private PictureBox _img;
        [XmlElement("Options")]
        public FileWriteOptions options = new FileWriteOptions{fileFormat = FileFormat.Text};


        public ruleItemFile()
        {
            _img = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(39, 33),
                Location = new Point((preferredSize().Width / 2) - (39 / 2),
                    (preferredSize().Height / 3) - (33 / 2)),
                Margin = new Padding(3, 3, 3, 3)
            };
            _img.Image = Resources.ruleItem_text;
            controls.Add(_img);
        }

        public override void start()
        {
            _lastVal = null;
            base.start();
        }

        public override Image background()
        {
            return _img == null ? null : _img.Image;
        }

        public override string ruleName()
        {
            return "Save";
        }

        public override string caption()
        {
            return "Save";
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("writeToFile", new pin {name = "writeToFile",description = "to write to file",direction = pinDirection.input,valueType = typeof (pinDataString)});
            return pins;
        }


        public override Form ruleItemOptions()
        {
            frmFileWriteOptions frm = new frmFileWriteOptions(options);
            frm.Closed += frm_Closed;
            return frm;
        }

        void frm_Closed(object sender, EventArgs e)
        {
            frmFileWriteOptions frm = (frmFileWriteOptions) sender;
            if (frm.DialogResult == DialogResult.OK)
            {
                options = frm.SelectedOptions;
                switch (options.fileFormat)
                {
                        case FileFormat.Text:
                        case FileFormat.Bin:
                            _img.Image = Resources.ruleItem_text;
                        break;
                        case FileFormat.RSS:
                            _img.Image = Resources.ruleItem_rss;
                        break;
                }
                
            }
        }

        public override void evaluate()
        {
            if (pinInfo["writeToFile"].value.ToString() == _lastVal)
                return;
            _lastVal = pinInfo["writeToFile"].value.ToString();
            using (FileStream toWrite = File.Open(options.filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                switch (options.fileFormat)
                {
                    case FileFormat.RSS:
                        SyndicationFeed feed;
                        try
                        {
                            feed = SyndicationFeed.Load(new XmlTextReader(toWrite)) ?? new SyndicationFeed(options.additionalInfo,"",options.publishURI);
                        }
                        catch (Exception)
                        {
                            feed = new SyndicationFeed(options.additionalInfo,"",options.publishURI);
                        }
                        List<SyndicationItem> items = new List<SyndicationItem>(feed.Items);
                        items.Add(new SyndicationItem("" , _lastVal , options.publishURI));
                        feed.Items = items;
                        XmlTextWriter xmlText = new XmlTextWriter(toWrite , Encoding.UTF8);
                        feed.SaveAsRss20(xmlText);
                        xmlText.Flush();
                        break;
                    case FileFormat.Text:
                        //seek to append to the end of the file.
                        toWrite.Seek(0 , SeekOrigin.End);
                        StreamWriter writer = new StreamWriter(toWrite);
                        writer.WriteLine(_lastVal);
                        writer.Flush();
                        break;
                    case FileFormat.Bin:
                        //this may be replaced by a binary type stream
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(toWrite,_lastVal);
                        toWrite.Flush();
                        break;
                   /* case FileFormat.Database:
                        if (options.dataTransform == DataTransformation.None)
                            DatabaseHelper.SaveTo(options.connectionString , options.table , options.field , _lastVal);
                        else
                            DatabaseHelper.SaveTo(options.connectionString, options.table, options.dataTransform, _lastVal);
                        break;*/
                }
            }
        }
    }
    [Serializable]
    public class FileWriteOptions
    {
        [XmlIgnore]
        public Uri publishURI { get; set; }

        public string siteString { get { return publishURI != null ? publishURI.AbsoluteUri : null; } set { publishURI = new Uri(value); } }

        public FileFormat fileFormat { get; set; }

        public string filePath { get; set; }

        public string additionalInfo { get; set; }

        public string description { get; set; }

        /*public string connectionString { get; set; }

        public string table { get; set; }

        public DataTransformation dataTransform { get; set; }

        public string field { get; set;}
        */
    }

    /*public enum DataTransformation
    {
        None,
        XML,
        JSON
    }*/

}
