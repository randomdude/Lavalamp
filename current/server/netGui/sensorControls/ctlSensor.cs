using System;
using System.Threading;
using System.Windows.Forms;
using netGui.sensorControls;

namespace netGui
{
    public partial class ctlSensor : UserControl
    {
        private System.Threading.Timer updateTimer = null;
        private long _updateInterval = 0;

        public readonly Node node;
        public readonly Int16 targetSensorIndex ;

        private sensor targetSensor
        {
            get
            {
                return node.sensors[targetSensorIndex];
            }
        }

        private long timerInterval
        {
            get
            {
                return _updateInterval;
            }

            set
            {
                _updateInterval = value;
                if (value != 0)
                {
                    // re-init the timer
                    if (null != updateTimer)
                        updateTimer.Dispose();
                    TimerCallback myTimerCallback = new TimerCallback(this.doUpdateNow);
                    updateTimer = new System.Threading.Timer(myTimerCallback, new object(), 0, value);
                } 
                else
                {
                    updateTimer.Dispose();
                    updateTimer = null;
                }
            }
        }

        #region delegates

        public delegate void setStatusDelegateType(String toThis);
        private setStatusDelegateType setStatusDelegate;
        private void setStatusMethod(String toThis)
        {
            this.lblStatus.Text = toThis;
        }
        private void SafelySetStatus(String toThis)
        {
            if (this.statusStrip1.InvokeRequired)
                this.Invoke(setStatusDelegate, new object[] { toThis });
            else
                setStatusMethod(toThis);
        }

        private void initDelegates()
        {
            setStatusDelegate = new setStatusDelegateType(setStatusMethod);
        }
        #endregion

        #region constructors
        
        public ctlSensor()
        {
            // Never call this constructor outside the VS IDE!
            InitializeComponent();
        }

        public ctlSensor(Node newNode, short newSensorIndex)
        {
            InitializeComponent();

            node = newNode;
            targetSensorIndex = newSensorIndex;

            lblTitle.Text = node.name + " : " + targetSensorIndex.ToString();
            lblType.Text = newNode.sensors[newSensorIndex].type.FriendlyType;

            sharedInit();
        }

        private void sharedInit()
        {
            // Things that all constructors use
            initDelegates();
        }
        #endregion

        #region boring UI stuff

