using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using ruleEngine.Properties;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_atTime : ruleItemBase
    {
        public Timer checkTimer;
        private long _hours;
        private long _minutes;
        private bool running = false;
        private long lastHours;
        private long lastMinutes;

        private Label lblCaption;

        private long hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                lblCaption.Text = "at " + _hours + ":" + _minutes;
            }
        }

        private long minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                lblCaption.Text = "at " + _hours.ToString("00") + ":" + _minutes.ToString("00");
            }
        }

        public override string ruleName() { return "At a certain time"; }

        public ruleItem_atTime()
        {
            lblCaption = new Label();
            lblCaption.AutoSize = false;
            lblCaption.Width = preferredSize().Width;
            lblCaption.Height = 20;
            lblCaption.Left = 1;
            lblCaption.Top = preferredSize().Height - lblCaption.Height;
            lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblCaption.Visible = true;
            hours = 20;
            minutes = 01;
            controls.Add(lblCaption);
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = new ContextMenuStrip();

            while (strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            ToolStripMenuItem newItem = new ToolStripMenuItem("Set &time to fire..");
            newItem.Click += setTimeDialogClosed;
            toRet.Items.Add(newItem);

            return toRet;
        }

        public override void start()
        {
            running = false;
            checkTimer = new Timer(checkTime, null, 500, 1000 );
        }

        private void checkTime(object state)
        {
            DateTime now = DateTime.Now;
            DateTime targettime = new DateTime(now.Year, now.Month, now.Day, (int)hours, (int)minutes, 00);
            DateTime lasttime = new DateTime(now.Year, now.Month, now.Day, (int)lastHours, (int)lastMinutes, 00);

            if (!running)
            {
                // This is the first time we have been called. Set the last time and return
                lastHours = now.Hour;
                lastMinutes = now.Minute;
                running = true;
                return;
            }
            bool doit = false;

            // Otherwise, see if the specified date has passed since the last call.
            if ( (targettime > lasttime ) &&       // was time of last check before the target time?
                 (targettime < now      )   )      // is the target time in the past?
                doit = true;

            if (targettime.CompareTo(now) == 0)     // or it is exactly time
                doit = true;

            if (doit == true)
            {
                timelineEventArgs startArgs = new timelineEventArgs();
                startArgs.newValue = new pinDataBool(true, this, pinInfo["timeIsNow"]);
                onRequestNewTimelineEvent(startArgs);

                timelineEventArgs cancelArgs = new timelineEventArgs();
                cancelArgs.newValue = new pinDataBool(false, this, pinInfo["timeIsNow"]); 
                onRequestNewTimelineEventInFuture(cancelArgs, 2);
            }

            lastHours = now.Hour;
            lastMinutes = now.Minute;
        }

        public override Form ruleItemOptions()
        {
            frmQuestion askyform = new frmQuestion("New time (HH:MM):", hours + ":" + minutes);   // TODO: format number in a more pretty manner
            askyform.Closed += setTimeDialogClosed;
            return askyform;
        }

        private void setTime()
        {
            frmQuestion frm = (frmQuestion) ruleItemOptions();
            frm.ShowDialog();
        }

        private void setTimeDialogClosed(object sender,EventArgs e)
        {
            frmQuestion askyform = (frmQuestion) sender;

            while (askyform.DialogResult == DialogResult.Cancel)
                return;

            int newhours = 0;
            int newminutes = 0; 
            try
            {
                String result = askyform.result;

                String[] time = result.Split(':');

                newhours = int.Parse(time[0]);
                newminutes = int.Parse(time[1]);

                if (newhours < 0 | newhours > 23)
                    throw new FormatException();
                if (newminutes < 0 | newminutes > 59)
                    throw new FormatException();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Please enter the desired time in the form HH:MM, ie, hours, a semicolon, and then minutes.");
                askyform.ShowDialog();
                return;
            }

            hours = newhours;
            minutes = newminutes;
            return ;
        }

        public override void stop()
        {
            checkTimer.Dispose();
        }

        public override System.Drawing.Image background()
        {
            return Resources.clock21;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("timeIsNow", new pin { name = "timeIsNow", description="it is the defined time", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
        
        }
    }
}