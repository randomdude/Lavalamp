﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine;
using ruleEngine.ruleItems.Starts;

namespace ruleEngine.ruleItems.itemControls
{
    public partial class ctlTimeInterval : UserControl
    {
        private Color normalBackground;
        public ruleItem_timeInterval.setIntervalDelegate setInterval;   // we call this when the timer interval changes.
        public ruleItem_timeInterval.getIntervalDelegate getInterval;

        public ctlTimeInterval()
        {
            InitializeComponent();
            normalBackground = this.BackColor;
        }

        public void setTimeCaption(int low)
        {
            lblTime.Text = "Fires every " + low + "ms";
        }

        public ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while (mnuParent.Items.Count > 0)
                toRet.Items.Add(mnuParent.Items[0]);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            while(contextMenuStrip1.Items.Count > 0)
                toRet.Items.Add(contextMenuStrip1.Items[0]);

            return toRet;
        }

        public void setTo(bool value)
        {
            // This will set the background of the timer, according to if the timer is currently firing.
            if (value)
                this.BackColor = Color.Red;
            else
                this.BackColor = normalBackground ;
        }

        private void setIntervalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            bool success = false;
            while(!success)
                success = promptForNewInterval();
        }

        private bool promptForNewInterval()
        {
            frmQuestion askyform = new frmQuestion("New time interval (ms):", getInterval().ToString() );

            if (askyform.ShowDialog(this) == DialogResult.Cancel)
                return true;

            String result = askyform.result;
            int newInterval;

            if (setInterval != null)
            {
                try
                {
                    newInterval = int.Parse(result);
                    if (newInterval  < 500)
                    {
                        if ( MessageBox.Show( "That value is rather small - are you sure you want to use such a low value? Making your system run too fast can cause problems.", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.No)
                            return false;
                    }
                    setInterval(newInterval);
                    setTimeCaption(newInterval);
                    return true;
                }
                catch (FormatException )
                {
                    MessageBox.Show("Please enter a valid number");
                    return false;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Please enter a sensibly-ranged number, between " + int.MinValue + " and " + int.MaxValue );
                    return false;
                }
            }

            MessageBox.Show("Unable to set timer interval");
            return true;    // eep, that's not good - bail
        }
    }
}