        private void showTypeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            lblType.Visible = ((ToolStripMenuItem)sender).Checked;
        }
        private void showIDToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Visible = ((ToolStripMenuItem)sender).Checked;
        }
        private void showStatusToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            statusStrip1.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void MnuItemInupdateFreq_Clicked(object sender, EventArgs e)
        {
            this.MnuItemupdateFreq_none.Checked = false;
            this.MnuItemupdateFreq_1s.Checked = false;
            this.MnuItemupdateFreq_5s.Checked = false;
            this.MnuItemupdateFreq_10s.Checked = false;
            this.MnuItemupdateFreq_60s.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true;

            if (MnuItemupdateFreq_1s.Checked)
                timerInterval = 1000;
            else if (MnuItemupdateFreq_5s.Checked)
                timerInterval = 5 * 1000;
            else if (MnuItemupdateFreq_10s.Checked)
                timerInterval = 10 * 1000;
            else if (MnuItemupdateFreq_60s.Checked)
                timerInterval = 60 * 1000;
            else
                timerInterval = 0;
        }

        public void adjustControls()
        {
            int midcontrolborder = 5;
            int topcursor = 0;

            if (lblType.Visible)
            {
                lblType.Top = topcursor;
                topcursor += lblType.Height + midcontrolborder;
            }
            if (lblTitle.Visible)
            {
                lblTitle.Top = topcursor;
                topcursor += lblTitle.Height + midcontrolborder;
            }
            if (_graphTarget.Visible)
            {
                _graphTarget.Top = topcursor + midcontrolborder;
            }
            _graphTarget.Height = this.Height - topcursor - 10;  // adjust for control border

            if (statusStrip1.Visible)
            {
                _graphTarget.Height -= (statusStrip1.Height + midcontrolborder );
            }

            lblType.Width = this.ClientRectangle.Width - (midcontrolborder * 2);
            lblStatus.Width = this.ClientRectangle.Width - (midcontrolborder * 2);
            lblTitle.Width = this.ClientRectangle.Width - (midcontrolborder * 2);
            _graphTarget.Width = this.ClientRectangle.Width - (midcontrolborder *2);

            lblType.Left  = (this.ClientRectangle.Width / 2) - (lblType.Width / 2);
            lblTitle.Left = (this.ClientRectangle.Width / 2) - (lblTitle.Width / 2);
            _graphTarget.Left = (this.ClientRectangle.Width / 2) - (_graphTarget.Width / 2);
        }

        private void updateNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Disposing)
                doUpdateNow(new object());
        }

        private void lblType_VisibleChanged(object sender, EventArgs e)
        {
            adjustControls();
        }

        private void lblTitle_VisibleChanged(object sender, EventArgs e)
        {
            adjustControls();
        }

        private void statusStrip1_VisibleChanged(object sender, EventArgs e)
        {
            adjustControls();
        }

        #endregion

        public void sendValueToNode(Object sendThis)
        {
            SafelySetStatus ( "updating.." );

            try
            {
                this.node.setValue(this.targetSensorIndex, sendThis, true);
                SafelySetStatus("last update OK");
            }
            catch (sensorException )
            {
                SafelySetStatus ( "last update failed");
                graphTarget.SetError("Not applicable");
            }
            catch (commsPortStateException)
            {
                SafelySetStatus("last update failed");
                graphTarget.SetError("Port is no longer open");
            }
            catch (Exception e)
            {
                SafelySetStatus("last update failed");
                graphTarget.SetError(e.Message);
            }
        }

        public ctlSensor copyOf()
        {
            ctlSensor newMe = new ctlSensor(this.node, this.targetSensorIndex);

            newMe.lblStatus.Text = lblStatus.Text;
            newMe.lblStatus.ForeColor = lblStatus.ForeColor;
            newMe.lblStatus.Visible = lblStatus.Visible;
            
            newMe.lblTitle.Text = lblTitle.Text;
            newMe.lblTitle.ForeColor = lblTitle.ForeColor;
            newMe.lblTitle.Visible = lblTitle.Visible;

            newMe.lblType.Text = lblType.Text;
            newMe.lblType.ForeColor = lblType.ForeColor;
            newMe.lblType.Visible = lblType.Visible;

            newMe.statusStrip1.Visible = statusStrip1.Visible;
            newMe._graphTarget.Visible = _graphTarget.Visible;

            // TODO: Write code to copy each graph state to the new sensor

            // Now, copy the state of the menu items. Note that handlers for each checked item are _not_ called.
            recursivelyCopyMenuStates(this.contextMenuStrip1.Items, newMe.contextMenuStrip1.Items );

            newMe.adjustControls();

            if (0 != this.timerInterval)
                newMe.timerInterval = this.timerInterval;

            return newMe;
        }

        private void recursivelyCopyMenuStates(ToolStripItemCollection source, ToolStripItemCollection dest)
        {
            foreach(ToolStripItem thisItem in source)
            {
                if (thisItem.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem  newItem = (ToolStripMenuItem)dest[thisItem.Name];
                    ToolStripMenuItem  oldItem = (ToolStripMenuItem)thisItem;
                    newItem.Checked = oldItem.Checked;
                    if (oldItem.HasDropDownItems)
                    {
                        recursivelyCopyMenuStates( ((ToolStripDropDownItem)oldItem).DropDownItems, (ToolStripItemCollection)newItem.DropDownItems);
                    }
                }
            }
        }

        private void ctlSensor_Load(object sender, EventArgs e)
        {
            // todo: make this work at design time
            sensorTypeEnum thisSensorType = this.node.sensors[this.targetSensorIndex].type.enumeratedType;
            
            if (sensorTypeEnum.generic_digital_in == thisSensorType)
                graphTarget = new ctlReadout();
            else if (sensorTypeEnum.generic_digital_out == thisSensorType)
                graphTarget = new ctlOnOff();
            else if (sensorTypeEnum.pwm_out == thisSensorType)
                graphTarget = new ctlPWM();
            else if (sensorTypeEnum.triac_out == thisSensorType)
                graphTarget = new ctlPWM();
            else
                graphTarget = new ctlOnOff();       // just guess.
            
        }

        public void doUpdateNow(object notUsed)
        {
            SafelySetStatus ( "updating.." );

            // Send the new value to the overridden UpdateValue method.
            object setToThis;
            try
            {
                setToThis = targetSensor.getValue(true);
                graphTarget.UpdateValue(setToThis);
                SafelySetStatus("last update OK");
            }
            catch (sensorException )
            {
                SafelySetStatus ( "last update failed");
                graphTarget.SetError("Not applicable");
            }
            catch (commsPortStateException)
            {
                SafelySetStatus("last update failed");
                graphTarget.SetError("Port is no longer open");
            }
            catch (Exception e)
            {
                SafelySetStatus("last update failed");
                graphTarget.SetError(e.Message);
            }
        }

        private void moveToNewFloatingWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Create a new sensor form which isn't an MDI child of anything.
            sensorFrm newSensor = new sensorFrm(this.node, this.targetSensorIndex);
            newSensor.Show();
            newSensor.Controls.Remove(newSensor.ctlSensor1);
            newSensor.ctlSensor1 = this.copyOf();
            newSensor.Controls.Add(newSensor.ctlSensor1);

            // then remove the old window.
            this.ParentForm.Dispose();
        }

        private void copyToNewfloatingWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new sensor form which isn't an MDI child of anything.
            sensorFrm newSensor = new sensorFrm(this.node, this.targetSensorIndex);
            newSensor.Show();
            newSensor.Controls.Remove(newSensor.ctlSensor1);
            newSensor.ctlSensor1 = this.copyOf();
            newSensor.Size = this.ParentForm.Size ;
            if (null != this.ParentForm.MdiParent)
            {
                newSensor.Opacity = this.ParentForm.MdiParent.Opacity ;
            }
            else
            {
                newSensor.Opacity = this.ParentForm.Opacity;
            }
            if (alwaysOnTopToolStripMenuItem.Checked)
                newSensor.TopMost = true;
            newSensor.Controls.Add(newSensor.ctlSensor1);
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // We adjust the 'always on top' of this form if we are not MDI-docked, or the MDI
            // parent if we are.
            if (this.ParentForm.MdiParent == null)
            {
                if (true == ((ToolStripMenuItem)sender).Checked)
                {
                    ParentForm.TopMost = true;
                }
                else
                {
                    ParentForm.TopMost = false;
                }
            }
            else
            {
                ((frmNodeSensors)ParentForm.MdiParent).setAlwaysOnTop( ((ToolStripMenuItem)sender).Checked);
            }
        }

        private void ctlSensor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.ParentForm.MdiParent != null)
                alwaysOnTopToolStripMenuItem.Enabled = true;
            else
                alwaysOnTopToolStripMenuItem.Enabled = false;
        }

        private void setTrans_Click(object sender, EventArgs e)
        {
            this.mnuTrans0.Checked = false;
            this.mnuTrans10.Checked = false;
            this.mnuTrans20.Checked = false;
            this.mnuTrans30.Checked = false;
            this.mnuTrans40.Checked = false;
            this.mnuTrans50.Checked = false;
            this.mnuTrans60.Checked = false;
            this.mnuTrans70.Checked = false;
            this.mnuTrans80.Checked = false;
            this.mnuTrans90.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true;

            // Modify the parent MDI window if we're docked in an MDI window, since MDI child
            // windows can't be transparent (and it doesn't make much sense anyway)
            Form target;
            if (null == this.ParentForm.MdiParent)
                target = this.ParentForm;
            else
                target = this.ParentForm.MdiParent;

            if (mnuTrans0.Checked)
                target.Opacity = 1;
            else if (mnuTrans10.Checked)
                target.Opacity = 0.9;
            else if (mnuTrans20.Checked)
                target.Opacity = 0.8;
            else if (mnuTrans30.Checked)
                target.Opacity = 0.7;
            else if (mnuTrans40.Checked)
                target.Opacity = 0.6;
            else if (mnuTrans50.Checked)
                target.Opacity = 0.5;
            else if (mnuTrans60.Checked)
                target.Opacity = 0.4;
            else if (mnuTrans70.Checked)
                target.Opacity = 0.3;
            else if (mnuTrans80.Checked)
                target.Opacity = 0.2;
            else if (mnuTrans90.Checked)
                target.Opacity = 0.1;
        }

        private void minimiseToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((sensorFrm)this.ParentForm).minimiseToTray();
        }
    }

    public class graph : UserControl
    {
        public virtual void UpdateValue(Boolean  toThis) { }
        public virtual void UpdateValue(String toThis) { }
        public virtual void UpdateValue(Object toThis) { }

        public virtual Object SendValue() 
        {
            // User should never see this
            throw new NotImplementedException("Please implement this for your sensor type");
        }

        public delegate void sendValueToNode(object sendThis);
        public sendValueToNode sendValueToNodeDelegate  ;
        
        public virtual void SetError(String errorString) { }
    }

}
