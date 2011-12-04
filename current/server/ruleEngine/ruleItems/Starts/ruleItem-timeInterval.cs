using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine;
using ruleEngine.pinDataTypes;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_timeInterval : ruleItemBase
    {
        private Timer _intervalCountdown;
        itemControls.ctlTimeInterval _timer = new itemControls.ctlTimeInterval();

        [XmlElement("timer")] public int timerLow = 5000;

        // This delegate type should be used to set the interval which the timer fires at.
        public delegate void setIntervalDelegate(int low);
        // And this to get it.
        public delegate int getIntervalDelegate();

        public override string ruleName() { return "Interval timer"; }

        public ruleItem_timeInterval()
        {
            _timer.Visible = true;
            _timer.getInterval = new getIntervalDelegate(handleGetTimerInterval);
            _timer.setInterval = new setIntervalDelegate(handleSetTimerInterval);

            controls.Add(_timer);
        }

        public override void onAfterLoad()
        {
            // Set this here since we need to have finished loading the object - we need the timerLow
            _timer.setTimeCaption(timerLow);
        }

        private int handleGetTimerInterval()
        {
            return timerLow;
        }

        private void handleSetTimerInterval(int newLow)
        {
            timerLow = newLow;
        }

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip menus = base.addMenus(mnuParent);
            return _timer.addMenus(menus);
        }

        public override void onResize(Control parent)
        {
            _timer.Left = (parent.Width / 2) - (_timer.Width / 2);
            _timer.Top  = (parent.Height/ 2) - (_timer.Height / 2);
        }

        public override void start()
        {
            if (_intervalCountdown != null)
                _intervalCountdown.Dispose();
            _intervalCountdown = new Timer(timerCallbackSet, null, timerLow, timerLow);
            _timer.setTo(false);
        }
        
        private void timerCallbackSet(object state)
        {
            timelineEventArgs args = new timelineEventArgs();
            args.newValue = new pinDataBool(true, this, pinInfo["IntervalIsNow"]);
            onRequestNewTimelineEvent(args);

            timelineEventArgs cancelArgs = new timelineEventArgs();
            cancelArgs.newValue = new pinDataBool(false, this, pinInfo["IntervalIsNow"]);
            onRequestNewTimelineEventInFuture(cancelArgs, 2);

            _timer.setTo(false);
        }

        public override void stop()
        {
            _intervalCountdown.Dispose();
            _timer.setTo(false);
        }

        public override System.Drawing.Image background()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            _timer.setTimeCaption(timerLow);

            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("IntervalIsNow", new pin { name = "IntervalIsNow", description = "time has elapsed", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
        
        }
    }
}