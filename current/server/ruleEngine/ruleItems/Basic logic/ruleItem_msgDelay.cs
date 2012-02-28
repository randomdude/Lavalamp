using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems
{
    [ToolboxRule]
    [ToolboxRuleCategory("Misc")]
    public class ruleItem_msgDelay : ruleItemBase
    {
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
        private int delayIntervalSecs = 10;

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

        public override ContextMenuStrip addMenus(ContextMenuStrip mnuParent)
        {
            ContextMenuStrip toRet = base.addMenus(mnuParent);

            if (toRet.Items.Count > 0)
                toRet.Items.Add("-");

            ToolStripMenuItem newItem = new ToolStripMenuItem("Set &Interval");
            newItem.Click += setIntervalToolStripMenuItem_Click;
            toRet.Items.Add(newItem);

            return toRet;
        }

        private void setIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool success = false;
            while (!success)
                success = promptForNewInterval();
        }

        public bool promptForNewInterval()
        {
            frmQuestion askyform = new frmQuestion("New time interval (seconds):", delayIntervalSecs.ToString());

            if (askyform.ShowDialog() == DialogResult.Cancel)
                return true;

            String result = askyform.result;

            bool parsedOK = int.TryParse(result, out delayIntervalSecs);

            if (!parsedOK)
            {
                MessageBox.Show("Please enter a valid number");
                return false;
            }

            return true;
        }
    }
}