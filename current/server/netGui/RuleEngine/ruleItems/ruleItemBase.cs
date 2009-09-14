using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using netGui.RuleEngine;

namespace netGui.RuleEngine.ruleItems
{
    public class ruleItemBase : IXmlSerializable
    {
        public ruleItemGuid serial = new ruleItemGuid() {id = Guid.NewGuid()};
        private PictureBox errorIcon = new PictureBox();
        public bool isErrored = false;
        public Exception whyIsErrored;
        public virtual Size preferredSize() { return new Size(75, 75); } 
        public Point location = new Point(0,0);

        // methods to be overriden by the new ruleItem
        public virtual string ruleName()  { return  "base"; }
        public virtual Dictionary<String, pin> getPinInfo() { return new Dictionary<String,pin>(); }
        public virtual void evaluate() { }
        public virtual Image background() { return null; }
        public virtual void start() { }
        public virtual void stop() { }
        public virtual void onResize(Control parent) { }

        public List<Control> controls = new List<Control>();
        public delegate void changeNotifyDelegate();
        public delegate void evaluateDelegate() ;
        public delegate void errorDelegate(Exception ex);

        private Dictionary<String, changeNotifyDelegate> pinChangeHandlers =
            new Dictionary<String, changeNotifyDelegate>();

        public bool isDeleted = false;

        public ruleItemBase()
        {
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

            this.pinStates.evaluate = new evaluateDelegate(evaluate);
            pinStates.setErrorHandler(new errorDelegate(errorHandler));
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

        public void initPins()
        {
            pinStates.pinInfo = getPinInfo();
        }

        public void setPinDefaults()
        {
            // Add default states for pins. TODO: non-bool types? Should probably be in each ruleItem, not just the base.
            foreach (KeyValuePair<string, pin> thisPinInfo in pinStates.pinInfo)
                pinStates.Add(thisPinInfo.Key, false);
        }

        public void addPinChangeHandler (string pinName, changeNotifyDelegate target)
        {
            pinChangeHandlers.Add(pinName, target);
            pinStates.pinChangeHandlers.Add(pinName, target);
        }

        public void removePinChangeHandler(string pinName)
        {
            pinChangeHandlers.Remove(pinName);
            pinStates.pinChangeHandlers.Remove(pinName);
        }

        public triggeredDictionary pinStates = new triggeredDictionary();

        public void errorHandler(Exception ex)
        {
            // Feel free to use a different error handler in your ruleItems (use .setErrorHandler).

            this.isErrored = true;
            this.whyIsErrored = ex;
            this.pinStates.enabled = false;

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
            pinStates.enabled = true;
            errorIcon.Visible = false;
        }

        public virtual string caption() { return null; }

        #region xml serialisation stuff

        public XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            // to be overridden, i guess? finish me.
        }

        public void WriteXml(XmlWriter writer)
        {
            // to be overridden, i guess? finish me.
        }

        #endregion

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
