using System;
using System.Collections.Generic;
using System.Drawing;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    public partial class lineChain 
    {
        public Point start;
        public Point end;
        public Color col;

        /// <summary>
        /// Has this pin been marked as 'deleted'?
        /// </summary>
        public bool isDeleted { get; protected set; }

        public const int handleSize = 7;
        public bool isdrawnbackwards;   // was the line drawn from destination to source?
        public lineChainGuid serial = new lineChainGuid();
        public List<Point> midPoints;

        /// <summary>
        /// Fired when the lineChain is being deleted
        /// </summary>
        public event EventHandler onLineDeleted;

        public lineChain()
        {
            midPoints = new List<Point>();
            start = new Point(0, 0);
            end = new Point(0, 0);
            isDeleted = false;

            Random rngGen = new Random( DateTime.Now.Millisecond );
            col = Color.FromArgb( 255 , rngGen.Next(255), rngGen.Next(255), rngGen.Next(255) );
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

        public void requestDelete()
        {
            if (isDeleted)
                return;

            if (onLineDeleted != null)
                onLineDeleted.Invoke(this, null);

            // goodbye cruel world
            isDeleted = true;
        }

        public void LineMoved(object sender, ItemMovedArgs args)
        {
            if (args.lineAffected.id != serial.id)
                return;
            if ((!isdrawnbackwards && args.pinDirection == pinDirection.input) ||
                (isdrawnbackwards && args.pinDirection == pinDirection.output))
                end = args.point;

           if ((!isdrawnbackwards && args.pinDirection == pinDirection.output) ||
               (isdrawnbackwards && args.pinDirection == pinDirection.input))
                start = args.point;
        }
    }
}
