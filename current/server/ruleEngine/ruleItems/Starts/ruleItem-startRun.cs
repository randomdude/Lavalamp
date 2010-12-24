using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ruleEngine;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_startRun : ruleItemBase
    {
        private Timer cancelTimer;
        private Label lblCaption;

        public override string ruleName() { return "At start of run"; }

        public ruleItem_startRun()
        {
            lblCaption = new Label();
            lblCaption.AutoSize = false;
            lblCaption.Width = preferredSize().Width;
            lblCaption.Height = 20;
            lblCaption.Left = 0;
            lblCaption.Top = preferredSize().Height - lblCaption.Height;
            lblCaption.Text = "At start of run";
            lblCaption.Visible = true;
            controls.Add(lblCaption);

        }

        public override void start()
        {
            // We don't fire at the start of simulation, per se, because this ruleItem may be start'ed before the others (since you can't start them all at exactly
            // the same time). Instead, we use a one-shot 200ms timer. 
            // FIXME: Think of a better way to do this.
            cancelTimer = new Timer(setOutput, null, 200, System.Threading.Timeout.Infinite );
        }

        private void setOutput(object state)
        {
            pinInfo["StartOfSim"].value.setData(true);
            cancelTimer = new Timer(cancelOutput, null, 100, System.Threading.Timeout.Infinite);
        }

        private void cancelOutput(object state)
        {
            pinInfo["StartOfSim"].value.setData(false);
        }

        public override void stop()
        {
            if (cancelTimer != null)
                cancelTimer.Dispose();
        }

        public override System.Drawing.Image background()
        {
            return Properties.Resources.New.ToBitmap();
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("StartOfSim", new pin { name = "StartOfSim", description = "simulation starts", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate(){}
    }
}