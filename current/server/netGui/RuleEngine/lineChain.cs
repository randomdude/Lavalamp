using System;
using System.Collections.Generic;
using System.Drawing;
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
        public lineChainGuid serial = new lineChainGuid();

        public List<Point> midPoints;

        private readonly delegatePack myDelegates;

        public lineChain() {}

        public lineChain(delegatePack newDelegates)
        {
            midPoints = new List<Point>();
            start = new Point(0, 0);
            end = new Point(0, 0);

            Random rngGen = new Random( DateTime.Now.Millisecond );
            col = Color.FromArgb( 255 , rngGen.Next(255), rngGen.Next(255), rngGen.Next(255) );

            myDelegates = newDelegates;
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
            foreach (Point nextPoint in midPoints)
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
            // Find both involved pins and disconnect them if necessary
            pin source = myDelegates.GetPinFromGuid(sourcePin);
            pin dest = myDelegates.GetPinFromGuid(destPin);

            if (source.isConnected)
                source.disconnect();
            if (dest.isConnected)
                dest.disconnect();

            // remove any changeHandler delegates currently hooked up
            source.removeAllPinHandlers();
            dest.removeAllPinHandlers();

            // goodbye cruel world
            deleted = true;
        }

        public void handleStateChange()
        {
            // coax signal from start of wire to end, which will fire off the appropriate events at the destination end.
            if (!deleted)
            {
                pin dest = myDelegates.GetPinFromGuid(destPin);
                ruleItemBase destItem = myDelegates.GetRuleItemFromGuid(dest.parentRuleItem);

                pin source = myDelegates.GetPinFromGuid(sourcePin);
                ruleItemBase sourceItem = myDelegates.GetRuleItemFromGuid(source.parentRuleItem);

                destItem.pinInfo[dest.name].value.setData( sourceItem.pinInfo[source.name].value.getData() );
            }
        }
    }
}
