namespace ruleEngine
{
    public class timelineEvent
    {
        public readonly int delta;
        public readonly IPinData pinValue;

        public timelineEvent(int newDelta, IPinData newPinValue)
        {
            delta = newDelta;
            pinValue = newPinValue;
        }
    }
}