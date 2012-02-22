using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ruleEngine;
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
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);
            
            andGate.pinInfo["input1"].value.data = true;
            andGate.pinInfo["input2"].value.data = false;
            andGate.evaluate();
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);

            andGate.pinInfo["input1"].value.data = false;
            andGate.pinInfo["input2"].value.data = true;
            andGate.evaluate();
            Assert.AreEqual(false, andGate.pinInfo["output1"].value.data);

            andGate.pinInfo["input1"].value.data = true;
            andGate.pinInfo["input2"].value.data = true;
            andGate.evaluate();
            Assert.AreEqual(true, andGate.pinInfo["output1"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemDebug()
        {
            rule targetRule = new rule();
            ruleItemBase debugItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_debug)));

            targetRule.start();
            debugItem.evaluate();
            Assert.AreEqual(tristate.noValue, debugItem.pinInfo["output"].value.data);

            debugItem.pinInfo["input"].value.data = false;
            debugItem.evaluate();
            Assert.AreEqual(tristate.no, debugItem.pinInfo["output"].value.data);

            debugItem.pinInfo["input"].value.data = true;
            debugItem.evaluate();
            Assert.AreEqual(tristate.yes, debugItem.pinInfo["output"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemNot()
        {
            rule targetRule = new rule();
            ruleItemBase debugItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_not)));

            targetRule.start();
            debugItem.evaluate();
            Assert.AreEqual(tristate.noValue, debugItem.pinInfo["output1"].value.data);

            debugItem.pinInfo["input1"].value.data = false;
            debugItem.evaluate();
            Assert.AreEqual(tristate.yes, debugItem.pinInfo["output1"].value.data);

            debugItem.pinInfo["input1"].value.data = true;
            debugItem.evaluate();
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
            Assert.AreEqual(false, orGate.pinInfo["output1"].value.data);

            orGate.pinInfo["input1"].value.data = true;
            orGate.pinInfo["input2"].value.data = false;
            orGate.evaluate();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.data);

            orGate.pinInfo["input1"].value.data = false;
            orGate.pinInfo["input2"].value.data = true;
            orGate.evaluate();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.data);

            orGate.pinInfo["input1"].value.data = true;
            orGate.pinInfo["input2"].value.data = true;
            orGate.evaluate();
            Assert.AreEqual(true, orGate.pinInfo["output1"].value.data);
        }

        [TestMethod]
        public void testStartRunRuleItemSplitter()
        {
            rule targetRule = new rule();
            ruleItemBase spliter = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_splitter)));
            targetRule.start();

            spliter.pinInfo["input1"].value.data = false;
            spliter.evaluate();
            Assert.AreEqual(false, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(false, spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].value.data = true;
            spliter.evaluate();
            Assert.AreEqual(true, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(true, spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].valueType = typeof (pinDataString);
            spliter.pinInfo["input1"].recreateValue();
            spliter.pinInfo["input1"].value.data = "lol";
            spliter.evaluate();
            Assert.AreEqual("lol", spliter.pinInfo["output1"].value.data);
            Assert.AreEqual("lol", spliter.pinInfo["output2"].value.data);

            spliter.pinInfo["input1"].valueType = typeof(pinDataTristate);
            spliter.pinInfo["input1"].recreateValue();
            spliter.pinInfo["input1"].value.data = tristate.yes;
            spliter.evaluate();
            Assert.AreEqual(tristate.yes, spliter.pinInfo["output1"].value.data);
            Assert.AreEqual(tristate.yes, spliter.pinInfo["output2"].value.data);
        }

        [TestMethod]
        public void testRssReader()
        {
            rule targetRule = new rule();
        //   ruleItemRss rss = (ruleItemRss) targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItemRss)));
           // rss._options.url = "file://" + Path.GetFullPath(Properties.Settings.Default.testDataPath) + @"\test.rss";
           // rss.pinInfo["trigger"].value.data = true;
           //targetRule.start();
           // rss.evaluate();

           // Assert.AreEqual("test entry test details", rss.pinInfo["feed"].value.data); 
            
        }
    }
}