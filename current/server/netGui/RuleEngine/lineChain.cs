using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public partial class lineChain 
    {
        public Point start;
        public Point end;
        public Color col;
        public pinGuid sourcePin;
        public pinGuid destPin;
        public bool deleted = false;

        public const int handleSize = 7;
        public bool isdrawnbackwards;   // was the line drawn from destination to source?
        public lineChainGuid serial;

        public List<Point> points;

        private delegatePack myDelegates;

        public lineChain() {}

        public lineChain(delegatePack newDelegates)
        {
            serial = new lineChainGuid();

            points = new List<Point>();
            start = new Point(0, 0);
            end = new Point(0, 0);

            Random rngGen = new Random( DateTime.Now.Millisecond );
            col = Color.FromArgb( 255 , rngGen.Next(255), rngGen.Next(255), rngGen.Next(255) );

            myDelegates = newDelegates;
        }

        #region XML serialisation

        private List<Point> readPointsCollection(ref XmlReader reader)
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
                    loadedPoints.Add(readPoint(ref reader));
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }

            return loadedPoints;
        }

        private Color readColour(ref XmlReader reader)
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

        private Point readPoint(ref XmlReader reader)
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

        private void writeGuid(XmlWriter writer, string name, Guid newSerial)
        {
            writer.WriteElementString(name, newSerial.ToString());
        }

        private void writeBool(XmlWriter writer, string name, bool writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteAttributeString("value", writeThis.ToString());
            writer.WriteEndElement();
        }

        private void writeColour(XmlWriter writer, string name, Color writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteElementString("R", writeThis.R.ToString());
            writer.WriteElementString("G", writeThis.G.ToString());
            writer.WriteElementString("B", writeThis.B.ToString());
            writer.WriteEndElement();
        }

        public void writePoint(XmlWriter writer, String name, Point writeThis)
        {
            writer.WriteStartElement(name);
            writer.WriteElementString("X", writeThis.X.ToString());
            writer.WriteElementString("Y", writeThis.Y.ToString());
            writer.WriteEndElement();
        }

        #endregion

        public void handleStateChange()
        {
            // coax signal from start of wire to end, which will fire off the appropriate events at the destination end.
            if (!deleted)
            {
                pin dest = myDelegates.GetPinFromGuid(destPin);
                ruleItemBase destItem = myDelegates.GetRuleItemFromGuid(dest.parentRuleItem);

                pin source = myDelegates.GetPinFromGuid(sourcePin);
                ruleItemBase sourceItem = myDelegates.GetRuleItemFromGuid(source.parentRuleItem);

                destItem.pinStates[dest.name] = sourceItem.pinStates[source.name];
            }
        }

        #region rendering

        /// <summary>
        /// Draw this lineChain on the Graphics object provided
        /// </summary>
        /// <param name="toThis">Graphics object to draw on</param>
        public void draw(Graphics toThis)
        {
            Point cursor = start;
            drawHandle(toThis, start);
            foreach (Point nextPoint in points)
            {
                drawLine(toThis, cursor, nextPoint, col);
                drawHandle(toThis, nextPoint);
                cursor = nextPoint;
            }
            drawHandle(toThis, end);
            drawLine(toThis, cursor, end, col);
        }

        private static void drawLine(Graphics toThis, Point from, Point to, Color lineCol)
        {
            toThis.DrawLine(new Pen(lineCol), new Point(from.X, from.Y), new Point(to.X, to.Y));
        }

        private static void drawHandle(Graphics toThis, Point here)
        {
            Rectangle bounds = new Rectangle(here.X - (handleSize / 2), here.Y - (handleSize / 2), handleSize, handleSize);
            toThis.DrawRectangle(new Pen(Color.Blue), bounds);
        }

        #endregion

        public void deleteSelf()
        {
            // Find both involved pins and disconnect them if neccesary
            pin source = myDelegates.GetPinFromGuid(sourcePin);
            pin dest = myDelegates.GetPinFromGuid(destPin);
            if (source.isConnected)
                source.disconnect();
            if (dest.isConnected)
                dest.disconnect();

            // remove any changeHandler delegates currently hooked up
            myDelegates.GetRuleItemFromGuid(source.parentRuleItem).removePinChangeHandler(source.name);
            myDelegates.GetRuleItemFromGuid(source.parentRuleItem).removePinChangeHandler(dest.name);

            // goodbye cruel world
            deleted = true;
        }

        public void connectTo(pin source, pin dest)
        {
            dest.connectTo(serial, source.serial);
            source.connectTo(serial, dest.serial);
        }

    }
}
