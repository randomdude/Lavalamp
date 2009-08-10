using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public class lineChain 
    {
        public Point start;
        public Point end;
        public Color col;
        public pinGuid sourcePin;
        public pinGuid destPin;
        public bool deleted = false;

        public static int handleSize = 7;
        public bool isdrawnbackwards;   // was the line drawn from destination to source?
        public lineChainGuid serial = new lineChainGuid() {id = Guid.NewGuid() };

        public List<Point> points;

        public delegatePack mydelegates;

        public lineChain()
        {
            //throw new Exception("linechain must have a delegatePack and all of its merry bunch of happy delegate friends");
        }

        public lineChain(delegatePack newDelegates)
        {
            points = new List<Point>();
            start = new Point(0, 0);
            end = new Point(0, 0);

            Random rngGen = new Random( DateTime.Now.Millisecond );
            col = Color.FromArgb( 255 , rngGen.Next(255), rngGen.Next(255), rngGen.Next(255) );

            mydelegates = newDelegates;
        }

        public lineChain(lineChain initWith)
        {
            points = new List<Point>();
            foreach (Point thisPnt in initWith.points)
                points.Add(new Point(thisPnt.X, thisPnt.Y));
            col = initWith.col;
            start.X = initWith.start.X;
            start.Y = initWith.start.Y;
            end.X = initWith.end.X;
            end.Y = initWith.end.Y;
            sourcePin = initWith.sourcePin;
            destPin = initWith.destPin;
            isdrawnbackwards = initWith.isdrawnbackwards;
            mydelegates = initWith.mydelegates;
        }

        #region XML serialisation

        public XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            bool keepGoing = true;
            bool inhibitNextRead = false;

            while (keepGoing)
            {
                String xmlName = reader.Name.ToLower();

                if (xmlName == "linechains" && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;


                if (xmlName == "id" && reader.NodeType == XmlNodeType.Element)
                {
                    this.serial = new lineChainGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "start" && reader.NodeType == XmlNodeType.Element)
                {
                    this.start = readPoint(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "end" && reader.NodeType == XmlNodeType.Element)
                {
                    this.end = readPoint(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "col" && reader.NodeType == XmlNodeType.Element)
                {
                    this.col = readColour(ref reader);
                    inhibitNextRead = true;
                }
                if (xmlName == "points" && reader.NodeType == XmlNodeType.Element)
                {
                    this.points = readPointsCollection(ref reader);
                }
                if (xmlName == "deleted" && reader.NodeType == XmlNodeType.Element)
                {
                    this.deleted = bool.Parse(reader.GetAttribute("value"));
                }
                if (xmlName == "isdrawnbackwards" && reader.NodeType == XmlNodeType.Element)
                {
                    this.isdrawnbackwards = bool.Parse(reader.GetAttribute("value"));
                }
                if (xmlName == "sourcepin" && reader.NodeType == XmlNodeType.Element)
                {
                    this.sourcePin = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }
                if (xmlName == "destpin" && reader.NodeType == XmlNodeType.Element)
                {
                    this.destPin = new pinGuid(reader.ReadElementContentAsString());
                    inhibitNextRead = true;
                }

                if (keepGoing && !inhibitNextRead)
                    keepGoing = reader.Read();
                inhibitNextRead = false;
            }
        }

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

        public void WriteXml(XmlWriter writer)
        {
            writeGuid(writer, "id", serial.id);
            writeGuid(writer, "destPin", destPin.id);
            writeGuid(writer, "sourcePin", sourcePin.id);
            writePoint(writer, "start", start);
            writePoint(writer, "end", end);
            writeColour(writer, "col", col);
            writeBool(writer, "deleted", deleted);
            writeBool(writer, "isdrawnbackwards", isdrawnbackwards);
            writer.WriteStartElement( "points" );
            foreach (Point thisPoint in this.points)
                writePoint(writer, "point", thisPoint);
            writer.WriteEndElement();
        }

        private void writeGuid(XmlWriter writer, string name, Guid serial)
        {
            writer.WriteElementString(name, serial.ToString());
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
            // coax signal from start of wire to end, which will fire off the appropriate events at the destination end
            if (!this.deleted)
            {
                pin dest = mydelegates.GetPinFromGuid(this.destPin);
                ruleItemBase destItem = mydelegates.GetRuleItemFromGuid(dest.parentRuleItem);
                pin source = mydelegates.GetPinFromGuid(this.sourcePin);
                ruleItemBase sourceItem = mydelegates.GetRuleItemFromGuid(source.parentRuleItem);
                destItem.pinStates[dest.name] = sourceItem.pinStates[source.name];
            }
        }

        #region rendering


        public static void drawLine(Graphics toThis, Point from, Point to, Color lineCol)
        {
            toThis.DrawLine(new Pen(lineCol), new Point(from.X, from.Y), new Point(to.X, to.Y));
        }

        public static void drawHandle(Graphics toThis, Point here)
        {
            Rectangle bounds = new Rectangle(here.X - (handleSize / 2), here.Y - (handleSize / 2), handleSize, handleSize);
            toThis.DrawRectangle(new Pen(Color.Blue), bounds);
        }

        public void draw(Graphics toThis)
        {
            Point cursor = this.start;
            drawHandle(toThis,this.start);
            foreach (Point nextPoint in this.points)
            {
                drawLine(toThis, cursor, nextPoint, this.col);
                drawHandle(toThis, nextPoint);
                cursor = nextPoint;
            }
            drawHandle(toThis, this.end);
            drawLine(toThis, cursor, this.end, this.col);
        }
        #endregion

        public void deleteSelf()
        {
            // tell both involved pins the bad news
            pin source = mydelegates.GetPinFromGuid(sourcePin);
            pin dest = mydelegates.GetPinFromGuid(destPin);
            if (source.isConnected)
                source.disconnect();
            if (dest.isConnected)
                dest.disconnect();

            // remove any changeHandler delegates currently hooked up
            mydelegates.GetRuleItemFromGuid(source.parentRuleItem).removePinChangeHandler(source.name);
            mydelegates.GetRuleItemFromGuid(source.parentRuleItem).removePinChangeHandler(dest.name);

            // goodbye cruel world
            this.deleted = true;
        }

        public void connectTo(pin source, pin dest)
        {
            dest.connectTo(this.serial, source.serial);
            source.connectTo(this.serial, dest.serial);
        }

    }
}
