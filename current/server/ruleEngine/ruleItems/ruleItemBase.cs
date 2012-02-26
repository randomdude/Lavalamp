using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.ruleItems.windows;

namespace ruleEngine.ruleItems
{
    [XmlRoot("config" )]
    public abstract class ruleItemBase : ruleItemEvents
    {
        [XmlIgnore] public ruleItemGuid serial = new ruleItemGuid() { id = Guid.NewGuid() };
        [XmlIgnore] private PictureBox _errorIcon = new PictureBox();
        [XmlIgnore] public bool isErrored = false;
        [XmlIgnore] public Exception whyIsErrored;
        [XmlIgnore] public Point location = new Point(0, 0);
        [XmlIgnore] public List<Control> controls = new List<Control>();

        /// <summary>
        /// Pin objects on this ruleItem, indexed by pin name
        /// </summary>
        [XmlIgnore] public Dictionary<string, pin> pinInfo = new Dictionary<string, pin>();

        /// <summary>
        /// Is this ruleItem currently permitted to evaluate()?
        /// </summary>
        [XmlIgnore] public bool isEnabled
        {
            get { return _isEnabled;  }
            private set { _isEnabled = value; }
        }

        [XmlIgnore] private bool _isEnabled;

        // methods to be overridden by the new ruleItem
        public virtual Size preferredSize() { return new Size(75, 75); }
        public abstract string ruleName();
        public virtual Dictionary<String, pin> getPinInfo() { return new Dictionary<String,pin>(); }
        public abstract void evaluate();
        public virtual Image background() { return null; }
        public virtual void start() { }
        public virtual void stop() { }
        public virtual void onResize(Control parent) { }
        public virtual string caption() { return null; }
        /// <summary>
        /// The default options for the rule which will be displayed when the ruleItemCtl is double clicked
        /// </summary>
        /// <returns>the options form</returns>
        public virtual Form ruleItemOptions() { return null; }

        public virtual void onAfterLoad() { }

        public delegate void changeNotifyDelegate();
        public delegate void evaluateDelegate() ;
        public delegate void errorDelegate(Exception ex);

        [XmlIgnore] public bool isDeleted = false;

        protected ruleItemBase()
        {
            // Control stuff 
            Size currentPreferredSize = preferredSize();

            _errorIcon.Image = Properties.Resources.error.ToBitmap();
            _errorIcon.Size = _errorIcon.Image.Size;
            _errorIcon.Visible = false;
            _errorIcon.Left = currentPreferredSize.Width - _errorIcon.Width;
            _errorIcon.Top = currentPreferredSize.Height - _errorIcon.Height;
            _errorIcon.Cursor = Cursors.Help;
            _errorIcon.Click += new EventHandler(errorIcon_Click);

            controls.Add(_errorIcon);

            // caption label
            String currentCaption = this.caption();
            if (currentCaption != null)
            {
                Label lblCaption = new Label();
                lblCaption.Text = currentCaption;
                lblCaption.Visible = true;
                lblCaption.Location = new Point(0, 0);
                lblCaption.Width = currentPreferredSize.Width;
                lblCaption.Height = currentPreferredSize.Height - 15;
                lblCaption.TextAlign = ContentAlignment.BottomCenter;
                lblCaption.BackColor = Color.Transparent;

                controls.Add(lblCaption);
            }
        }

        public void errorIcon_Click(object sender, EventArgs e)
        {
            if (whyIsErrored != null)
            {
                frmException exceptionWindow = new frmException(whyIsErrored);

                exceptionWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("This control is not in an error state. No error has occurred since the last run.");
            }
        }

        /// <summary>
        /// Get pin info from the inheriting ruleItem. Call this only once, as it creates new Pins and thus Pin GUIDs.
        /// </summary>
        public void initPins()
        {
            pinInfo = getPinInfo();
            foreach (pin thisPinInfo in pinInfo.Values)
                thisPinInfo.createValue(this);
        }

        public void errorHandler(Exception ex)
        {
            // Feel free to use a different error handler in your ruleItems (use .setErrorHandler).

            this.isErrored = true;
            this.whyIsErrored = ex;
            this.isEnabled = false;

            if (_errorIcon.Parent == null)
            {
                // Oh crap! We've got no window - we're probably running a unit test?
                // todo/fixme: what happens when we run rules without a UI?
                throw ex;
            }

            _errorIcon.Invoke( () => _errorIcon.BringToFront());
            _errorIcon.Invoke( () => _errorIcon.Visible = true );
        }

        public virtual ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while(strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            ToolStripMenuItem newItem = new ToolStripMenuItem("Show error detail..");
            newItem.Click += new EventHandler(errorIcon_Click);
            toRet.Items.Add(newItem);

            return toRet;
        }

        public void clearErrors()
        {
            isErrored = false;
            isEnabled = true;
            _errorIcon.Visible = false;
        }

        /// <summary>
        /// After deserialisation, we have no pins, and they are in the global pins()
        /// list. This function claims them.
        /// </summary>
        /// <param name="pins">Global dictionary of pins</param>
        public void claimPinsPostDeSer(Dictionary<string,pin> pins )
        {
            // Since we've just been deserialised, we need to initialise the pins that the ruleItem uses.
            // We do this by going through each pin, checking if it's one of ours, and if it is, adding
            // an entry in pinInfo.
            foreach (pin thisPin in pins.Values)
            {
                if (thisPin.parentRuleItem.ToString() == serial.ToString())
                {
                    thisPin.createValue(this);
                    pinInfo.Add(thisPin.name, thisPin);

                    // Wire up the pin to do stuff when activated, if necessary.
                    if (thisPin.isConnected && thisPin.direction == pinDirection.output)
                    {
                        pin dest = pins[thisPin.linkedTo.id.ToString()];
                        thisPin.OnPinChange += dest.stateChanged;
                    }
                }
            }
        }
    }

    public class ruleItemEvents
    {
        /// <summary>
        /// Fired when we are inserting a new timeline event to the next delta
        /// </summary>
        public event timelineEventHandlerDelegate newTimelineEvent;
        public delegate void timelineEventHandlerDelegate(ruleItemBase sender, timelineEventArgs e);

        public void onRequestNewTimelineEvent(timelineEventArgs e)
        {
            if (newTimelineEvent != null)
            {
                newTimelineEvent.Invoke((ruleItemBase) this, e);
            }
        }

        public event requestNewTimelineEventInFutureDelegate newTimelineEventInFuture;
        public delegate void requestNewTimelineEventInFutureDelegate(ruleItemBase sender, timelineEventArgs e, int timeBeforeEvent);

        public void onRequestNewTimelineEventInFuture(timelineEventArgs e, int timeBeforeEvent)
        {
            if (newTimelineEventInFuture != null)
            {
                newTimelineEventInFuture.Invoke((ruleItemBase)this, e, timeBeforeEvent);
            }
        }
        
    }

    public class timelineEventArgs : EventArgs
    {
         public timelineEventArgs()
         {
             newValue = null;
         }
        public timelineEventArgs(IPinData val)
        {
            newValue = val;
        }
        public IPinData newValue;
    }
}

