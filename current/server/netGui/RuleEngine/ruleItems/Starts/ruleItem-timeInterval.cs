using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timer=System.Threading.Timer;

namespace netGui.RuleEngine.ruleItems.Starts
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_timeInterval : ruleItemBase
    {
        private Timer intervalCountdown;
        netGui.RuleEngine.ruleItems.itemControls.ctlTimeInterval timer =
            new netGui.RuleEngine.ruleItems.itemControls.ctlTimeInterval();

        private int timerHigh = 100 ;
        private int timerLow = 5000 ;

        // This delegate type should be used to set the interval which the timer fires at.
        public delegate void setIntervalDelegate(int low, int high);
        // And this to get it.
        public delegate int getIntervalDelegate();

        public override string ruleName() { return "Interval timer"; }

        public ruleItem_timeInterval()
        {
            this.pinStates.evaluate = new evaluateDelegate(evaluate);
            pinStates.setErrorHandler(new errorDelegate(base.errorHandler));
            timer.Visible = true;
            timer.setInterval = new setIntervalDelegate(handleSetTimerInterval);
            timer.getInterval = new getIntervalDelegate(handleGetTimerInterval);

            this.controls.Add(timer);
            timer.setTimeCaption(timerLow);
        }

        private int handleGetTimerInterval()
        {
            return timerLow;
        }

        private void handleSetTimerInterval(int newLow, int newHigh)
        {
            timerLow = newLow;
            timerHigh = newHigh;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip menus = base.addMenus(mnuParent);
            return timer.addMenus(menus);
        }

        public override void onResize(Control parent)
        {
            timer.Left = (parent.Width / 2) - (timer.Width / 2);
            timer.Top  = (parent.Height/ 2) - (timer.Height / 2);
        }

        public override void start()
        {
            if (intervalCountdown != null)
                intervalCountdown.Dispose();
            intervalCountdown = new Timer(timerCallbackSet, null, timerLow, timerLow);
            timer.setTo(false);
        }
        
        private void timerCallbackSet(object state)
        {
            this.pinStates["IntervalIsNow"] = true;
            // pass the change over to the ctlTimeInterval so it can flash for the user
            System.Threading.Thread.Sleep(timerHigh);
            this.pinStates["IntervalIsNow"] = false;
            timer.setTo(false);
        }

        public override void stop()
        {
            intervalCountdown.Dispose();
            timer.setTo(false);
        }

        public override System.Drawing.Image background()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("IntervalIsNow", new pin { name = "IntervalIsNow", description = "time has elapsed", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
        
        }
    }
}