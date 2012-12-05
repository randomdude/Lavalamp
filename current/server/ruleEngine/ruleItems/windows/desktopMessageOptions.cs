using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems
{
    public class desktopMessageOptions : BaseOptions, IXmlSerializable
    {
        // These are in hundreds of milliseconds
        public int fadeInSpeed = 2;
        public int holdSpeed = 40;
        public int fadeOutSpeed = 5;
        public string message = "$message happened!";
        public Color background = Color.DarkViolet;
        public Color foreground = Color.White;

        public desktopMessageLocation dsklocation = desktopMessageLocation.BottomRight;

        public desktopMessageOptions(desktopMessageOptions newOptions)
        {
            fadeInSpeed = newOptions.fadeInSpeed;
            holdSpeed = newOptions.holdSpeed;
            fadeOutSpeed = newOptions.fadeOutSpeed;
            message = newOptions.message;
            dsklocation = newOptions.dsklocation;
            background = newOptions.background;
            foreground = newOptions.foreground;
        }

        public desktopMessageOptions() { }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }



        public void ReadXml(XmlReader reader)
        {
            int foreR = 0, foreG = 0, foreB = 0;
            int backR = 0, backG = 0, backB = 0;
            String parentTag = reader.Name.ToLower();

            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (xmlName == "fadeinspeed" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    fadeInSpeed = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "holdspeed" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    holdSpeed = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "fadeoutspeed" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    fadeOutSpeed = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "message" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    message = reader.ReadElementContentAsString();
                    inhibitNextRead = true;
                }

                if (xmlName == "foreground-r" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    foreR = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "foreground-g" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    foreG = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "foreground-b" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    foreB = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }


                if (xmlName == "background-r" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    backR = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "background-g" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    backG = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "background-b" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    backB = int.Parse(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (xmlName == "location" && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    dsklocation = (desktopMessageLocation) Enum.Parse(typeof (desktopMessageLocation), reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                
                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }

            foreground = Color.FromArgb(foreR, foreG, foreB);
            background = Color.FromArgb(backR, backG, backB);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("fadeInSpeed", fadeInSpeed.ToString());
            writer.WriteElementString("holdSpeed", holdSpeed.ToString());
            writer.WriteElementString("fadeOutSpeed", fadeOutSpeed.ToString());
            writer.WriteElementString("message", message);

            writer.WriteElementString("location", Enum.GetName(typeof(desktopMessageLocation), this.dsklocation) );

            // todo: writeElementColor
            writer.WriteElementString("foreground-r", foreground.R.ToString());
            writer.WriteElementString("foreground-g", foreground.G.ToString());
            writer.WriteElementString("foreground-b", foreground.B.ToString());
            writer.WriteElementString("background-r", background.R.ToString());
            writer.WriteElementString("background-g", background.G.ToString());
            writer.WriteElementString("background-b", background.B.ToString());
        }

        public override string typedName
        {
            get { return "DesktopMessage";  }
        }

    }
}

public enum desktopMessageLocation
{
    TopLeft,
    TopMiddle,
    TopRight,
    BottomLeft,
    BottomMiddle,
    BottomRight
}