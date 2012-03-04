using Microsoft.VisualStudio.TestTools.UnitTesting;
using ruleEngine;
using ruleEngine.pinDataTypes;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.Starts;
using ruleEngine.ruleItems.windows;

namespace TestProjects
{
    [TestClass]
    public class ruleTests
    {
        /// <summary>
        /// Verify that the StartRun ruleItem fires at the start of a run
        /// </summary>
        [TestMethod]
        public void testStartRunRuleItem()
        {
            rule foo = new rule();
            ruleItemBase newItem = foo.addRuleItem(new ruleItemInfo(typeof(ruleItem_startRun)));

            foo.start();
            Assert.AreEqual(false, newItem.pinInfo["StartOfSim"].value.data);
            foo.advanceDelta();
            Assert.AreEqual(true, newItem.pinInfo["StartOfSim"].value.data);
            foo.advanceDelta();
            Assert.AreEqual(false, newItem.pinInfo["StartOfSim"].value.data);
            foo.stop();
        }

        /// <summary>
        /// Verify that a debug item correctly reacts to, and outputs, a value
        /// </summary>
        [TestMethod]
        public void testStartRunRuleItemLinkedToDebugItem()
        {
            rule targetRule = new rule();
            ruleItemBase startItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_startRun)));
            ruleItemBase debugItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_debug)));

            lineChain newChain = new lineChain();
            startItem.pinInfo["StartOfSim"].connectTo(newChain.serial, debugItem.pinInfo["input"]);
            targetRule.AddLineChainToGlobalPool(newChain);

            targetRule.start();
            Assert.AreEqual(false, startItem.pinInfo["StartOfSim"].value.data);
            Assert.AreEqual(tristate.noValue, debugItem.pinInfo["input"].value.data);
            targetRule.advanceDelta();
            Assert.AreEqual(true, startItem.pinInfo["StartOfSim"].value.data);
            Assert.AreEqual(tristate.yes, debugItem.pinInfo["input"].value.data);
            targetRule.advanceDelta();
            Assert.AreEqual(false, startItem.pinInfo["StartOfSim"].value.data);
            Assert.AreEqual(tristate.no, debugItem.pinInfo["input"].value.data);
            targetRule.stop();
        }

        /// <summary>
        /// Verify that an inverter item correctly reacts to, and outputs, a value
        /// </summary>
        [TestMethod]
        public void testStartRunRuleItemLinkedToInverter()
        {
            rule targetRule = new rule();
            ruleItemBase startItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_startRun)));
            ruleItemBase invItem = targetRule.addRuleItem(new ruleItemInfo(typeof(ruleItem_not)));

            lineChain newChain = new lineChain();
            startItem.pinInfo["StartOfSim"].connectTo(newChain.serial, invItem.pinInfo["input1"]);
            targetRule.AddLineChainToGlobalPool(newChain);

            targetRule.start();
            Assert.AreEqual(false, startItem.pinInfo["StartOfSim"].value.data);
            Assert.AreEqual(tristate.noValue, invItem.pinInfo["input1"].value.data);
            targetRule.advanceDelta();
            Assert.AreEqual(true, startItem.pinInfo["StartOfSim"].value.data);
            Assert.AreEqual(tristate.yes, invItem.pinInfo["input1"].value.data);
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.no, invItem.pinInfo["output1"].value.data);
            Assert.AreEqual(false, startItem.pinInfo["StartOfSim"].value.data);
            targetRule.advanceDelta();
            Assert.AreEqual(tristate.no, invItem.pinInfo["input1"].value.data);
            Assert.AreEqual(tristate.yes, invItem.pinInfo["output1"].value.data);
            targetRule.stop();
        }

    }
}