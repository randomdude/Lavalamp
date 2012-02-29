using System;
using System.Collections.Generic;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems.Starts
{
    [ToolboxRule]
    [ToolboxRuleCategory("Start items")]
    public class ruleItem_startRun : ruleItemBase
    {
        public override string ruleName() { return "At start of run"; }

        public override string caption()
        {
            return "At start of run";
        }

        public override void start()
        {
            timelineEventArgs args = new timelineEventArgs();
            args.newValue = new pinDataBool(true, this, pinInfo["StartOfSim"]);
            onRequestNewTimelineEvent( args );

            timelineEventArgs cancelArgs = new timelineEventArgs();
            cancelArgs.newValue = new pinDataBool(false, this, pinInfo["StartOfSim"]);
            onRequestNewTimelineEventInFuture(cancelArgs, 2);
        }

        public override void stop() { }

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