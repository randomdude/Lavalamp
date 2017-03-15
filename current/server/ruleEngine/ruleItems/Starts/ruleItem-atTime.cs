using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_atTime : ruleItemBase
    {
        [XmlIgnore]
        public Timer checkTimer;

        [XmlElement("options")]
        public TimeOptions options = new TimeOptions();
        private bool _running = false;
        private long _lastHours;
        private long _lastMinutes;

        private string _caption;

        public override string caption()
        {
            return _caption;
        } 

        private int hours
        {
            get { return options.hours; }
            set
            {
                options.hours = value;
                _caption = "at " + options.hours + ":" + options.minutes;
            }
        }

        private int minutes
        {
            get { return options.minutes; }
            set
            {
                options.minutes = value;
                _caption = "at " + options.hours.ToString("00") + ":" + options.minutes.ToString("00");
            }
        }

        public override string typedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ruleName() { return "At a certain time"; }

        public ruleItem_atTime()
        {
            hours = 20;
            minutes = 01;
        }

        public override void start()
        {
            this._running = false;
            checkTimer = new Timer(checkTime, null, 500, 1000 );
        }

        private void checkTime(object state)
        {
            DateTime now = DateTime.Now;
            DateTime targettime = new DateTime(now.Year, now.Month, now.Day, (int)hours, (int)minutes, 00);
            DateTime lasttime = new DateTime(now.Year, now.Month, now.Day, (int)this._lastHours, (int)this._lastMinutes, 00);

            if (!this._running)
            {
                // This is the first time we have been called. Set the last time and return
                this._lastHours = now.Hour;
                this._lastMinutes = now.Minute;
                this._running = true;
                return;
            }
            bool doit = false;
            switch (options.when)
            {
                    case TimeToRun.Daily:
                        doit = (targettime > lasttime ) &&       // was time of last check before the target time?
                                (targettime < now      );
                        break;
                    case TimeToRun.Weekly:
                        doit = (targettime > lasttime) &&       // was time of last check before the target time?
                                (targettime < now) && 
                                ((int)now.DayOfWeek) == options.Day;
                        break;
                    case TimeToRun.Monthly:
                        int dayToRun = DateTime.DaysInMonth(now.Year, now.Month);
                        // if greater than the days in the month use the last day
                        if (options.Day < dayToRun) dayToRun = options.Day;
                        doit = (targettime > lasttime) && // was time of last check before the target time?
                               (targettime < now) && 
                               now.Day == dayToRun;
                        break;
                    case TimeToRun.Yearly:
                        doit = (targettime > lasttime) && // was time of last check before the target time?
                               (targettime < now) &&
                               now.Month == options.Month &&
                               now.Year == options.Year;
                        break;
            }
  
             

            // Otherwise, see if the specified date has passed since the last call.

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

            this._lastHours = now.Hour;
            this._lastMinutes = now.Minute;
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }


        public override void stop()
        {
            checkTimer.Dispose();
        }

        public override System.Drawing.Image background()
        {
            return null;// Resources.clock21;
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

    public class TimeOptions :BaseOptions
    {
        [XmlElement("hours")]
        public int hours;
        [XmlElement("minutes")]
        public int minutes;

        public int after;

        public TimeToRun when;

        public int Day = 1;

        public int Year;

        public int Month;

        public override string displayName
        {
            get
            {
                return "Set Time...";
            }
        }

        public override string typedName
        {
            get
            {
                return "Time";
            }
        }

    }

    public enum TimeToRun
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

}