using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Diagnostics.Contracts;

namespace ruleEngine.ruleItems
{
    [XmlRoot("config" )]
    public abstract class ruleItemBase : ruleItemEvents, IRuleItem, IFormOptions
    {
        [XmlIgnore] public ruleItemGuid serial = new ruleItemGuid() { id = Guid.NewGuid() };
      //  [XmlIgnore] private PictureBox _errorIcon = new PictureBox();
        [XmlIgnore] public bool isErrored = false;
        [XmlIgnore] public Exception whyIsErrored;

    //    [XmlIgnore] public List<Control> controls = new List<Control>();
        [XmlIgnore] private Image _backgroundImage;


        /// <summary>
        /// Pin objects on this ruleItem, indexed by pin name
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, pin> pinInfo { get; set; }

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

        /// <summary>
        /// Called when the tool box item is registered
        /// use to check requirements of the rule
        /// </summary>
        public virtual void onRegister()
        {

        }

        /// <summary>
        /// catches any errors caused be evalate (and todo runs rule in sandbox?)
        /// </summary>
        /// <param name="throwErrors">if the method rethrows errors</param>
        public void evaluate(bool throwErrors)
        {
            try
            {
                evaluate();
            }
            catch (Exception ex)
            {
                errorHandler(ex);
                if (throwErrors)
                    throw ex;
            }

        }
        public virtual Image background() { return _backgroundImage; }
        public virtual void setBackground(Image image)
        {
         //   _backgroundBox.Image = image;
         //   _backgroundBox.Refresh();
           
        }
        public virtual void start() { }
        public virtual void stop() { }
        public virtual void onResize(Control parent) { }
        public virtual string caption() { return string.Empty; }
        
        public string category()
        {
            Type tpy = this.GetType();
            if (tpy.IsDefined(typeof(ToolboxRuleCategoryAttribute), true))
            {
                ToolboxRuleCategoryAttribute toolbox = (ToolboxRuleCategoryAttribute)tpy.GetCustomAttributes(typeof(ToolboxRuleCategoryAttribute), true)[0];
                return toolbox.name;
            }
            return string.Empty;
        }

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
        [XmlIgnore]
        public Point location{get; set; }

        public string displayName
        {
            get
            {
                return ruleName();
            }
        }

        public abstract string typedName
        {
            get;
        }

        public virtual void onAfterLoad() { }

        public virtual void onOptionsChanged(object sender, EventArgs eventArgs)
        {
            if (optionsChanged != null)
                optionsChanged(sender, eventArgs);


        }

        public delegate void changeNotifyDelegate();
        public delegate void evaluateDelegate() ;
        public delegate void errorDelegate(Exception ex);

        [XmlIgnore] public bool isDeleted = false;

        public event EventHandler optionsChanged;

        protected ruleItemBase() { 
            pinInfo = new Dictionary<string, pin>();
        }

        protected ruleItemBase(Image backgroundImage)
        {
            _backgroundImage = backgroundImage;
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

        }

        public virtual ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while(strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            ToolStripMenuItem newItem = new ToolStripMenuItem("Show error detail..");
         //   newItem.Click += new EventHandler(errorIcon_Click);
            toRet.Items.Add(newItem);

            return toRet;
        }

        public void clearErrors()
        {
            isErrored = false;
            isEnabled = true;
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

        public void setChanged()
        {
            throw new NotImplementedException();
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

