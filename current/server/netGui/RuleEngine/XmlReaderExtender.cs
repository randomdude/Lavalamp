using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace netGui.RuleEngine
{
    public static class xmlReaderExtender
    {
        public static Point ReadContentAsPoint(this XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

            Point currentPoint = new Point();
            bool keepGoing = true;
            bool inhibitNextRead = false;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                {
                    keepGoing = false;
                    inhibitNextRead = true;
                }

                if (xmlName == "x" && reader.NodeType == XmlNodeType.Element)
                {
                    currentPoint.X = reader.ReadElementContentAsInt();
                    inhibitNextRead = true;
                }
                if (xmlName == "y" && reader.NodeType == XmlNodeType.Element)
                {
                    currentPoint.Y = reader.ReadElementContentAsInt();
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }

            return currentPoint;
        }

        public static Color ReadContentAsColor(this XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();

            int r = 0, g = 0, b = 0;

            bool keepGoing = true;
            bool inhibitNextRead = false;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && reader.NodeType == XmlNodeType.EndElement)
                {
                    keepGoing = false;
                    inhibitNextRead = true;
                }

                if (xmlName == "r" && reader.NodeType == XmlNodeType.Element)
                {
                    r = reader.ReadElementContentAsInt();
                    inhibitNextRead = true;
                }
                if (xmlName == "g" && reader.NodeType == XmlNodeType.Element)
                {
                    g = reader.ReadElementContentAsInt();
                    inhibitNextRead = true;
                }
                if (xmlName == "b" && reader.NodeType == XmlNodeType.Element)
                {
                    b = reader.ReadElementContentAsInt();
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }

            return Color.FromArgb(255, r,g,b);
        }

        public static List<Point> ReadContentAsPointsCollection(this XmlReader reader)
        {
            String parentTag = reader.Name.ToLower();
            List<Point> loadedPoints = new List<Point>();

            bool inhibitNextRead = false;
            bool keepGoing = true;
            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == parentTag && (reader.NodeType == XmlNodeType.EndElement || reader.IsEmptyElement))
                    keepGoing = false;

                if (xmlName == "point" && reader.NodeType == XmlNodeType.Element)
                {
                    loadedPoints.Add(reader.ReadContentAsPoint());
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }

            return loadedPoints;
            
        }
    }
}
