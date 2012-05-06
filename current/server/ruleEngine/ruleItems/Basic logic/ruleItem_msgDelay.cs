using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Misc")]
    public class ruleItem_msgDelay : ruleItemBase
    {
        [XmlElement("options")]
        public IntervalOption options = new IntervalOption();

        public override string ruleName()
        {
            return "Message delay";
        }

        public override Size preferredSize()
        {
            return new Size(75, 100);
        }

        public override Image background()
        {
            return Properties.Resources.ruleItem_msgDelay;
        }

        public override string caption()
        {
            return "Message delay";
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input",
                        new pin
                            {
                                name = "input",
                                description = "input",
                                direction = pinDirection.input,
                                valueType = typeof (pinDataString)
                            });
            pinList.Add("output",
                        new pin
                            {
                                name = "output",
                                description = "delayed output",
                                direction = pinDirection.output,
                                valueType = typeof (pinDataString)
                            });

            return pinList;
        }

        private int timeUntilEmpty = 0;
        private string lastInput = "";
        [XmlElement("delay")]
        public int delayIntervalSecs = 10;

        public override void evaluate()
        {
            if (timeUntilEmpty > 0)
                timeUntilEmpty--;

            IPinData input = pinInfo["input"].value;
            if (lastInput == (string) input.data)
                return;
            lastInput = (string) input.data;

            if (timeUntilEmpty == 0)
            {
                // Nothing is queued! Send the item right through.
                onRequestNewTimelineEvent(
                    new timelineEventArgs(new pinDataString((string) input.data, this, pinInfo["output"])));
                timeUntilEmpty += delayIntervalSecs * 10;
                return;
            }

            // There's something blocking our FIFO. We should set a new event in the future for the time
            // when the fifo will be empty,.
            onRequestNewTimelineEventInFuture(
                new timelineEventArgs(new pinDataString((string) input.data, this, pinInfo["output"])), timeUntilEmpty);
            timeUntilEmpty += delayIntervalSecs * 10;
        }


    }

    public class IntervalOption : BaseOptions
    {
        public int hours;

        public int mins;

        public override string displayName
        {
            get
            {
                return "Select an Interval...";
            }
        }

        public override string typedName
        {
            get
            {
                return "Interval";
            }
        }
    }
}