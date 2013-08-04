namespace netGui.RuleEngine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;
    using System.Linq;

    using ruleEngine;
    using ruleEngine.ruleItems;

    [Serializable]
    public partial class ctlRule : UserControl
    {
        private rule _rule;

        // These two hold the pictureBox and (partial-)lineChain which we are currently wiring up
        private PictureBox currentlyConnecting;
        private lineChain currentLine;

        private lineChain currentlyDraggingLine = null;         // These two are indexes to the lineChain/point that we are dragging when we drag a handle
        private int currentlyDraggingPoint = -1;

        private lineChain currentlyConextedLine = null;         // These two are indexes to the lineChain/point that we right-clicked on, when showing a context menu
        private int currentlyConextedPoint = -1;

        private readonly Color connectingPinColour = Color.Cyan;
        private Color normalPinColour = Color.Transparent;
        private Bitmap _backBuffer;

        private bool snapWidgetsToGrid;

        public delegate void ruleItemLoaded(ruleItemBase item);
        public event ruleItemLoaded onRuleItemLoaded;

        /// <summary>
        /// A List of all child ctlRuleItems
        /// </summary>
        List<ctlRuleItemWidget> itemWidgets = new List<ctlRuleItemWidget>();

        private Cursor _dragCursorMove;

        public ctlRule()
        {
            this.InitializeComponent();
            this._rule = new rule();
            this.currentLine = new lineChain();
            // done for unitTests
            if (Parent == null) Parent = new Form();
#if DEBUG
            this.showDebugInfoToolStripMenuItem.Visible = true;
#endif
        }

        #region rule manipulation

        /// <summary>
        /// Spawn a new ruleItem of the described type
        /// </summary>
        /// <param name="info">Description of the type to create</param>
        public void addRuleItem(ruleItemInfo info)
        {
            this.addRuleItem(info, 0, 0);
        }

        public void addRuleItem(ruleItemInfo info, int x, int y)
        {
            // Create our rule item
            ruleItemBase newRuleItem = this._rule.addRuleItem(info);

            // add a visual widget for it, and then add it to the visible controls
            ctlRuleItemWidget newCtl = new ctlRuleItemWidget(newRuleItem, this.setTsStatus);
            newCtl.Location = new Point(x, y);
       //     this._rule.AddctlRuleItemWidgetToGlobalPool(newCtl);
            newCtl.snapToGrid = this.snapWidgetsToGrid;
            this.Controls.Add(newCtl);
            newCtl.BringToFront();
            this.itemWidgets.Add(newCtl);
        }

        /// <summary>
        /// Make rule item controls for every rule item present in our rule.
        /// </summary>
        private void addRuleItemControlsAfterDeserialisation()
        {
            this.Parent.Width = this._rule.preferredWidth;
            this.Parent.Height = this._rule.preferredHeight;
            IEnumerable<IRuleItem> childRuleItems = this._rule.getRuleItems();

            foreach (ruleItemBase thisRule in childRuleItems)
            {
                ctlRuleItemWidget newCtl = new ctlRuleItemWidget(thisRule, this.setTsStatus);

                //hook up line events for deserialized control.
                foreach (var pin in newCtl.conPins.Keys.Where(p => p.isConnected))
                {
                    lineChain line = this._rule.GetLineChainFromGuid(pin.parentLineChain);
                    newCtl.OnRuleItemMoved += line.LineMoved;
                    line.onLineDeleted += pin.Disconnected;
                }
               // this._rule.AddctlRuleItemWidgetToGlobalPool(newCtl);
                newCtl.snapToGrid = this.snapWidgetsToGrid;
                this.Controls.Add(newCtl);
                newCtl.BringToFront();
                this.itemWidgets.Add(newCtl);
                if (this.onRuleItemLoaded != null)
                    this.onRuleItemLoaded.Invoke(thisRule);
            }
        }

        public string serialiseRule()
        {
            this._rule.preferredHeight = this.Parent.Height;
            this._rule.preferredWidth = this.Parent.Width;
            return this._rule.serialise();
        }

        public void deserialiseRule(string serialised)
        {
            this._rule = rule.deserialise(serialised);
            this.Parent.Width = this._rule.preferredWidth;
            this.Parent.Height = this._rule.preferredHeight;
        }

        #endregion

        #region logic to wire up lineChain wires

        public void startOrFinishLine(PictureBox startFrom)
        {
            if (this.currentlyConnecting == null)   
            {
                // user has just started a drag
                this.startLine(startFrom);
            }
            else
            {
                // user has finished a drag
                this.finishLine(startFrom);
            }
            this.Invalidate();
        }

        private void startLine(PictureBox from)
        {
            pin source = (pin)from.Tag;

            if ( source.isConnected )
            {
                MessageBox.Show("Pin is already connected to something");
                return;
            }

            this.currentlyConnecting = from;
            this.currentlyConnecting.BackColor = this.connectingPinColour;
            this.currentLine = new lineChain
                {
                    serial = new lineChainGuid(){id = Guid.NewGuid()},
                    start = this.PointToClient(Control.MousePosition),
                    end = this.PointToClient(Control.MousePosition)
                };
            // set temporarily, so the line doesn't stretch to the origin until the first mouseMove
        }

        public void finishLine(PictureBox endTarget)
        {
            ctlRuleItemWidget sourceItemWidget;

            pin source;
            pin dest;

            // Which direction has the user drawn a line in?
            if (((pin)this.currentlyConnecting.Tag).direction == pinDirection.output)
            {
                sourceItemWidget = ((ctlRuleItemWidget)this.currentlyConnecting.Parent);

                source = (pin)this.currentlyConnecting.Tag;
                dest = (pin)endTarget.Tag;
            }
            else
            {
                // the user has drawn the line from destination to source.
                this.currentLine.isdrawnbackwards = true;
                sourceItemWidget = ((ctlRuleItemWidget)endTarget.Parent);

                dest = (pin)this.currentlyConnecting.Tag;
                source = (pin)endTarget.Tag;
            }
            Contract.Assume(source != null && dest != null);

            this.currentlyConnecting.BackColor = this.normalPinColour;  // erase highlight

            // A few sanity checks
            bool errored = false;
            if (source.isConnected || dest.isConnected )
            {
                MessageBox.Show("Pin is already connected to something");
                errored = true;
            }
            if (source == dest)
            {
                MessageBox.Show("Pin cannot be connected to itself");
                errored = true;
            }
            if (source.direction == dest.direction)
            {
                MessageBox.Show(source.direction.ToString() + " pins cannot be connected to other " + source.direction.ToString() + " pins. Wires must connect inputs to outputs.");
                errored = true;
            }
            if (dest.possibleTypes.Count(t => t == source.valueType) < 1)
            {
                MessageBox.Show("Pins types are incompatable");
                errored = true;
            }

            if(errored)
            {
                this.currentLine = new lineChain();
                source.parentLineChain.id = Guid.Empty;
                this.currentlyConnecting = null;
                return;
            }
            
            // Hook pins up
            this.currentLine.onLineDeleted += source.Disconnected;
            this.currentLine.onLineDeleted += dest.Disconnected;
            dest.connectTo(this.currentLine.serial, source);
            source.connectTo(this.currentLine.serial, dest);
            
            // hook ruleitem events up to the line
            ctlRuleItemWidget ruleEnd = ((ctlRuleItemWidget) endTarget.Parent);
            ctlRuleItemWidget ruleStart = ((ctlRuleItemWidget) this.currentlyConnecting.Parent);
            ruleEnd.OnRuleItemMoved += this.currentLine.LineMoved;
            ruleStart.OnRuleItemMoved += this.currentLine.LineMoved;

            this._rule.AddLineChainToGlobalPool(this.currentLine);
            
            this.currentLine = new lineChain();

            ruleEnd.alignWires();
            ruleStart.alignWires();

            this.currentlyConnecting = null;
        }

        /// <summary>
        /// Determine if the current mouse cursor is hovering over a draggable handle.
        /// </summary>
        /// <returns></returns>
        private bool isHandleUnderPointer()
        {
            Point cursor = this.PointToClient(Control.MousePosition);

            IEnumerable<lineChain> childLines = this._rule.getNonDeletedLineChains();
            foreach (lineChain aLine in childLines )
            {
                if (this.isHandleUnderPoint(aLine.start, cursor))
                    return true;

                if (this.isHandleUnderPoint(aLine.end, cursor))
                    return true;

                foreach (Point aPoint in aLine.midPoints)
                {
                    if (this.isHandleUnderPoint(aPoint, cursor))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Is there a draggable handle directly under a given Point?
        /// </summary>
        /// <param name="checkThis"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        private bool isHandleUnderPoint(Point checkThis, Point cursor)
        {

            int offX = Math.Abs(checkThis.X - cursor.X);
            int offY = Math.Abs(checkThis.Y - cursor.Y);

            if (offX <= lineChain.handleSize / 2 && offY <= lineChain.handleSize / 2)
                return true;

            return false;
        }

        #endregion

        private void selectCurrentHandleAsContexted(Point cursor)
        {
            // Find the handle under the mouse cursor and select it as the 'contexted'.
            this.currentlyConextedLine = null;
            this.currentlyConextedPoint = -1;

            IEnumerable<lineChain> lines = this._rule.getNonDeletedLineChains();
            foreach (lineChain aLine in lines)
            {

                // First, see if the cursor is over a draggable midPoint, and start 
                // dragging it if so.
                int pointIndex = 0;
                foreach (Point aPoint in aLine.midPoints)
                {
                    if (this.isHandleUnderPoint(aPoint, cursor))
                    {
                        this.currentlyConextedLine = aLine ;
                        this.currentlyConextedPoint = pointIndex;
                        return;
                    }
                    pointIndex++;
                }
            }

            // If we got here, we didn't find the handle under the pointer. 
            // Perhaps a start/end is being positioned (which are handled differently).
            foreach (lineChain aLine in lines)
            {
                // If the cursor _is_ hovering over a start or end, don't set 
                // currentlyContextedPoint (since there isn't one),just -Line.
                if (this.isHandleUnderPoint(aLine.start, cursor))
                {
                    this.currentlyConextedLine = aLine;
                    return;
                }

                if (this.isHandleUnderPoint(aLine.end, cursor))
                {
                    this.currentlyConextedLine = aLine;
                    return;
                }
            }

            throw new Exception("Unable to find current Handle");
        }

        private void selectCurrentHandleAsDragging(Point cursor)
        {
            IEnumerable<lineChain> lines = this._rule.getNonDeletedLineChains();
            foreach (lineChain aLine in lines)
            {
                int pointIndex = 0;

                foreach (Point aPoint in aLine.midPoints)
                {
                    if (this.isHandleUnderPoint(aPoint, cursor))
                    {
                        this.currentlyDraggingLine = aLine;
                        this.currentlyDraggingPoint = pointIndex;
                        return;
                    }
                    pointIndex++;
                }
            }
        }

        public void deleteRuleItem(ctlRuleItemWidget toDelete)
        {
            this._rule.deleteRuleItem(toDelete.targetRuleItem);

            // remove the associated ctlRuleItem
           // this._rule.deleteCtlRuleItem(toDelete);

            // remove the actual control
            this.Controls.Remove(toDelete);
            this.Invalidate();
        }

        public delegate void setTsStatusDlg(String toThis);

        /// <summary>
        /// Sets the text of the toolstrip's 'status' label
        /// </summary>
        /// <param name="toThis">new status</param>
        public void setTsStatus(string toThis)
        {
            if (toThis != null) 
                this.tsStatus.Text = toThis;
        }

        #region general event handlers

        private void FrmRule_Paint(object sender, PaintEventArgs e)
        {
            if (this._backBuffer == null)
            {
                this._backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            }
            using(Graphics graphics = Graphics.FromImage(this._backBuffer))
            {
                graphics.Clear(Color.LightGray);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                IEnumerable<lineChain> lines = this._rule.getNonDeletedLineChains();
                foreach (lineChain aLine in lines)
                    aLine.draw(graphics);

                if (this.currentlyConnecting != null)
                    this.currentLine.draw(graphics);
            }
            e.Graphics.DrawImageUnscaled(this._backBuffer,0,0);

        }

        private void FrmRule_Click(object sender, EventArgs e)
        {
            if (this.currentlyConnecting != null)
            {
                // We are currently drawing a wire. Add a line segment.
                this.currentLine.midPoints.Add(this.PointToClient(Control.MousePosition));
            } 
            else if ( e.GetType() == typeof(MouseEventArgs) &&  ((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right  )
            {
                // Right-click has occured! I wonder where?
                if (this.isHandleUnderPointer())
                {
                    // user has right-clicked a handle. Find which one is under the pointer
                    this.selectCurrentHandleAsContexted(((MouseEventArgs)e).Location);

                    // If the point in question isn't found by the above, it is a start or end handle, and thus is undeletable.
                    if (this.currentlyConextedPoint == -1)
                        this.mnuItemDelJunc.Enabled = false;
                    else
                        this.mnuItemDelJunc.Enabled = true;

                    // and show the menu in the appropriate place
                    this.mnuStripHandles.Show();
                    this.mnuStripHandles.Location = (Control.MousePosition);
                }
            }
        }

        private void FrmRule_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.isHandleUnderPointer())
            {
                // cursor is over a handle
                this.Cursor = Cursors.Hand;
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // User is attempting to drag a wire corner handle. Find which one
                    this.selectCurrentHandleAsDragging(e.Location);
                }
            }
        }

        #endregion

        #region junction context-menu handlers

        private void FrmRule_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.currentlyConnecting != null)
            {
                // if we're currently drawing a line, then, er, draw it to the cursor
                this.setTsStatus("Click to create a 'corner' in the wire, or click on destination arrow to end");
                this.currentLine.end = this.PointToClient(Control.MousePosition);
                this.Invalidate(false);
            }
            else
            {
                if (this.currentlyDraggingPoint > 0 && e.Button == MouseButtons.Left && this.currentlyDraggingPoint < currentlyDraggingLine.midPoints.Count)
                {
                    // User is attempting to drag a wire corner handle
                    this.setTsStatus("");
                    this.currentlyDraggingLine.midPoints[this.currentlyDraggingPoint] = this.PointToClient(Control.MousePosition);
                    this.Invalidate(false);
                }
                else
                {
                    if (this.isHandleUnderPointer())
                    {
                        this.setTsStatus("Drag to move this wire junction, or right-click for options pertaining to it");
                        this.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        this.setTsStatus("");
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void FrmRule_MouseUp(object sender, MouseEventArgs e)
        {
            this.currentlyDraggingPoint = -1;
            this.currentlyDraggingLine = null;
        }

        private void mnuItemDelJunc_Click(object sender, EventArgs e)
        {
            this.currentlyConextedLine.midPoints.RemoveAt(this.currentlyConextedPoint);
            this.Invalidate();
        }

        private void mnuItemAddJunc_Click(object sender, EventArgs e)
        {
            Point prevPoint;
            Point nextPoint;
            if (this.currentlyConextedPoint == 0 && currentlyConextedPoint < currentlyConextedLine.midPoints.Count)
            {
                prevPoint = this.currentlyConextedLine.start;
                nextPoint = this.currentlyConextedLine.midPoints[this.currentlyConextedPoint];
            }
            else if (this.currentlyConextedPoint == -1)
            {
                // the line we are manipulating has only start and end points, no midpoints. 
                prevPoint = this.currentlyConextedLine.start;
                nextPoint = this.currentlyConextedLine.end;
            }
            else if (this.currentlyConextedLine.midPoints.Count > this.currentlyConextedPoint + 1 && 
                        this.currentlyConextedPoint >= 0)
            {
                prevPoint = this.currentlyConextedLine.midPoints[this.currentlyConextedPoint];
                nextPoint = this.currentlyConextedLine.midPoints[this.currentlyConextedPoint + 1];
            } 
            else 
            {
                prevPoint = this.currentlyConextedLine.midPoints[this.currentlyConextedPoint];
                nextPoint = this.currentlyConextedLine.end;
            }

            Point nextLine = new Point();
            nextLine.X = Math.Abs(prevPoint.X - nextPoint.X);
            nextLine.Y = Math.Abs(prevPoint.Y - nextPoint.Y);

            Point newPoint = new Point();
            
            if (prevPoint.X < nextPoint.X)
                newPoint.X = prevPoint.X + (nextLine.X / 2);
            else
                newPoint.X = prevPoint.X - (nextLine.X / 2);

            if (prevPoint.Y < nextPoint.Y)
                newPoint.Y = prevPoint.Y + (nextLine.Y / 2);
            else
                newPoint.Y = prevPoint.Y - (nextLine.Y / 2);

            if (this.currentlyConextedPoint < 1 )
                this.currentlyConextedLine.midPoints.Insert(0 , newPoint);
            else
                this.currentlyConextedLine.midPoints.Insert(this.currentlyConextedPoint + 1 , newPoint);

            this.Invalidate();
        }

        private void mnuItemDelLine_Click(object sender, EventArgs e)
        {
            this.currentlyConextedLine.requestDelete();
            this.Invalidate();
        }

        #endregion

        public void stop()
        {
            if (this._rule.isRunning)
            {
                this.Enabled = true;
                this.toolStripProgressBar.MarqueeAnimationSpeed = 0;
                this.toolStripProgressBar.Visible = false;

                this._rule.stop();
                this.tmrStep.Stop();
            }
        }

        public void start()
        {
            if (!this._rule.isRunning)
            {
                this.Enabled = false;
                this.toolStripProgressBar.MarqueeAnimationSpeed = 100;
                this.toolStripProgressBar.Visible = true;

                this._rule.start();
                this.tmrStep.Start();
            }
        }

        private void showDebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDebug(this.currentlyConextedLine).Show();
        }

        public rule getRule()
        {
            return this._rule;
        }

        public void loadRule(rule toLoad)
        {
            this.stop();
            this._rule = toLoad;

            this.addRuleItemControlsAfterDeserialisation();
        }

        private void ctlRule_SizeChanged(object sender, EventArgs e)
        {
            if(this._backBuffer != null)
            {
                this._backBuffer.Dispose();
                this._backBuffer = null;
            }
         //   base.OnSizeChanged(e);
        }

        public void advanceDelta()
        {
            this._rule.advanceDelta();
        }

        public void snapAllToGrid()
        {
            foreach (lineChain thisLC in this._rule.getLineChains())
            {
                for (int n = 0; n < thisLC.midPoints.Count; n++ )
                {
                    Point midPoint = thisLC.midPoints[n];

                    double roundedX = midPoint.X / 10D;
                    double roundedY = midPoint.Y / 10D;

                    roundedX = Math.Round(roundedX, 0) * 10 ;
                    roundedY = Math.Round(roundedY, 0) * 10 ;

                    thisLC.midPoints[n] = new Point((int) roundedX, (int) roundedY);
                }
            }

            foreach (ctlRuleItemWidget thisWidget in this.itemWidgets)
            {
                double roundedX = thisWidget.Location.X / 10D;
                double roundedY = thisWidget.Location.Y / 10D;

                roundedX = Math.Round(roundedX, 0) * 10 ;
                roundedY = Math.Round(roundedY, 0) * 10 ;

                thisWidget.Location = new Point((int) roundedX, (int) roundedY);
                thisWidget.alignWires();
            } 

            this.Invalidate();
        }

        public void setGridSnapping(bool newVal)
        {
            foreach (ctlRuleItemWidget widget in this.itemWidgets)
            {
                widget.snapToGrid = newVal;
                this.snapWidgetsToGrid = newVal;
            } 
        }

        private void tmrStep_Tick(object sender, EventArgs e)
        {
            this.advanceDelta();
        }

        private void ctlRule_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ruleItemInfo)))
                e.Effect = DragDropEffects.Copy;
            else
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            ruleItemInfo info = (ruleItemInfo)e.Data.GetData(typeof(ruleItemInfo));
            if (this._dragCursorMove != null)
            {
                this._dragCursorMove.Dispose();
                this._dragCursorMove = null;
            }
            Cursor.Current = this._dragCursorMove = cursorUtil.CreateCursor((Bitmap)ctlRuleItemWidget.getItemImage(info.ruleItemBaseType), 0, 0);
        }

        private void ctlRule_DragDrop(object sender, DragEventArgs e)
        {
            ruleItemInfo info = (ruleItemInfo) e.Data.GetData(typeof(ruleItemInfo));
            Point actual = this.PointToClient(new Point(e.X , e.Y));
            this.addRuleItem(info, actual.X, actual.Y);
        }

    }
}
