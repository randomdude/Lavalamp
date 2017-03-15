using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Internet")]
    public class ruleItem_HTMLStripper : ruleItemBase
    {
        public override string ruleName() { return "HTML stripper"; }

        public override System.Drawing.Image background()
        {
            return null;// Properties.Resources.ruleItem_HTMLStripper;
        }

        public override string caption()
        {
            return "HTML stripper";
        }

        public override IFormOptions setupOptions()
        {
            return null;
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            Dictionary<string, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("input", new pin { name = "input", description = "input pin", direction = pinDirection.input, valueType = typeof(pinDataString) });
            pinList.Add("output", new pin { name = "output", description = "stripped text", direction = pinDirection.output, valueType = typeof(pinDataString) });

            return pinList;
        }

        private string lastData = "";

        public override string typedName
        {
            get
            {
                return "HTMLStripper";
            }
        }

        public override void evaluate()
        {
            IPinData input = pinInfo["input"].value;

            if(lastData == (string)input.data)
                return;
            lastData = (string) input.data;

            // Strip any HTML tags out of the input using the 'HTML agility pack'. Doing this in Regex is
            // (apparently) impossible, and since data can come from anywhere, I figure it's a good idea
            // to use a tried-and-tested 3rd party lib.
            HtmlDocument hd = new HtmlDocument(); 
            hd.LoadHtml((string) input.data);
            foreach (var script in hd.DocumentNode.Descendants("script").ToArray())
                script.Remove();
            foreach (var style in hd.DocumentNode.Descendants("style").ToArray())
                style.Remove();
            string strippedHTML = hd.DocumentNode.InnerText;

            //propagate changed value
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(strippedHTML, this, pinInfo["output"])));
        }
    }
}