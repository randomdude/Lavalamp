using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using netGui.RuleEngine.ruleItems.Starts;

namespace netGui.RuleEngine.ruleItems
{
    [RuleEngine.ToolboxRule]
    [RuleEngine.ToolboxRuleCategory("Internet")]
    public class ruleItem_Email : ruleItemBase
    {
        public override string ruleName() { return "email checker"; }
        private bool lastState = false;
        ctlCheckEmail widget = new ctlCheckEmail();

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();

            pinList.Add("newEmail", new pin { name = "newEmail", description = "unread mail is present", direction = pinDirection.output });
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
            bool thisState = (bool) pinInfo["checkNow"].value.getData();

            imapChecker mychecker = new imapChecker(widget.options);

            if ((lastState != thisState ) && (thisState == true))
            {
                if (mychecker.newMail != (bool)pinInfo["newEmail"].value.getData())
                    pinInfo["newEmail"].value.setData(mychecker.newMail);
            }
            lastState = thisState;
        }
    }
}
