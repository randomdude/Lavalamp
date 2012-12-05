using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ruleEngine;
using ruleEngine.basicLogic;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.windows;

namespace TestProjects
{
    [TestClass]
    public class ruleItemTests
    {
        [TestMethod]
        public void testStartRunRuleItemAnd()
        {
            rule targetRule = new rule();

            ruleItemBase andGate = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_and)));
            targetRule.start();
            andGate.pinInfo["input1"].value.data = false;
            andGate.pinInfo["input2"].value.data = false;
            andGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);
            
            andGate.pinInfo["input1"].value.data = true;
            andGate.pinInfo["input2"].value.data = false;
            andGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);

            andGate.pinInfo["input1"].value.data = false;
            andGate.pinInfo["input2"].value.data = true;
            andGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);

            andGate.pinInfo["input1"].value.data = true;
            andGate.pinInfo["input2"].value.data = true;
            andGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(true, andGate.pinInfo["output1"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemDebug()
        {
            rule targetRule = new rule();
            ruleItemBase debugItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_debug)));

            targetRule.start();
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.noValue, debugItem.pinInfo["output"].value.data);

            debugItem.pinInfo["input"].value.data = false;
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.no, debugItem.pinInfo["output"].value.data);

            debugItem.pinInfo["input"].value.data = true;
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.yes, debugItem.pinInfo["output"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemNot()
        {
            rule targetRule = new rule();
            ruleItemBase debugItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_not)));

            targetRule.start();
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.noValue, debugItem.pinInfo["output1"].value.data);

            debugItem.pinInfo["input1"].value.data = false;
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.yes, debugItem.pinInfo["output1"].value.data);

            debugItem.pinInfo["input1"].value.data = true;
            debugItem.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.no, debugItem.pinInfo["output1"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemOr()
        {
            rule targetRule = new rule();
            ruleItemBase orGate = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_or)));
            targetRule.start();

            orGate.pinInfo["input1"].value.data = false;
            orGate.pinInfo["input2"].value.data = false;
            orGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(false, orGate.pinInfo["output1"].value.asBoolean());

            orGate.pinInfo["input1"].value.data = true;
            orGate.pinInfo["input2"].value.data = false;
            orGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.asBoolean());

            orGate.pinInfo["input1"].value.data = false;
            orGate.pinInfo["input2"].value.data = true;
            orGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.asBoolean());

            orGate.pinInfo["input1"].value.data = true;
            orGate.pinInfo["input2"].value.data = true;
            orGate.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.asBoolean());
        }

        [TestMethod]
        public void testStartRunRuleItemSplitter()
        {
            rule targetRule = new rule();
            ruleItemBase spliter = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_splitter)));
            targetRule.start();

            spliter.pinInfo["input1"].value.data = false;
            spliter.evaluate();
            targetRule.advanceDelta();
            
            Assert.AreEqual(false, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(false, spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].value.data = true;
            spliter.evaluate();
            targetRule.advanceDelta();
            
            Assert.AreEqual(true, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(true, spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].valueType = typeof(pinDataString);
            spliter.pinInfo["input1"].recreateValue();
            spliter.pinInfo["input1"].value.data = "lol";
            spliter.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual("lol", spliter.pinInfo["output1"].value.data);
            Assert.AreEqual("lol", spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].valueType = typeof(pinDataTristate);
            spliter.pinInfo["input1"].recreateValue();
            spliter.pinInfo["input1"].value.data = tristate.yes;
            spliter.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.yes, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(tristate.yes, spliter.pinInfo["output2"].value.data);
        }

        [TestMethod]
        public void testRssReader()
        {
            rule targetRule = new rule();
            ruleItemRss rss = (ruleItemRss) targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItemRss)));
            rss._options.url = "file://" + Path.GetFullPath(Properties.Settings.Default.testDataPath) + @"\test.rss";
            rss.pinInfo["trigger"].value.data = true;
            targetRule.start();
            rss.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual("hi <3", rss.pinInfo["feed Title"].value.data);
            Assert.AreEqual("Hello World", rss.pinInfo["feed Content"].value.data);
            rss.pinInfo["trigger"].value.data = true;
            rss.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual("test entry", rss.pinInfo["feed Title"].value.data);
            Assert.AreEqual("Test 'details'", rss.pinInfo["feed Content"].value.data);
            targetRule.stop();
            rss._options.url = "file://" + Path.GetFullPath(Properties.Settings.Default.testDataPath) + @"\test.atom";
            targetRule.start();
            rss.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual("Atom-Powered Robots Run Amok", rss.pinInfo["feed Title"].value.data);
            Assert.AreEqual("Some text.", rss.pinInfo["feed Content"].value.data);
            rss.evaluate();
            targetRule.advanceDelta();
            Assert.AreEqual("lol", rss.pinInfo["feed Title"].value.data);
            Assert.AreEqual("test", rss.pinInfo["feed Content"].value.data);
            targetRule.stop();

        }
        [TestMethod]
        public void testHTMLStripper()
        {
            string html = @"<p><style>font{blah: x }</style>llololol<b>bolded</b><script>alert('x');</script></p>";
            rule targetRule = new rule();
            ruleItem_HTMLStripper htmlStripper = (ruleItem_HTMLStripper)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_HTMLStripper)));
            htmlStripper.pinInfo["input"].value.data = html;
            htmlStripper.evaluate();
            targetRule.advanceDelta();
            string value = (string) htmlStripper.pinInfo["output"].value.data;
            Assert.AreEqual(value,"llolololbolded");
        }

        [TestMethod]
        public void testProcessRuleItems()
        {
            rule targetRule = new rule();
            ruleItem_runexe runexe = (ruleItem_runexe)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_runexe)));
            ruleItem_isProcessRunning isRunning = (ruleItem_isProcessRunning)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_isProcessRunning)));
            ruleItem_killProcess killProcess = (ruleItem_killProcess)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_killProcess)));
            runexe.fileToRun = @"C:\Windows\system32\cmd.exe";
            isRunning.processName = "cmd";
            killProcess.name = "cmd";
            runexe.pinInfo["input1"].value.data = true;
            targetRule.start();
            runexe.evaluate();
            targetRule.advanceDelta();
            isRunning.pinInfo["trigger"].value.data = true;
            isRunning.evaluate();
            targetRule.advanceDelta();
            bool running = isRunning.pinInfo["output1"].value.asBoolean(); 
            for(int i = 0; i < 20 && !running;i++ )
            {
                isRunning.evaluate();
                targetRule.advanceDelta();
                running = isRunning.pinInfo["output1"].value.asBoolean();
                if (!running)
                    Thread.Sleep(20);
            }
            Assert.AreEqual(running, true);
            killProcess.pinInfo["input1"].value.data = true;
            killProcess.evaluate();
            targetRule.advanceDelta();
            isRunning.evaluate();
            targetRule.advanceDelta();
            running = isRunning.pinInfo["output1"].value.asBoolean();
            Assert.AreEqual(running, false);

        }
        /*
        [TestMethod]
        public void testFlipFlopRuleItem()
        {
            rule targetRule = new rule();
            ruleItem_flipFlop switchRule = (ruleItem_flipFlop)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_flipFlop)));

            targetRule.start();

            switchRule.pinInfo["set"].value.data = false;
            switchRule.pinInfo["clear"].value.data = false;
            targetRule.advanceDelta();
            // output is trisated
            switchRule.pinInfo["set"].value.data = true;
            switchRule.pinInfo["clear"].value.data = false;
            targetRule.advanceDelta();
            // output is true
            targetRule.advanceDelta();
            switchRule.pinInfo["set"].value.data = false;
            switchRule.pinInfo["clear"].value.data = false;
            // output is still true
            targetRule.advanceDelta();
            switchRule.pinInfo["set"].value.data = false;
            switchRule.pinInfo["clear"].value.data = true;
            targetRule.advanceDelta();
            // output is false

        }*/

        [TestMethod]
        public void testSwitchRuleItem()
        {
            rule targetRule = new rule();
            ruleItem_switch switchRule = (ruleItem_switch)targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_switch)));

            targetRule.start();
            //test bools 
            switchRule.pinInfo["inputTrue"].value.data = true;
            switchRule.pinInfo["inputFalse"].value.data = false;
            switchRule.pinInfo["switch"].value.data = false;
            //must start as tristate and noValue
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(tristate));
            Assert.AreEqual(switchRule.pinInfo["output"].value.data,tristate.noValue);
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(bool));
            Assert.IsFalse((bool) switchRule.pinInfo["output"].value.data);
            switchRule.pinInfo["switch"].value.data = true;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsTrue((bool) switchRule.pinInfo["output"].value.data);
            //test string and bool
            switchRule.pinInfo["inputFalse"].valueType = typeof(pinDataString);
            switchRule.pinInfo["inputFalse"].recreateValue();
            switchRule.pinInfo["inputFalse"].value.data = "True";
            switchRule.pinInfo["switch"].value.data = false;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(string));
            Assert.AreEqual(switchRule.pinInfo["output"].value.data, "True");
            switchRule.pinInfo["switch"].value.data = true;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(bool));
            Assert.IsTrue((bool) switchRule.pinInfo["output"].value.data);
            //test int and bool 
            switchRule.pinInfo["inputFalse"].valueType = typeof(pinDataNumber);
            switchRule.pinInfo["inputFalse"].recreateValue();
            switchRule.pinInfo["inputFalse"].value.data = new GenericNumber<short>(255);
            switchRule.pinInfo["switch"].value.data = false;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(INumber));
            Assert.AreEqual(switchRule.pinInfo["output"].value.data, new GenericNumber<short>(255));
            switchRule.pinInfo["switch"].value.data = true;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(bool));
            Assert.AreEqual(switchRule.pinInfo["output"].value.data, true);
            //bool and tristate
            switchRule.pinInfo["inputFalse"].valueType = typeof(pinDataTristate);
            switchRule.pinInfo["inputFalse"].recreateValue();
            switchRule.pinInfo["inputFalse"].value.data = tristate.yes;
            switchRule.pinInfo["switch"].value.data = false;
            switchRule.evaluate();
            targetRule.advanceDelta();
            Assert.IsInstanceOfType(switchRule.pinInfo["output"].value.data, typeof(tristate));
            Assert.AreEqual(switchRule.pinInfo["output"].value.data, tristate.yes);
            targetRule.stop();

        }


    }
}