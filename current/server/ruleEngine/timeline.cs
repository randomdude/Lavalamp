using System;
using System.Collections.Generic;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    public class timeline : timelineEvents
    {
        /// <summary>
        /// The current 'time' of the rule
        /// </summary>
        private int delta { get; set; }

        /// <summary>
        /// Pending events
        /// </summary>
        private readonly Dictionary<int, timelineBucket> _queue = new Dictionary<int, timelineBucket>();

        public void addEventAtNextDelta(timelineEventArgs eventArgs)
        {
            addEvent(new timelineEvent(delta + 1, eventArgs.newValue ));
        }

        public void addEventAtDelta(timelineEventArgs eventArgs, int timebeforeevent)
        {
            addEvent(new timelineEvent(delta + timebeforeevent, eventArgs.newValue));
        }

        private void addEvent(timelineEvent timelineEvent)
        {
            if (!_queue.ContainsKey(timelineEvent.delta))
                _queue.Add(timelineEvent.delta, new timelineBucket());

            _queue[timelineEvent.delta].Add(timelineEvent);
        }

        public void advanceDelta()
        {
            delta = delta + 1;

            // Process any events at this delta
            if (_queue.ContainsKey(delta))
            {
                foreach (timelineEvent eventNow in _queue[delta])
                {
                    onEventOccuring(eventNow);
                }
            }

            onTimelineAdvance();
        }

        public void reset()
        {
            _queue.Clear();
            delta = 0;
        }
    }
}