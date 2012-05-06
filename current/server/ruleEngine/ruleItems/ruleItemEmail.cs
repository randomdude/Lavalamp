using System;
using System.Collections.Generic;
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
        private bool lastState;

        [XmlElement("EmailOptions")]
        public emailOptions options = new emailOptions();

        public override string caption()
        {
            return "Any new Email?";
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("newEmail", new pin { name = "newEmail", description = "unread mail is present", direction = pinDirection.output, valueType = typeof(pinDataTrigger)});
            pinList.Add("newEmailTitle", new pin { name = "newEmailTitle", description = "unread mail title", direction = pinDirection.output, valueType = typeof(pinDataString)});
            pinList.Add("checkNow", new pin { name = "checkNow", description = "trigger to check email now", direction = pinDirection.input });

            return pinList;
        }


        public override void evaluate()
        {
            bool thisState = pinInfo["checkNow"].value.asBoolean();
             
            if ((lastState != thisState ) && (thisState == true))
            {
                imapChecker mychecker = new imapChecker(options);
                if (mychecker.newMail != pinInfo["newEmail"].value.asBoolean())
                {
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataTrigger(mychecker.newMail, this, pinInfo["newEmail"])));
                    onRequestNewTimelineEvent(new timelineEventArgs(new pinDataString(mychecker.mailSubject,this,pinInfo["newEmailTitle"])));
                }
            }
            lastState = thisState;
        }

        public override IFormOptions setupOptions()
        {
            return options;
        }
    }
    [Serializable]
    public class emailOptions :BaseOptions
    {
        [XmlElement]
        public string serverName = "imap.gmail.com";
        [XmlElement]
        public int portNum = 993;
        [XmlElement]
        public bool useSSL = true;
        [XmlElement]
        public string username = "username";
        //todo: crypt password
        public string password = "password";

        public override string displayName
        {
            get
            {
                return "Email Options";
            }
        }

        public override string typedName
        {
            get
            {
               return "CheckEmail";
            }
        }
    }
}
