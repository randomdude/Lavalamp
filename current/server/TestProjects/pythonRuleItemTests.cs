﻿using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ruleEngine;
using ruleEngine.ruleItems;

namespace TestProjects
{
    [TestClass]
    public class pythonRuleItemTests
    {
        private const string filenameParameters =  @"\test-configuration.py";
        private const string filename = @"\test.py";
        private const string filenameCategory = @"\test-category.py";

        /// <summary>
        /// Verify load of an example python ruleItem, ensuring that the getPinInfo operates.
        /// </summary>
        [TestMethod]
        public void testLoadOfPythonRuleItem()
        {
            ruleItem_script testItem = new ruleItem_script(Properties.Settings.Default.testDataPath + filename);

            Assert.AreEqual("test python rule item", testItem.ruleName());

            Dictionary<String, pin> pins = testItem.getPinInfo();

            Assert.IsTrue(testItem.getCategory() == "");

            Assert.IsTrue(pins.Count == 2 );
            Assert.IsTrue(pins.ContainsKey("myInputPin"));
            Assert.IsTrue(pins.ContainsKey("myOutputPin"));

            Assert.IsTrue(pins["myOutputPin"].direction == pinDirection.output);
            Assert.IsTrue(pins["myInputPin"].direction == pinDirection.input);
        }

        [TestMethod]
        public void testLoadOfPythonRuleItemWithCategory()
        {
            ruleItem_script testItem = new ruleItem_script(Properties.Settings.Default.testDataPath + filenameCategory);

            Assert.AreEqual("test python rule item", testItem.ruleName());
            Assert.AreEqual("test category", testItem.getCategory());
        }

        [TestMethod]
        public void testLoadOfPythonRuleItemWithParameters()
        {
            ruleItem_script testItem = new ruleItem_script(Properties.Settings.Default.testDataPath + filenameParameters);

            Assert.AreEqual(2, testItem.parameters.Count);
            Assert.AreEqual("first value", testItem.parameters["clampToZero"]);
            Assert.AreEqual("second value", testItem.parameters["IAmTheSecondOption"]);

            Assert.AreEqual(2, testItem.parameters.Count);
            Assert.AreEqual("first value", testItem.parameters["clampToZero"]);
            Assert.AreEqual("second value", testItem.parameters["IAmTheSecondOption"]);
        }

        [TestMethod]
        public void testFunctionalityOfPythonRuleItemWithParameters()
        {
            ruleItem_script testItem = new ruleItem_script(Properties.Settings.Default.testDataPath + filenameParameters);

            testItem.parameters["clampToZero"] = "NO U";

            testItem.pinInfo["myInputPin"].value.data = true;
            testItem.evaluate();
            Assert.IsTrue(testItem.pinInfo["myOutputPin"].value.asBoolean());

            testItem.parameters["clampToZero"] = "yes";
            testItem.evaluate();
            Assert.IsFalse(testItem.pinInfo["myOutputPin"].value.asBoolean());
        }

        [TestMethod]
        public void testFunctionalityOfPythonRuleItem()
        {
            ruleItem_script testItem = new ruleItem_script(Properties.Settings.Default.testDataPath + filename);
            testItem.pinInfo["myInputPin"].value.data = false;
            testItem.evaluate();
            Assert.IsTrue((testItem.pinInfo["myOutputPin"].value.asBoolean()) == false);

            testItem.pinInfo["myInputPin"].value.data = true;
            testItem.evaluate();
            Assert.IsTrue((testItem.pinInfo["myOutputPin"].value.asBoolean()) == true);
        }


        // This will fail unless the emailItem is bodged to store your login credentials/settings.
        //[TestMethod]
        public void testFunctionalityOfEmailModule()
        {
            ruleItem_Email testItem = new ruleItem_Email();

            testItem.pinInfo["checkNow"].value.data = false;
            testItem.pinInfo["checkNow"].value.data = true;
        }   
    }
}
