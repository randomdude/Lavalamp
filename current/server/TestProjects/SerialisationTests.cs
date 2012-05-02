namespace TestProjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using netGui;

    using ruleEngine;
    using ruleEngine.pinDataTypes;
    using ruleEngine.ruleItems;
    using ruleItems_winamp;

    using System.Windows.Forms;

    /// <summary>
    /// Tests the serialization of rules and rule item.
    /// </summary>
    [TestClass]
    public class SerialisationTests
    {
        /// <summary>
        /// Tests that an empty rule attributes is serialized.
        /// Attributes tested:
        /// - preferredWidth
        /// - preferredHeight
        /// - name
        /// - state 
        /// </summary>
        [TestMethod]
        public void testSerialisationOfEmptyRule()
        {
            Form form = new Form();
            form.Width = 200;
            form.Height = 200;
            // Create a rule named 'magicName's;
            ctlRule ruleControl = new ctlRule { Parent = form };

            rule targetRule = ruleControl.getRule();
            targetRule.changeName("", "magicName");
            targetRule.state = ruleState.running;

            // Serialize it
            string serialised = ruleControl.serialiseRule();

            // verify that serialization hasn't broken anything
            Assert.IsTrue(targetRule.name == "magicName");
            Assert.IsTrue(targetRule.state == ruleState.running);
            Assert.IsTrue(targetRule.preferredHeight == 200);
            Assert.IsTrue(targetRule.preferredWidth == 200);

            // deserialise in to a new rule
            ctlRule deSerRuleControl = new ctlRule();
            deSerRuleControl.deserialiseRule(serialised);
            targetRule = deSerRuleControl.getRule();
            // Verify that de-serialised rule has correct settings
            Assert.IsTrue(targetRule.name == "magicName", "De-serialised rule did not have the expected name - got " + targetRule.name);
            Assert.IsTrue(targetRule.state == ruleState.running, "De-serialised rule did not have the expected state - got " + targetRule.state);
            Assert.IsTrue(targetRule.preferredWidth == 200, "De-serialised rule did not have the expected width - got " + targetRule.preferredWidth);
            Assert.IsTrue(targetRule.preferredHeight == 200, "De-serialised rule did not have the expected height - got " + targetRule.preferredHeight);
        }
        
        [TestMethod]
        public void testSerialisationOfRuleWithOneItem()
        {
            testSerialisationOfRuleWithNamedRuleItem(typeof (ruleItem_and));
        }

        [TestMethod]
        public void testSerialisationOfRuleWithAnySingleRuleItem()
        {
            int count = 0;
            List<string> failedTests = new List<string>();
            // the main set of rule items
            foreach (Module myMod in Assembly.GetAssembly(typeof(rule)).GetModules()) 
                foreach (Type thisType in myMod.GetTypes())
                    if (thisType.IsDefined(typeof(ToolboxRule), false))
                    {
                        try
                        {
                            testSerialisationOfRuleWithNamedRuleItem(thisType);
                        }
                        catch (Exception ex)
                        {
                            string error = string.Format("Rule item {0} failed to serialise\nException: {1}" , thisType ,
                                                         ex);
                            failedTests.Add(error);
                            Debug.Print(error);
                        }

                        ++count;
                    }
            if (count == 0)
                throw new AssertInconclusiveException();
            count = 0;
            //netgui's rule items
            foreach (Module myMod in Assembly.GetAssembly(typeof(sensor)).GetModules())
                foreach (Type thisType in myMod.GetTypes())
                    if (thisType.IsDefined(typeof(ToolboxRule), false))
                    {
                        try 
                        {
                            testSerialisationOfRuleWithNamedRuleItem(thisType);
                        }
                        catch (Exception ex)
                        {
                            string error = string.Format("Rule item {0} failed to serialise\nException: {1}" , thisType ,
                                                         ex);
                            failedTests.Add(error);
                            Debug.Print(error);
                        }
                        ++count;
                    }
            if (count == 0)
                throw new AssertInconclusiveException();

            count = 0;
            //winamp's rule items
            foreach (Module myMod in Assembly.GetAssembly(typeof(ruleItem_winamp_base)).GetModules())
                foreach (Type thisType in myMod.GetTypes())
                    if (thisType.IsDefined(typeof(ToolboxRule), false))
                        if (thisType.IsDefined(typeof(ToolboxRule), false))
                        {
                            try 
                            {
                                testSerialisationOfRuleWithNamedRuleItem(thisType);
                            }
                            catch (Exception ex)
                            {
                                string error = string.Format("Rule item {0} failed to serialise\nException: {1}" ,
                                                             thisType , ex);
                                failedTests.Add(error);
                                Debug.Print(error);
                            }
                            ++count;
                        }
            if (count == 0)
                throw new AssertInconclusiveException();
            if (failedTests.Count > 0)
                throw new AssertFailedException(string.Join("\n",failedTests.ToArray()));
        }

        [TestMethod]
        public void testSerialisationOfDeletedLineChain()
        {

            ctlRule ruleControl = new ctlRule(){Parent = new Form()};

            lineChain newChain = new lineChain();
            rule targetRule = ruleControl.getRule();
            targetRule.AddLineChainToGlobalPool(newChain);

            // Delete the new chain
            newChain.requestDelete();

            // Ser and deser
            String serialized = ruleControl.serialiseRule();
            ctlRule deSerRuleControl = new ctlRule();
            deSerRuleControl.deserialiseRule(serialized);

            // Verify that our deleted linechain exists
            Assert.AreEqual(1, deSerRuleControl.getRule().lineChainCount);
            // and that it has been deleted
            Assert.AreEqual(true, deSerRuleControl.getRule().GetLineChainFromGuid(newChain.serial).isDeleted);
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
            targetRule.AddLineChainToGlobalPool(newChain);
            newChain.start.X = 10;
            newChain.start.Y = 20;
            newChain.end.X = 11;
            newChain.end.Y = 22;
            newChain.col = Color.CornflowerBlue;
            newChain.isdrawnbackwards = true;
            newChain.midPoints = new List<Point>();
            newChain.midPoints.Add(new Point(33, 44));

            // serialise it
            String serialised = ruleControl.serialiseRule();

            // Deserialise it!
            ctlRule deSerRuleControl = new ctlRule();
            deSerRuleControl.deserialiseRule(serialised);

            Assert.IsTrue(deSerRuleControl.getRule().lineChainCount == 1, "Deserialised rule did not have exactly one lineChain");
            Assert.IsTrue(deSerRuleControl.getRule().ruleItemCount == 1, "Deserialised rule did not have exactly one ruleItem");

            // Ensure that we can get the lineChain by the same GUID as we had before
            lineChain deSerLineChain = deSerRuleControl.getRule().GetLineChainFromGuid(newChain.serial);

            // Get the ruleItem. Note that we don't know its GUID yet so we must find it via its iterator.
            string ruleGuid = null;
            foreach (string indexer in deSerRuleControl.getRule().ruleItems.Keys)
                ruleGuid = indexer;

            Assert.IsNotNull(ruleGuid);

            rule deserialisedRule = deSerRuleControl.getRule();
            Assert.IsInstanceOfType(deserialisedRule.GetRuleItemFromGuid(new ruleItemGuid(ruleGuid)), targetType, "Deserialised rule did not preserve type of its ruleItem");
            Assert.IsTrue(deSerLineChain.start.X == 10);
            Assert.IsTrue(deSerLineChain.start.Y == 20);
            Assert.IsTrue(deSerLineChain.end.X == 11);
            Assert.IsTrue(deSerLineChain.end.Y == 22);
            Assert.IsTrue(deSerLineChain.col.R == Color.CornflowerBlue.R);
            Assert.IsTrue(deSerLineChain.col.G == Color.CornflowerBlue.G);
            Assert.IsTrue(deSerLineChain.col.B == Color.CornflowerBlue.B);
            Assert.IsFalse(deSerLineChain.isDeleted);
            Assert.IsTrue(deSerLineChain.isdrawnbackwards);
            Assert.IsTrue(deSerLineChain.midPoints.Count == 1);
            Assert.IsTrue(deSerLineChain.midPoints[0].X == 33);
            Assert.IsTrue(deSerLineChain.midPoints[0].Y == 44);
         }

        [TestMethod]
        public void testSerialisationOfRuleWithTwoRuleItemWithPinsConnected()
        {
            rule rule = new rule("test");
            lineChain line = new lineChain();
            

            ruleItemInfo myInfo = new ruleItemInfo();
            myInfo.itemType = ruleItemType.RuleItem;
            myInfo.ruleItemBaseType = typeof(ruleItem_and);
            ruleItemBase andItem = rule.addRuleItem(myInfo);
            ruleItemGuid andGuid = andItem.serial;

            ruleItemInfo myInfo2 = new ruleItemInfo();
            myInfo2.itemType = ruleItemType.RuleItem;
            myInfo2.ruleItemBaseType = typeof(ruleItem_desktopMessage);
            ruleItemBase messageItem = rule.addRuleItem(myInfo2);
            ruleItemGuid messageGuid = messageItem.serial;

            messageItem.pinInfo["trigger"].parentRuleItem = messageGuid;
            messageItem.pinInfo["trigger"].connectTo(line.serial,andItem.pinInfo["output1"]);
            andItem.pinInfo["output1"].connectTo(line.serial, messageItem.pinInfo["trigger"]);
            andItem.pinInfo["output1"].parentRuleItem = andGuid;
            rule.AddLineChainToGlobalPool(line);

            String serialised = rule.serialise();

            rule = rule.deserialise(serialised);

            Assert.AreEqual("test", rule.name);
            Assert.AreEqual(1, rule.lineChainCount);
            Assert.AreEqual(2, rule.ruleItems.Count);
            andItem = rule.ruleItems[andGuid.id.ToString()];
            messageItem = rule.ruleItems[messageGuid.id.ToString()];
            Assert.IsInstanceOfType(andItem, typeof(ruleItem_and));
            Assert.IsInstanceOfType(messageItem, typeof(ruleItem_desktopMessage));
            Assert.AreEqual(1, messageItem.pinInfo.Count);
            Assert.AreEqual(3, andItem.pinInfo.Count);
            Assert.AreEqual(line.serial.id.ToString(), messageItem.pinInfo["trigger"].parentLineChain.id.ToString());
            Assert.AreEqual(line.serial.id.ToString(), andItem.pinInfo["output1"].parentLineChain.id.ToString());
            Assert.AreEqual(andItem.pinInfo["output1"].serial.id.ToString(), messageItem.pinInfo["trigger"].linkedTo.id.ToString());
            Assert.AreEqual(messageItem.pinInfo["trigger"].serial.id.ToString(), andItem.pinInfo["output1"].linkedTo.id.ToString());
            Assert.AreEqual(typeof(pinDataBool), andItem.pinInfo["output1"].valueType);
            Assert.AreEqual(typeof(pinDataString), messageItem.pinInfo["trigger"].valueType) ;
        }    

    }
}
