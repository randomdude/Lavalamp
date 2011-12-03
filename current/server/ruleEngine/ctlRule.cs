using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    [Serializable]
    public partial class ctlRule : UserControl
    {
        public ctlRule()
        {
            InitializeComponent();
            _rule = new rule();
            commonConstructorStuff();
        }

        public ctlRule(rule ruleToDisplay)
        {
            InitializeComponent();
            _rule = ruleToDisplay;
            commonConstructorStuff();
        }

        void commonConstructorStuff()
        {
            currentLine = new lineChain();
#if DEBUG
            showDebugInfoToolStripMenuItem.Visible = true;
#endif
        }

        private rule _rule;

        // These two hold the pictureBox and (partial-)lineChain which we are currently wiring up
        private PictureBox currentlyConnecting;
        public lineChain currentLine;

        private lineChain currentlyDraggingLine = null;         // These two are indexes to the lineChain/point that we are dragging when we drag a handle
        private int currentlyDraggingPoint = -1;

        private lineChain currentlyConextedLine = null;         // These two are indexes to the lineChain/point that we right-clicked on, when showing a context menu
        private int currentlyConextedPoint = -1;

        private readonly Color connectingPinColour = Color.Cyan;
        private Color normalPinColour = Color.Transparent;

        #region rule manipulation

        /// <summary>
        /// Spawn a new ruleItem of the described type
        /// </summary>
        /// <param name="info">Description of the type to create</param>
        public void addRuleItem(ruleItemInfo info)
        {
            // Create our rule item
            ruleItemBase newRuleItem = _rule.addRuleItem(info);

            // add a visual widget for it, and then add it to the visible controls
            ctlRuleItemWidget newCtl = new ctlRuleItemWidget(newRuleItem, this.setTsStatus, false, _rule.pins);
            _rule.AddctlRuleItemWidgetToGlobalPool(newCtl);
            this.Controls.Add(newCtl);
            newCtl.BringToFront();
        }

        /// <summary>
        /// Make rule item controls for every rule item present in our rule.
        /// </summary>
        public void addRuleItemControlsAfterDeserialisation()
        {
            IEnumerable<ruleItemBase> childRuleItems = _rule.getRuleItems();

            foreach (ruleItemBase thisRule in childRuleItems)
            {
                //if (thisRule.pinInfo.Count == 0)
                //    thisRule.claimPinsPostDeSer(myDelegates, _runner.getPins());

                ctlRuleItemWidget newCtl = new ctlRuleItemWidget(thisRule, setTsStatus, true, _rule.pins);
                //hook up line moved events for deserialized control.
                foreach (var pins in newCtl.conPins)
                {
                    lineChain line = _rule.GetLineChainFromGuid(pins.Key.parentLineChain);
                    newCtl.OnRuleItemMoved += line.LineMoved;
                }
                _rule.AddctlRuleItemWidgetToGlobalPool(newCtl);
                Controls.Add(newCtl);
                newCtl.BringToFront();
            }
        }

        public string serialiseRule()
        {
            return _rule.serialise();
        }

        public void deserialiseRule(string serialised)
        {
            _rule = rule.deserialise(serialised);
        }

        #endregion

        #region logic to wire up lineChain wires

        public void startOrFinishLine(PictureBox startFrom)
        {
            if (currentlyConnecting == null)   
            {
                // user has just started a drag
                startLine(startFrom);
            }
            else
            {
                // user has finished a drag
                finishLine(startFrom);
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

            currentlyConnecting = from;
            currentlyConnecting.BackColor = connectingPinColour;
            currentLine = new lineChain();
            currentLine.start = PointToClient(Control.MousePosition);
            // set temporarily, so the line doesn't stretch to the origin until the first mouseMove
            currentLine.end = PointToClient(Control.MousePosition);             
        }

        public void finishLine(PictureBox endTarget)
        {
            ctlRuleItemWidget sourceItemWidget;

            pin source;
            pin dest;

            // Which direction has the user drawn a line in?
            if (((pin)currentlyConnecting.Tag).direction == pinDirection.output)
            {
                sourceItemWidget = ((ctlRuleItemWidget)currentlyConnecting.Parent);

                source = (pin)currentlyConnecting.Tag;
                dest = (pin)endTarget.Tag;
            }
            else
            {
                // the user has drawn the line from destination to source.
                currentLine.isdrawnbackwards = true;
                sourceItemWidget = ((ctlRuleItemWidget)endTarget.Parent);

                dest = (pin)currentlyConnecting.Tag;
                source = (pin)endTarget.Tag;
            }

            currentlyConnecting.BackColor = normalPinColour;  // erase highlight

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

            if(errored)
            {
                currentLine = new lineChain();
                source.parentLineChain.id = Guid.Empty;
                currentlyConnecting = null;
                return;
            }
            
            // Hook pins up
            currentLine.OnLineDeleted += source.Disconnected;
            currentLine.OnLineDeleted += dest.Disconnected;
            dest.connectTo(currentLine.serial, source);
            source.connectTo(currentLine.serial, dest);
            
            // hook ruleitem events up to the line
            ctlRuleItemWidget ruleEnd = ((ctlRuleItemWidget) endTarget.Parent);
            ctlRuleItemWidget ruleStart = ((ctlRuleItemWidget) currentlyConnecting.Parent);
            ruleEnd.OnRuleItemMoved += currentLine.LineMoved;
            ruleStart.OnRuleItemMoved += currentLine.LineMoved;

            _rule.AddLineChainToGlobalPool(currentLine);
            
            currentLine = new lineChain();

            ruleEnd.alignWires();
            ruleStart.alignWires();

            currentlyConnecting = null;
        }

        /// <summary>
        /// Determine if the current mouse cursor is hovering over a draggable handle.
        /// </summary>
        /// <returns></returns>
        private bool isHandleUnderPointer()
        {
            Point cursor = PointToClient(Control.MousePosition);

            IEnumerable<lineChain> childLines = _rule.getNonDeletedLineChains();
            foreach (lineChain aLine in childLines )
            {
                if (isHandleUnderPoint(aLine.start, cursor))
                    return true;

                if (isHandleUnderPoint(aLine.end, cursor))
                    return true;

                foreach (Point aPoint in aLine.midPoints)
                {
                    if (isHandleUnderPoint(aPoint, cursor))
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
            currentlyConextedLine = null;
            currentlyConextedPoint = -1;

            IEnumerable<lineChain> lines = _rule.getNonDeletedLineChains();
            foreach (lineChain aLine in lines)
            {

                // First, see if the cursor is over a draggable midPoint, and start 
                // dragging it if so.
                int pointIndex = 0;
                foreach (Point aPoint in aLine.midPoints)
                {
                    if (isHandleUnderPoint(aPoint, cursor))
                    {
                        currentlyConextedLine = aLine ;
                        currentlyConextedPoint = pointIndex;
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
                if (isHandleUnderPoint(aLine.start, cursor))
                {
                    currentlyConextedLine = aLine;
                    return;
                }

                if (isHandleUnderPoint(aLine.end, cursor))
                {
                    currentlyConextedLine = aLine;
                    return;
                }
            }

            throw new Exception("Unable to find current Handle");
        }

        private void selectCurrentHandleAsDragging(Point cursor)
        {
            IEnumerable<lineChain> lines = _rule.getNonDeletedLineChains();
            foreach (lineChain aLine in lines)
            {
                int pointIndex = 0;

                foreach (Point aPoint in aLine.midPoints)
                {
                    if (isHandleUnderPoint(aPoint, cursor))
                    {
                        currentlyDraggingLine = aLine;
                        currentlyDraggingPoint = pointIndex;
                        return;
                    }
                    pointIndex++;
                }
            }
        }

        public void deleteRuleItem(ctlRuleItemWidget toDelete)
        {
            _rule.deleteRuleItem(toDelete.targetRuleItem);

            // remove the associated ctlRuleItem
            _rule.deleteCtlRuleItem(toDelete);

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
            IEnumerable<lineChain> lines = _rule.getNonDeletedLineChains();
            foreach (lineChain aLine in lines)
                aLine.draw( Graphics.FromHwnd(this.Handle)  );

            if (currentlyConnecting != null)
                currentLine.draw(Graphics.FromHwnd(this.Handle));
        }

        private void FrmRule_Click(object sender, EventArgs e)
        {
            if (currentlyConnecting != null)
            {
                // We are currently drawing a wire. Add a line segment.
                currentLine.midPoints.Add(PointToClient(Control.MousePosition));
            } 
            else if ( e.GetType() == typeof(MouseEventArgs) &&  ((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right  )
            {
                // Right-click has occured! I wonder where?
                if (isHandleUnderPointer())
                {
                    // user has right-clicked a handle. Find which one is under the pointer
                    selectCurrentHandleAsContexted(((MouseEventArgs)e).Location);

                    // If the point in question isn't found by the above, it is a start or end handle, and thus is undeletable.
                    if (currentlyConextedPoint == -1)
                        mnuItemDelJunc.Enabled = false;
                    else
                        mnuItemDelJunc.Enabled = true;

                    // and show the menu in the appropriate place
                    mnuStripHandles.Show();
                    mnuStripHandles.Location = (Control.MousePosition);
                }
            }
        }

        private void FrmRule_MouseDown(object sender, MouseEventArgs e)
        {
            if (isHandleUnderPointer())
            {
                // cursor is over a handle
                this.Cursor = Cursors.Hand;
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // User is attempting to drag a wire corner handle. Find which one
                    selectCurrentHandleAsDragging(e.Location);
                }
            }
        }

        #endregion

        #region junction context-menu handlers

        private void FrmRule_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentlyConnecting != null)
            {
                // if we're currently drawing a line, then, er, draw it to the cursor
                setTsStatus("Click to create a 'corner' in the wire, or click on destination arrow to end");
                currentLine.end = PointToClient(Control.MousePosition);
                this.Invalidate(false);
            }
            else
            {
                if (currentlyDraggingPoint != -1 && e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // User is attempting to drag a wire corner handle
                    setTsStatus("");
                    currentlyDraggingLine.midPoints[currentlyDraggingPoint] = PointToClient(Control.MousePosition);
                    this.Invalidate(false);
                }
                else
                {
                    if (isHandleUnderPointer())
                    {
                        setTsStatus("Drag to move this wire junction, or right-click for options pertaining to it");
                        this.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        setTsStatus("");
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void FrmRule_MouseUp(object sender, MouseEventArgs e)
        {
            currentlyDraggingPoint = -1;
            currentlyDraggingLine = null;
        }

        private void mnuItemDelJunc_Click(object sender, EventArgs e)
        {
            currentlyConextedLine.midPoints.RemoveAt(currentlyConextedPoint);
            this.Invalidate();
        }

        private void mnuItemAddJunc_Click(object sender, EventArgs e)
        {
            Point prevPoint;
            Point nextPoint;
            if (currentlyConextedPoint == 0)
            {
                prevPoint = currentlyConextedLine.start;
                nextPoint = currentlyConextedLine.midPoints[currentlyConextedPoint];
            }
            else if (currentlyConextedPoint == -1)
            {
                // the line we are manipulating has only start and end points, no midpoints. 
                prevPoint = currentlyConextedLine.start;
                nextPoint = currentlyConextedLine.end;
            }
            else if (currentlyConextedLine.midPoints.Count > currentlyConextedPoint +1)
            {
                prevPoint = currentlyConextedLine.midPoints[currentlyConextedPoint];
                nextPoint = currentlyConextedLine.midPoints[currentlyConextedPoint + 1];
            } 
            else 
            {
                prevPoint = currentlyConextedLine.midPoints[currentlyConextedPoint];
                nextPoint = currentlyConextedLine.end;
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

            if (currentlyConextedPoint < 1 )
                currentlyConextedLine.midPoints.Insert(0 , newPoint);
            else
                currentlyConextedLine.midPoints.Insert(currentlyConextedPoint + 1 , newPoint);

            this.Invalidate();
        }

        private void mnuItemDelLine_Click(object sender, EventArgs e)
        {
            currentlyConextedLine.deleteSelf();
            this.Invalidate();
        }

        #endregion

        public void stop()
        {
            if (_rule.isRunning)
            {
                this.Enabled = true;
                toolStripProgressBar.MarqueeAnimationSpeed = 0;
                toolStripProgressBar.Visible = false;

                _rule.stop();
            }
        }

        public void start()
        {
            if (!_rule.isRunning)
            {
                this.Enabled = false;
                toolStripProgressBar.MarqueeAnimationSpeed = 100;
                toolStripProgressBar.Visible = true;

                _rule.start();
            }
        }

        private void showDebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDebug(currentlyConextedLine).Show();
        }

        public rule getRule()
        {
            return _rule;
        }

        public void loadRule(rule toLoad)
        {
            stop();
            _rule = toLoad;

            addRuleItemControlsAfterDeserialisation();
        }
    }
}
