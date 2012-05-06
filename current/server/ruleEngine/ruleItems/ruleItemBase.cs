using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

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
        [XmlIgnore] private Image _backgroundImage;

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
        public virtual Image background() { return _backgroundImage; }
        public virtual void setBackground(Image image)
        {
            _backgroundBox.Image = image;
            _backgroundBox.Refresh();
           
        }
        public virtual void start() { }
        public virtual void stop() { }
        public virtual void onResize(Control parent) { }
        public virtual string caption() { return null; }

        /// <summary>
        /// The default options for the rule which will be displayed when the ruleItemCtl is double clicked
        /// </summary>
        /// <returns>the options form</returns>
        public IFormOptions ruleItemOptions()
        {
            IFormOptions options = this.setupOptions();
            if (options != null)
                options.optionsChanged += onOptionsChanged;
            return options;
        }

        public abstract IFormOptions setupOptions();

        public virtual void onAfterLoad() { }

        public virtual void onOptionsChanged(object sender, EventArgs eventArgs)
        {
            _lblCaption.Text = this.caption() ?? "";
            this._lblCaption.Visible = this._lblCaption.Text != "";
            if (_backgroundImage != null)
            {
                _backgroundBox.Image = _backgroundImage;
                _backgroundBox.Visible = true;
                _backgroundBox.Left = (preferredSize().Width / 2) - (_backgroundImage.Width / 2);
                _backgroundBox.Invalidate();
            }
            else 
            {
                _backgroundBox.Visible = false;
            }
            _lblCaption.Invalidate();
            
        }

        public delegate void changeNotifyDelegate();
        public delegate void evaluateDelegate() ;
        public delegate void errorDelegate(Exception ex);

        [XmlIgnore] public bool isDeleted = false;
        private PictureBox _backgroundBox;

        private Label _lblCaption;

        protected ruleItemBase()
        {
            // Control stuff TODO this needs movinvg to the rulectlwidget
            Size currentPreferredSize = preferredSize();

            _errorIcon.Image = Properties.Resources.error.ToBitmap();
            _errorIcon.Size = _errorIcon.Image.Size;
            _errorIcon.Visible = false;
            _errorIcon.Left = currentPreferredSize.Width - _errorIcon.Width;
            _errorIcon.Top = currentPreferredSize.Height - _errorIcon.Height;
            _errorIcon.Cursor = Cursors.Help;
            _errorIcon.Click += this.errorIcon_Click;

            controls.Add(_errorIcon);

            // Load up background. We put this in a PictureBox instead of the control background so that
            // we can position it - we want it slightly above the center of the image, so that it does not
            // foul the ruleItem's caption.
            Image bg = background();

            _backgroundBox = new PictureBox();
            // We keep the background image the size of the bitmap passed in, just so that the ruleItem
            // can keep control of it that way.
            // We position the image at the middle-top. We leave a 3px border at the top so it looks a
            // bit nicer.
            if (bg != null)
            {
                _backgroundBox.Image = bg;
                _backgroundBox.Left = (preferredSize().Width / 2) - (bg.Width / 2);
                _backgroundBox.Visible = true;
            }
            else 
                _backgroundBox.Visible = false;
            
            _backgroundBox.SizeMode = PictureBoxSizeMode.AutoSize;
            
            
            _backgroundBox.Top = 3;

            _backgroundBox.BackColor = Color.Transparent;
            controls.Add(_backgroundBox);


            // caption label even if not set it maybe set by a rule item lators
            String currentCaption = this.caption() ?? "";
            _lblCaption = new Label
                {
                    Text = currentCaption,
                    Visible = currentCaption != "",
                    Location = new Point(0, 0),
                    Width = currentPreferredSize.Width,
                    Height = currentPreferredSize.Height - 15,
                    TextAlign = ContentAlignment.BottomCenter,
                    BackColor = Color.Transparent
                };
            controls.Add(_lblCaption);
        }

        protected ruleItemBase(Image backgroundImage)
        {
            _backgroundImage = backgroundImage;
        }

        public void errorIcon_Click(object sender, EventArgs e)
        {
            if (whyIsErrored != null)
            {
                //frmException exceptionWindow = new frmException(whyIsErrored);
                 
              //  exceptionWindow.ShowDialog();
            }
            else
            {
              //  MessageBox.Show("This control is not in an error state. No error has occurred since the last run.");
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
        /// After deserialisation, the pins aren't connected 
        /// This function connections them.
        /// </summary>
        public void hookPinConnectionsUp(IEnumerable<ruleItemBase> ruleItems)
        {
            foreach (var pi in pinInfo.Values.Where(x => x.direction == pinDirection.output))
            {
                bool pinConnected = false;
                foreach (ruleItemBase d in ruleItems)
                {
                    foreach (var pd in d.pinInfo.Values)
                    {
                        if (pi.linkedTo == pd.serial)
                        {
                            pi.OnPinChange += pd.stateChanged;
                            pinConnected = true;
                            break;
                        }
                    }
                    if (pinConnected)
                        break;
                }
            }
            foreach (pin thisPinInfo in pinInfo.Values)
                thisPinInfo.createValue(this);
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

