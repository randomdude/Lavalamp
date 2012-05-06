using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_timeInterval : ruleItemBase
    {
        private Timer _intervalCountdown;
        private Object _intervalCountdownLock = new object();
        public TimeOptions options = new TimeOptions();

        [XmlElement("timer")] public int intervalSec = 5;

        // This delegate type should be used to set the interval which the timer fires at.
        public delegate void setIntervalDelegate(int low);
        // And this to get it.
        public delegate int getIntervalDelegate();

        public override string ruleName() { return "Interval timer"; }

        public override string caption()
        {
            return  "Fires every " + intervalSec + " sec";
        }

        public ruleItem_timeInterval()
        {
            options.minutes = 5;
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }



        private int handleGetTimerInterval()
        {
            return intervalSec;
        }

        private void handleSetTimerInterval(int newLow)
        {
            intervalSec = newLow;
        }

        public override void start()
        {
            lock (_intervalCountdownLock)
            {
                _intervalCountdown = new Timer(timerCallbackSet, null, intervalSec * 1000, intervalSec * 1000);
            }
            //_timer.setTo(false);
        }
        
        private void timerCallbackSet(object state)
        {
            timelineEventArgs args = new timelineEventArgs();
            args.newValue = new pinDataBool(true, this, pinInfo["IntervalIsNow"]);
            onRequestNewTimelineEvent(args);

            timelineEventArgs cancelArgs = new timelineEventArgs();
            cancelArgs.newValue = new pinDataBool(false, this, pinInfo["IntervalIsNow"]);
            onRequestNewTimelineEventInFuture(cancelArgs, 2);

        //    _timer.setTo(false);
        }

        public override void stop()
        {
            lock (_intervalCountdownLock)
            {
                if (_intervalCountdown != null)
                    _intervalCountdown.Dispose();
            }
          //  _timer.setTo(false);
        }

        public override System.Drawing.Image background()
        {
            return null;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            //_timer.setTimeCaption(intervalSec);

            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("IntervalIsNow", new pin { name = "IntervalIsNow", description = "Time has elapsed", direction = pinDirection.output });

            return pinList;
        }

        public override void evaluate()
        {
        
        }
    }
}