namespace ruleEngine
{
    public class timelineEvents
    {
        public event timelineEventOccuringEventHandlerDelegate eventOccuring;
        public delegate void timelineEventOccuringEventHandlerDelegate(timeline sender, timelineEvent e);

        protected void onEventOccuring(timelineEvent occuringNow)
        {
            if (eventOccuring != null)
            {
                eventOccuring.Invoke((timeline)this, occuringNow);
            }
        }

        public event timelineAdvanceEventHandlerDelegate timelineAdvance;
        public delegate void timelineAdvanceEventHandlerDelegate(timeline sender);

        protected void onTimelineAdvance()
        {
            if (timelineAdvance != null)
            {
                timelineAdvance.Invoke((timeline)this);
            }
        }
    }
}