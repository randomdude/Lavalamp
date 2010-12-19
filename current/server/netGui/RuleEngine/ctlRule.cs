using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    [Serializable]
    public partial class ctlRule : UserControl
    {
        public ctlRule()
        {
            InitializeComponent();
            targetRule = new rule();

            commonConstructorStuff();
        }

        void commonConstructorStuff()
        {
            currentLine = new lineChain(myDelegates);            
#if DEBUG
            showDebugInfoToolStripMenuItem.Visible = true;
#endif
        }

        public delegatePack myDelegates
        {
            get { return targetRule.generateDelegates(); }
        }

        private bool running = false;

        public rule targetRule;

        private PictureBox currentlyConnecting;         // These three hold the pictureBox and (partial-)lineChain which we are currently wiring up
        public lineChain currentLine;

        private lineChain currentlyDraggingLine = null;         // These two are indexes to the lineChain/point that we are dragging when we drag a handle
        private int currentlyDraggingPoint = -1;

        private lineChain currentlyConextedLine = null;         // These two are indexes to the lineChain/point that we right-clicked on, when showing a context menu
        private int currentlyConextedPoint = -1;


        #region rule manipulation

        public void addRuleItem(ruleItemInfo info)
        {
            // Make new ruleItem control of this RuleItem type
            ruleItemBase newRuleItem;
            if (info.itemType == ruleItemType.RuleItem)
            {
                ConstructorInfo constr = info.ruleItemBaseType.GetConstructor(new Type[0]);
                newRuleItem = (ruleItemBase) constr.Invoke(new object[0] { });
            }
            else if (info.itemType == ruleItemType.PythonFile)
            {
                newRuleItem = new ruleItem_python(new pythonEngine(info.pythonFileName));
            }
            else
                throw new Exception("eh? Unrecognised file type?");

            // Initialise the Pins on the control. This will generate a new guid for each pin.
            newRuleItem.initPins();

            ctlRuleItemWidget newCtl = new ctlRuleItemWidget(newRuleItem , myDelegates, this.setTsStatus, false, targetRule.pins);
            myDelegates.AddRuleItemToGlobalPool(newCtl.targetRuleItem);
            myDelegates.AddctlRuleItemWidgetToGlobalPool(newCtl);
            this.Controls.Add(newCtl);
            newCtl.BringToFront();
        }

        public void addRuleItemControlsAfterDeserialisation()
        {
            // Make rule item controls for each rule
            foreach (ruleItemBase thisRule in this.targetRule.ruleItems.Values)
            {
                ctlRuleItemWidget newCtl = new ctlRuleItemWidget(thisRule as ruleItemBase, myDelegates, this.setTsStatus, true, targetRule.pins);
                myDelegates.AddctlRuleItemWidgetToGlobalPool(newCtl);
                this.Controls.Add(newCtl);
                newCtl.BringToFront();
            }
        }

        public string SerialiseRule()
        {
            return this.targetRule.serialise();
        }

        public void DeserialiseRule(string serialised)
        {
            // Create a new rule, and deserialise in to it.
            XmlSerializer mySer = new XmlSerializer(targetRule.GetType());
            Encoding ascii = Encoding.BigEndianUnicode;
            Stream stream = new MemoryStream(ascii.GetBytes(serialised));
            targetRule = (rule) mySer.Deserialize(stream);
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
            currentlyConnecting.BackColor = Color.Cyan;
            currentLine = new lineChain(myDelegates);
            currentLine.start = PointToClient(Control.MousePosition);
            currentLine.end = PointToClient(Control.MousePosition);             // set temporarily, so the line doesn't stretch to the origin until the first mouseMove
        }

        public void finishLine(PictureBox endTarget)
        {
            ctlRuleItemWidget sourceItemWidget;
            ctlRuleItemWidget destItemWidget;

            pin source;
            pin dest;

            // Which direction has the user drawn a line in?
            if (((pin)currentlyConnecting.Tag).direction == pinDirection.output)
            {
                sourceItemWidget = ((ctlRuleItemWidget)currentlyConnecting.Parent);
                destItemWidget   = ((ctlRuleItemWidget)endTarget.Parent);

                source = (pin)currentlyConnecting.Tag;
                dest = (pin)endTarget.Tag;
            }
            else
            {
                // the user has drawn the line from destination to source.
                currentLine.isdrawnbackwards = true;
                destItemWidget = ((ctlRuleItemWidget)currentlyConnecting.Parent);
                sourceItemWidget = ((ctlRuleItemWidget)endTarget.Parent);

                dest = (pin)currentlyConnecting.Tag;
                source = (pin)endTarget.Tag;
            }

            currentlyConnecting.BackColor = Color.Transparent;  // erase highlight

            if (source.isConnected || dest.isConnected )
            {
                MessageBox.Show("Pin is already connected to something");
                currentLine = new lineChain(myDelegates);
                source.parentLineChain.id = Guid.Empty;
                currentlyConnecting = null;
                return;
            }
            if (source == dest)
            {
                MessageBox.Show("Pin cannot be connected to itself");
                currentLine = new lineChain(myDelegates);
                source.parentLineChain.id = Guid.Empty;
                currentlyConnecting = null;
                return;
            }
            if (source.direction == dest.direction)
            {
                MessageBox.Show(source.direction.ToString() + " pins cannot be connected to other " + source.direction.ToString() + " pins. Wires must connect inputs to outputs.");
                currentLine = new lineChain(myDelegates);
                source.parentLineChain.id = Guid.Empty;
                currentlyConnecting = null;
                return;
            }            
            
            // Hook pins up
            dest.connectTo(currentLine.serial, source.serial);
            source.connectTo(currentLine.serial, dest.serial);

            currentLine.destPin = dest.serial;
            currentLine.sourcePin = source.serial;
            sourceItemWidget.targetRuleItem.addPinChangeHandler(source.name, currentLine.handleStateChange);

            myDelegates.AddLineChainToGlobalPool(currentLine);

            
            currentLine = new lineChain(myDelegates);

            ((ctlRuleItemWidget)endTarget.Parent).alignWires();
            ((ctlRuleItemWidget)currentlyConnecting.Parent).alignWires();

            currentlyConnecting = null;
        }

        private bool isHandleUnderPointer()
        {
            Point cursor = PointToClient(Control.MousePosition);
            foreach (lineChain aLine in targetRule.lineChains.Values )
            {
                if (isHandleUnderPoint(aLine.start, cursor))
                    return true;
                foreach (Point aPoint in aLine.points)
                    if (isHandleUnderPoint(aPoint, cursor))
                        return true;
                if (isHandleUnderPoint(aLine.end, cursor))
                    return true;
            }
            return false;
        }

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
            foreach (lineChain aLine in targetRule.lineChains.Values)
            {
                if (aLine.deleted)
                    continue;

                int pointIndex = 0;
                foreach (Point aPoint in aLine.points)
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

            // If we got here, we didn't find the handle under the pointer. try .start/.ends.
            foreach (lineChain aLine in targetRule.lineChains.Values)
            {
                if (aLine.deleted)
                    continue;
                // If the cursor _is_ hovering over a start or end, don't set currentlyContextedPoint (since there isn't one), just -Line.
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
        }

        private void selectCurrentHandleAsDragging(Point cursor)
        {
            int lineIndex = 0;
            foreach (lineChain aLine in targetRule.lineChains.Values)
            {
                int pointIndex = 0;
                foreach (Point aPoint in aLine.points)
                {
                    if (isHandleUnderPoint(aPoint, cursor))
                    {
                        currentlyDraggingLine = aLine;
                        currentlyDraggingPoint = pointIndex;
                        return;
                    }
                    pointIndex++;
                }
                lineIndex++;
            }
        }

        public void deleteRuleItem(ctlRuleItemWidget toDelete)
        {
            // Remove any lineChains attatched to the to-delete item
            foreach (pin thisPin in toDelete.conPins.Keys)
            {
                if (thisPin.isConnected)
                {
                    lineChain tonuke = targetRule.GetLineChainFromGuid(thisPin.parentLineChain);
                    tonuke.deleteSelf();
                }
            }

            // remove the actual control
            this.Controls.Remove(toDelete);

            // remove the associated ruleItem
            targetRule.ctlRuleItems.Remove(toDelete.serial.id.ToString());

            // remove from our global pool
            toDelete.targetRuleItem.isDeleted = true;

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
            if (targetRule != null && targetRule.lineChains != null)
            {
                foreach (lineChain aLine in targetRule.lineChains.Values)
                    if (!aLine.deleted)
                        aLine.draw( Graphics.FromHwnd(this.Handle)  );
            }

            if (currentlyConnecting != null)
                currentLine.draw(Graphics.FromHwnd(this.Handle));
        }

        private void FrmRule_Click(object sender, EventArgs e)
        {
            if (currentlyConnecting != null)
            {
                // We are currently drawing a wire. Add a line segment.
                currentLine.points.Add(PointToClient(Control.MousePosition));
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
                    currentlyDraggingLine.points[currentlyDraggingPoint] = PointToClient(Control.MousePosition);
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
            currentlyConextedLine.points.RemoveAt(currentlyConextedPoint);
            this.Invalidate();
        }

        private void mnuItemAddJunc_Click(object sender, EventArgs e)
        {
            Point prevPoint;
            Point nextPoint;
            if (currentlyConextedPoint == 0)
            {
                prevPoint = currentlyConextedLine.start;
                nextPoint = currentlyConextedLine.points[currentlyConextedPoint];
            }
            else if (currentlyConextedPoint == -1)
            {
                // the line we are manipulating has only start and end points, no midpoints. 
                prevPoint = currentlyConextedLine.start;
                nextPoint = currentlyConextedLine.end;
            }
            else if (currentlyConextedLine.points.Count > currentlyConextedPoint +1)
            {
                prevPoint = currentlyConextedLine.points[currentlyConextedPoint];
                nextPoint = currentlyConextedLine.points[currentlyConextedPoint + 1];
            } 
            else 
            {
                prevPoint = currentlyConextedLine.points[currentlyConextedPoint];
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
                currentlyConextedLine.points.Insert(0 , newPoint);
            else
                currentlyConextedLine.points.Insert(currentlyConextedPoint + 1 , newPoint);

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
            if (running)
            {
                running = false;
                this.Enabled = true;
                toolStripProgressBar.MarqueeAnimationSpeed = 0;
                toolStripProgressBar.Visible = false;

                targetRule.stop();
            }
        }

        public void start()
        {
            if (!running)
            {
                running = true;
                this.Enabled = false;
                toolStripProgressBar.MarqueeAnimationSpeed = 100;
                toolStripProgressBar.Visible = true;

                targetRule.start();
            }
        }

        private void showDebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDebug(currentlyConextedLine).Show();
        }

    }
}
