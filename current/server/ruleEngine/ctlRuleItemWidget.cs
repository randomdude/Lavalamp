using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    [Serializable]
    public class ctlRuleItemWidget : UserControl
    {
        public readonly Dictionary<pin, PictureBox> conPins = new Dictionary<pin, PictureBox>();
        public ruleItemBase targetRuleItem;

        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private readonly ctlRule.setTsStatusDlg setToolbarText;

        private Point? mouseDownAt = null;
        public readonly ctlRuleItemWidgetGuid serial = new ctlRuleItemWidgetGuid() { id = Guid.NewGuid() };
        private ToolStripMenuItem showDebugInfoToolStripMenuItem;
        public bool snapToGrid;

        /// <summary>
        /// fired when the item is moved
        /// </summary>
        public event ruleItemMoved OnRuleItemMoved;
        public delegate void ruleItemMoved(object sender, ItemMovedArgs args);

        public ctlRuleItemWidget(ruleItemBase newRuleItemBase, ctlRule.setTsStatusDlg newSetToolbarText)
        {
            setToolbarText = newSetToolbarText;

            commonConstructorStuff();
            loadRuleItem(newRuleItemBase);
            try
            {
                ContextMenuStrip = newRuleItemBase.addMenus(contextMenuStrip1);
            }
            catch (NotImplementedException)
            {
                // Fair enough, it has no menus to add.
            }

            // this probably should be done in addMenus on the base class but the code to openOptions isn't there so to avoid repeating code its here
            var opt = newRuleItemBase.ruleItemOptions();
            if (opt != null)
            {
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
                ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Options...",null,openOptions));
            }
        }

        private void commonConstructorStuff()
        {
            InitializeComponent();

            foreach (Control thisCtl in Controls)
            {
                thisCtl.MouseDown += item_MouseDown;
                thisCtl.MouseMove += item_MouseMove;
                thisCtl.MouseUp += item_MouseUp;
            }
            MouseDown += item_MouseDown;
            MouseMove += item_MouseMove;
            MouseUp += item_MouseUp;

#if DEBUG
            showDebugInfoToolStripMenuItem.Visible = true;
#endif
        }

        #region serialisation

        public XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                if (reader.Name == "ruleItems" && reader.NodeType == XmlNodeType.EndElement)
                    keepGoing = false;

                if (reader.Name == "ctlRuleItemWidget")
                {
                    String targetRuleItemTypeString = reader.GetAttribute("type");
                    // todo: reduce code duplication between this and ruleEditor
                    Assembly myAss = Assembly.GetExecutingAssembly();
                    Type targetRuleItemType;
                    try
                    {
                        // Pull type out of myAss
                        targetRuleItemType = myAss.GetType(targetRuleItemTypeString);
                    }
                    catch (ArgumentException)
                    {
                        // todo: prompt user for location
                        throw new Exception("Unable to load ruleItem of type " + targetRuleItemTypeString);
                    }

                    ConstructorInfo targetRuleItemConstructorInfo =
                        targetRuleItemType.GetConstructor(new Type[0]);
                    targetRuleItem = (ruleItemBase) targetRuleItemConstructorInfo.Invoke(new Object[0]);
                }
                if (keepGoing)
                    keepGoing = reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", targetRuleItem.GetType().ToString() );
        }

        #endregion

        #region IO marker stuff

        private void loadRuleItem(ruleItemBase newRuleItem)
        {
            targetRuleItem = newRuleItem;

            // Load up input/output pin icons 
            conPins.Clear();
            foreach (pin thisPin in targetRuleItem.pinInfo.Values)
            {
                addIcon(thisPin);

                // Note down pin as belonging to this ruleItem.
                thisPin.parentRuleItem = targetRuleItem.serial;
            }

            this.Location = newRuleItem.location;
            this.Size = newRuleItem.preferredSize();

            // wire up events
            foreach (PictureBox thisCtl in conPins.Values)
                thisCtl.Click += new EventHandler(onPinClick);
            
            addEvents(newRuleItem.controls);

            // Load any controls that the item wants
            foreach (Control thisCtl in newRuleItem.controls)
            {
                if (thisCtl.GetType() == typeof(ContextMenuStrip))
                {
                    while (((ContextMenuStrip)thisCtl).Items.Count > 0 )
                        this.contextMenuStrip1.Items.Add(((ContextMenuStrip)thisCtl).Items[0]);
                }
                else
                {
                    Controls.Add(thisCtl);
                }
            }
           
            newRuleItem.onResize(this);
            ruleItemBaseForm_Resize(new object(), new EventArgs());
        }

        private void addEvents(IEnumerable<Control> items)
        {
            foreach (Control thisCtl in items)
            {
                if (thisCtl.GetType().BaseType == typeof(UserControl))  // todo: should really recurse baseTypes
                    addEvents(  thisCtl.Controls );

                thisCtl.MouseDown += new MouseEventHandler(item_MouseDown);
                thisCtl.MouseUp += new MouseEventHandler(item_MouseUp);
                thisCtl.MouseMove += new MouseEventHandler(item_MouseMove);
                thisCtl.DoubleClick += openOptions;
            }
        }

        private void addEvents(ControlCollection items)
        {
            foreach (Control thisCtl in items)
            {
                if (thisCtl.GetType().BaseType == typeof(UserControl))  // todo: should really recurse baseTypes
                {
                    addEvents(thisCtl.Controls);
                }
                else
                {
                    thisCtl.MouseDown += new MouseEventHandler(item_MouseDown);
                    thisCtl.MouseUp += new MouseEventHandler(item_MouseUp);
                    thisCtl.MouseMove += new MouseEventHandler(item_MouseMove);
                    thisCtl.DoubleClick += openOptions;
                }
            }
        }

        private void openOptions(object sender, EventArgs e)
        {
            var opts = targetRuleItem.ruleItemOptions();
            if (opts != null)
            {
                opts.ShowDialog(this);
            }
        }

        private void addIcon(pin pin)
        {
            PictureBox thisPinBox = new PictureBox();
            thisPinBox.Image = Properties.Resources.ArrowRight;
            thisPinBox.Size = thisPinBox.Image.Size;
            thisPinBox.Cursor = Cursors.Hand;
            thisPinBox.BackColor = Color.Transparent;
            thisPinBox.MouseMove += new MouseEventHandler(thisPinBox_MouseMove);
            ToolTip tt = new ToolTip();
            tt.SetToolTip(thisPinBox, pin.description);
            conPins.Add(pin, thisPinBox);
            thisPinBox.Tag = pin;
            pin.imageBox = thisPinBox;
            this.Controls.Add(thisPinBox);
        }

        private void thisPinBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.setToolbarText == null)
                return;

            StringBuilder message = new StringBuilder();
            message.Append("Click to connect wire for action '");
            message.Append(((pin)((PictureBox)sender).Tag).description);
            message.Append("'");
            setToolbarText(message.ToString());
        }

        private void ruleItemBaseForm_Resize(object sender, EventArgs e)
        {
            int inputPinCount = 0;
            int outputPinCount = 0;
            foreach (pin thisPin in conPins.Keys)
                if (thisPin.direction == pinDirection.input)
                    inputPinCount++;
                else if (thisPin.direction == pinDirection.output)
                    outputPinCount++;

            int inputPinsSoFar = 0;
            int outputPinsSoFar = 0;
            foreach (KeyValuePair<pin, PictureBox> thiskvp in conPins)
            {
                PictureBox pinPictureBox = thiskvp.Value;
                pin pin = thiskvp.Key;

                if (pin.direction == pinDirection.input)
                {
                    pinPictureBox.Left = 0;
                    pinPictureBox.Top = (inputPinsSoFar + 1) * (this.ClientSize.Height / (inputPinCount + 1));
                    inputPinsSoFar++;
                }
                else if (pin.direction == pinDirection.output)
                {
                    pinPictureBox.Left = (this.ClientSize.Width - pinPictureBox.Width);
                    pinPictureBox.Top = (outputPinsSoFar + 1) * (this.ClientSize.Height / (outputPinCount + 1));
                    outputPinsSoFar++;
                }
                pinPictureBox.Top -= pinPictureBox.Height / 2;
            }
        }

        public void onPinClick(object sender, EventArgs e)
        {
            ((ctlRule)Parent).startOrFinishLine((PictureBox)sender);
        }

        #endregion

        #region dragdrop stuff

        public void item_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownAt = e.Location;
        }

        public void item_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownAt = null;
        }

        public void item_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownAt.HasValue)
            {
                // We only start a drag if we've left the user-set 'dragsize' rectangle
                int offX = Math.Abs(mouseDownAt.Value.X - (e.Location).X);
                int offY = Math.Abs(mouseDownAt.Value.Y - (e.Location).Y);
                if (offX > (SystemInformation.DragSize.Width / 2)
                     || offY > (SystemInformation.DragSize.Height / 2))
                {
                    // OK, we're dragging! Position control around pointer.
                    Point mousePos = this.Parent.PointToClient(this.PointToScreen(e.Location));
                    Point newPos = new Point();
                    newPos.X = mousePos.X - mouseDownAt.Value.X;
                    newPos.Y = mousePos.Y - mouseDownAt.Value.Y;

                    //bounds checking 
                    if ((newPos.X + Width) > Parent.Width)
                        newPos.X = Parent.Width - Width;
                    else if (newPos.X < 0)
                        newPos.X = 0;
                    if ((newPos.Y + Height) > Parent.Height)
                        newPos.Y = Parent.Height - Height;
                    else if (newPos.Y < 0)
                        newPos.Y = 0;


                    // snap to grid if necessary
                    if (snapToGrid)
                    {
                        double roundedX = newPos.X / 10D;
                        double roundedY = newPos.Y / 10D;

                        roundedX = Math.Round(roundedX, 0) * 10;
                        roundedY = Math.Round(roundedY, 0) * 10;

                        newPos = new Point((int) roundedX, (int) roundedY);
                    }

                    this.Location = newPos;
                    this.targetRuleItem.location = newPos;

                    alignWires();
                    this.Parent.Invalidate();
                }
            }
        }
        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showDebugInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1,
            this.showDebugInfoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(156, 70);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.deleteToolStripMenuItem1.Text = "&Delete ruleItem";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // showDebugInfoToolStripMenuItem
            // 
            this.showDebugInfoToolStripMenuItem.Name = "showDebugInfoToolStripMenuItem";
            this.showDebugInfoToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.showDebugInfoToolStripMenuItem.Text = "Show &Debug info";
            this.showDebugInfoToolStripMenuItem.Visible = false;
            this.showDebugInfoToolStripMenuItem.Click += new System.EventHandler(this.showDebugInfoToolStripMenuItem_Click);
            // 
            // ctlRuleItemWidget
            // 
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "ctlRuleItemWidget";
            this.Size = new System.Drawing.Size(75, 75);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctlRuleItemWidget_MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ctlRule) this.Parent).deleteRuleItem(this);
        }

        public void alignWires()
        {
            foreach (pin thisPin in conPins.Keys)
            {
                if (thisPin.isConnected)
                {
                    PictureBox icon = conPins[thisPin];
                    int midIcon = (icon.Height/2) + this.Parent.PointToClient(PointToScreen(icon.Location)).Y;
                    int iconEdge = this.Parent.PointToClient(PointToScreen(icon.Location)).X;
                    if (thisPin.direction == pinDirection.output)
                        iconEdge += icon.Width;
                    else
                        iconEdge -= 3;

                        if (OnRuleItemMoved != null)
                        {
                            ItemMovedArgs arg = new ItemMovedArgs
                                                    {
                                                        point = new Point(iconEdge, midIcon),
                                                        pinDirection = thisPin.direction,
                                                        lineAffected = thisPin.parentLineChain
                                                    };
                            OnRuleItemMoved.Invoke(this, arg);
                        }
                }
            }

        }

        private void ctlRuleItemWidget_MouseMove(object sender, MouseEventArgs e)
        {
            setToolbarText("Right-click for options");
        }

        private void showDebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDebug(this.targetRuleItem).Show();
        }


    }
}
