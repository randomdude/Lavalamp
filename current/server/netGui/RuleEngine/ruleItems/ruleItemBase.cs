using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace netGui.RuleEngine.ruleItems
{
    [XmlRoot("config" )]
    public abstract class ruleItemBase 
    {
        [XmlIgnore] public ruleItemGuid serial = new ruleItemGuid() { id = Guid.NewGuid() };
        [XmlIgnore] private PictureBox errorIcon = new PictureBox();
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
        // TODO: change to public get/private set
        [XmlIgnore] public bool isEnabled;

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

        public delegate void changeNotifyDelegate();
        public delegate void evaluateDelegate() ;
        public delegate void errorDelegate(Exception ex);

        [XmlIgnore] private Dictionary<String, changeNotifyDelegate> pinChangeHandlers =
            new Dictionary<String, changeNotifyDelegate>();

        [XmlIgnore] public bool isDeleted = false;

        protected ruleItemBase()
        {
            // Control stuff (!?)
            Size currentPreferredSize = preferredSize();

            errorIcon.Image = netGui.Properties.Resources.error.ToBitmap();
            errorIcon.Size = errorIcon.Image.Size;
            errorIcon.Visible = false;
            errorIcon.Left = currentPreferredSize.Width - errorIcon.Width;
            errorIcon.Top = currentPreferredSize.Height - errorIcon.Height;
            errorIcon.Cursor = Cursors.Help;
            errorIcon.Click += new EventHandler(errorIcon_Click);

            controls.Add(errorIcon);

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

            if (errorIcon.Parent == null)
            {
                // Oh crap! We've got no window - we're probably running a unit test?
                // todo/fixme: what happens when we run rules without a UI?
                throw ex;
            }
            errorIcon.Invoke( () => errorIcon.BringToFront());

            errorIcon.Invoke( () => errorIcon.Visible = true );
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
            errorIcon.Visible = false;
        }
    }

}

namespace netGui.RuleEngine
{
    public class ToolboxRule : Attribute { }

    public class ToolboxRuleCategoryAttribute : Attribute
    {
        internal string name;

        public ToolboxRuleCategoryAttribute(string newName)
        {
            name = newName;
        }
    }
}
