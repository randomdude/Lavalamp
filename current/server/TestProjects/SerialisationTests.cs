using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ruleEngine;
using ruleEngine.ruleItems;

namespace TestProjects
{
    /// <summary>
    /// Summary description for SerialisationTests
    /// </summary>
    [TestClass]
    public class SerialisationTests
    {
        [TestMethod]
        public void testSerialisationOfEmptyRule()
        {
            // Create a rule named 'magicName;
            ctlRule ruleControl = new ctlRule();

            rule targetRule = ruleControl.getRule();
            targetRule.name = "magicName";
            targetRule.state = ruleState.running;

            // Serialise it
            string serialised = ruleControl.serialiseRule();

            // verify that serialisation hasn't broken anything
            Assert.IsTrue(targetRule.name == "magicName");
            Assert.IsTrue(targetRule.state == ruleState.running);

            // deserialise in to a new rule
            ctlRule deSerRuleControl = new ctlRule();
            deSerRuleControl.deserialiseRule(serialised);

            // Verify that deserialised rule has correct setings
            Assert.IsTrue(deSerRuleControl.getRule().name == "magicName", "Deserialised rule did not have the expected name - got " + deSerRuleControl.getRule().name);
            Assert.IsTrue(deSerRuleControl.getRule().state == ruleState.running, "Deserialised rule did not have the expected state - got " + deSerRuleControl.getRule().state);
        }
        
        [TestMethod]
        public void testSerialisationOfRuleWithOneItem()
        {
            testSerialisationOfRuleWithNamedRuleItem(typeof (ruleItem_and));
        }

        [TestMethod]
        public void testSerialisationOfRuleWithAnySingleRuleItem()
        {
            foreach (Module myMod in Assembly.GetExecutingAssembly().GetModules()) 
                foreach (Type thisType in myMod.GetTypes()) 
                    if (thisType.IsDefined(typeof (ToolboxRule), false))
                        testSerialisationOfRuleWithNamedRuleItem(thisType);
        }
        
        private void testSerialisationOfRuleWithNamedRuleItem(Type targetType)
        {
            ctlRule ruleControl = new ctlRule();

            // Make a new rule with one RuleItem, of the specified type, and one lineChain.
            ruleItemInfo myInfo = new ruleItemInfo();
            myInfo.itemType = ruleItemType.RuleItem;
            myInfo.ruleItemBaseType = targetType;
            ruleControl.addRuleItem(myInfo);

            lineChain newChain = new lineChain();
            rule targetRule = ruleControl.getRule();
            targetRule.lineChains.Add(newChain.serial.id.ToString() , newChain);
            targetRule.lineChains[newChain.serial.id.ToString()].start.X = 10;
            targetRule.lineChains[newChain.serial.id.ToString()].start.Y = 20;
            targetRule.lineChains[newChain.serial.id.ToString()].end.X = 11;
            targetRule.lineChains[newChain.serial.id.ToString()].end.Y = 22;
            targetRule.lineChains[newChain.serial.id.ToString()].col = Color.CornflowerBlue;
            targetRule.lineChains[newChain.serial.id.ToString()].deleted = true;
            targetRule.lineChains[newChain.serial.id.ToString()].isdrawnbackwards = true;
            targetRule.lineChains[newChain.serial.id.ToString()].midPoints = new List<Point>();
            targetRule.lineChains[newChain.serial.id.ToString()].midPoints.Add(new Point(33, 44));
            targetRule.lineChains[newChain.serial.id.ToString()].destPin = new pinGuid();
            targetRule.lineChains[newChain.serial.id.ToString()].sourcePin = new pinGuid();

            // serialise it
            String serialised = ruleControl.serialiseRule();

            // Deserialise it!
            ctlRule deSerRuleControl = new ctlRule();
            deSerRuleControl.deserialiseRule(serialised);

            Assert.IsTrue(deSerRuleControl.getRule().lineChains.Keys.Count == 1, "Deserialised rule did not have exactly one lineChain");
            Assert.IsTrue(deSerRuleControl.getRule().ruleItems.Count == 1, "Deserialised rule did not have exactly one ruleItem");

            string chainGuid = null;
            foreach (string indexer in deSerRuleControl.getRule().lineChains.Keys)
                chainGuid = indexer;

            string ruleGuid = null;
            foreach (string indexer in deSerRuleControl.getRule().ruleItems.Keys)
                ruleGuid = indexer;

            rule deserialisedRule = deSerRuleControl.getRule();
            Assert.IsInstanceOfType(deserialisedRule.ruleItems[ruleGuid], targetType, "Deserialised rule did not preserve type of its ruleItem");
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].start.X == 10);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].start.Y == 20);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].end.X == 11);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].end.Y == 22);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].col.R == Color.CornflowerBlue.R);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].col.G == Color.CornflowerBlue.G);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].col.B == Color.CornflowerBlue.B);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].deleted);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].isdrawnbackwards);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].midPoints.Count == 1);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].midPoints[0].X == 33);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].midPoints[0].Y == 44);
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].destPin.id.ToString() == ruleControl.getRule().lineChains[newChain.serial.id.ToString()].destPin.id.ToString());
            Assert.IsTrue(deserialisedRule.lineChains[chainGuid].sourcePin.id.ToString() == ruleControl.getRule().lineChains[newChain.serial.id.ToString()].sourcePin.id.ToString());
        }
        /*
        [TestMethod]
        public void testSerialisationOfRuleWithOneRuleItemWithPinsConnected()
        {
            ctlRule ruleControl = new ctlRule();

            ruleItemInfo myInfo = new ruleItemInfo();
            myInfo.itemType = ruleItemType.RuleItem;

            myInfo.ruleItemBaseType = typeof(ruleItem_and);

            ruleControl.addRuleItem(myInfo);

            Dictionary<String, pin> pinInfo = ((ruleItem_and) ruleControl.targetRule.ruleItems[0].targetRuleItem).getPinInfo();

            PictureBox input1Box = pinInfo["input1"].imageBox;
            PictureBox output1Box = pinInfo["output1"].imageBox;

            // connect the input1 pin to the output1 pin
            ruleControl.startOrFinishLine(input1Box);
            ruleControl.startOrFinishLine(output1Box);

            ruleControl.targetRule.lineChains.Add(new lineChain());
            ruleControl.targetRule.lineChains[0].start.X = 10;
            ruleControl.targetRule.lineChains[0].start.Y = 20;
            ruleControl.targetRule.lineChains[0].end.X = 11;
            ruleControl.targetRule.lineChains[0].end.Y = 22;
            ruleControl.targetRule.lineChains[0].col = Color.CornflowerBlue;
            ruleControl.targetRule.lineChains[0].deleted = true;
            ruleControl.targetRule.lineChains[0].isdrawnbackwards = true;
            ruleControl.targetRule.lineChains[0].points.Add(new Point(33, 44));

            String serialised = ruleControl.SerialiseRule();

            ctlRule deSerRuleControl = new ctlRule(serialised);

            Assert.IsInstanceOfType(deSerRuleControl.targetRule.ruleItems[0].targetRuleItem, typeof(ruleItem_and));
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].start.X == 10);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].start.Y == 20);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].end.X == 11);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].end.Y == 22);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].col.R == Color.CornflowerBlue.R);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].col.G == Color.CornflowerBlue.G);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].col.B == Color.CornflowerBlue.B);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].deleted);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].isdrawnbackwards);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].points[0].X == 33);
            Assert.IsTrue(deSerRuleControl.targetRule.lineChains[0].points[0].Y == 44);
        }    
        */
    }
}
