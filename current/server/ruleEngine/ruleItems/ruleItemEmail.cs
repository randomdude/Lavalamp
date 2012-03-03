using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems.Starts;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Internet")]
    public class ruleItem_Email : ruleItemBase
    {
        public override string ruleName() { return "email checker"; }
        private bool lastState = false;
        ctlCheckEmail widget = new ctlCheckEmail();
        [XmlElement("EmailOptions")]
        public emailOptions options { get { return widget.options; } set { widget.options = value; } }
        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("newEmail", new pin { name = "newEmail", description = "unread mail is present", direction = pinDirection.output, valueType = typeof(pinDataTrigger)});
            pinList.Add("newEmailTitle", new pin { name = "newEmailTitle", description = "unread mail title", direction = pinDirection.output, valueType = typeof(pinDataString)});
            pinList.Add("checkNow", new pin { name = "checkNow", description = "trigger to check email now", direction = pinDirection.input });

            return pinList;
        }

        public override System.Windows.Forms.ContextMenuStrip addMenus(System.Windows.Forms.ContextMenuStrip strip1)
        {
            ContextMenuStrip menus = base.addMenus(strip1);
          
            return widget.addMenus( menus );
        }

        public ruleItem_Email()
        {
            widget.Location = new Point(0,0);
            widget.Size = this.preferredSize();
            this.controls.Add(widget);
        }

        public override void evaluate()
        {
            bool thisState = pinInfo["checkNow"].value.asBoolean();
             
            if ((lastState != thisState ) && (thisState == true))
            {
                imapChecker mychecker = new imapChecker(widget.options);
                if (mychecker.newMail != pinInfo["newEmail"].value.asBoolean())
                {
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTrigger(mychecker.newMail, this, pinInfo["newEmail"])));
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(mychecker.mailSubject,this,pinInfo["newEmailTitle"])));
                }
            }
            lastState = thisState;
        }
    }
}
