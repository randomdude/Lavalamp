using Microsoft.VisualStudio.TestTools.UnitTesting;

using netGui.RuleEngine;
using netGui.RuleEngine.ruleItems;
using System.Collections.Generic;
using System;

namespace TestProjects
{
    [TestClass]
    public class UnitTests
    {
        private string filenameParameters = @"..\..\..\testData\test-configuration.py";
        private const string filename = @"..\..\..\testData\test.py";
        private const string filenameCategory = @"..\..\..\testData\test-category.py";

        /// <summary>
        /// Verify load of an example python ruleItem, ensuring that the getPinInfo operates.
        /// </summary>
        [TestMethod]
        public void testLoadOfPythonRuleItem()
        {
            pythonEngine myEng = new pythonEngine(filename);

            ruleItem_python testItem = new ruleItem_python(myEng);

            Assert.AreEqual("test python rule item", testItem.ruleName());

            Dictionary<String, pin> pins = testItem.getPinInfo();

            Assert.IsTrue(testItem.category == "");

            Assert.IsTrue(pins.Count == 2 );
            Assert.IsTrue(pins.ContainsKey("myInputPin"));
            Assert.IsTrue(pins.ContainsKey("myOutputPin"));

            Assert.IsTrue(pins["myOutputPin"].direction == pinDirection.output);
            Assert.IsTrue(pins["myInputPin"].direction == pinDirection.input);
        }

        [TestMethod]
        public void testLoadOfPythonRuleItemWithCategory()
        {
            pythonEngine myEng = new pythonEngine(filenameCategory);

            ruleItem_python testItem = new ruleItem_python(myEng);

            Assert.AreEqual("test category", myEng.category);
            Assert.AreEqual("test category", testItem.category);
        }

        [TestMethod]
        public void testLoadOfPythonRuleItemWithParameters()
        {
            pythonEngine myEng = new pythonEngine(filenameParameters);

            ruleItem_python testItem = new ruleItem_python(myEng);

            Assert.AreEqual(2, myEng.Parameters.Count);
            Assert.AreEqual("first value", myEng.Parameters["clampToZero"]);
            Assert.AreEqual("second value", myEng.Parameters["IAmTheSecondOption"]);

            Assert.AreEqual(2, testItem.parameters.Count );
            Assert.AreEqual("first value", testItem.parameters["clampToZero"]);
            Assert.AreEqual("second value", testItem.parameters["IAmTheSecondOption"]);
        }

        [TestMethod]
        public void testFunctionalityOfPythonRuleItemWithParameters()
        {
            pythonEngine myEng = new pythonEngine(filenameParameters);

            ruleItem_python testItem = new ruleItem_python(myEng);

            testItem.parameters["clampToZero"] = "NO U";

            testItem.pinInfo["myInputPin"].value.setData(true);
            testItem.evaluate();
            Assert.IsTrue((bool)testItem.pinInfo["myOutputPin"].value.getData());

            testItem.parameters["clampToZero"] = "yes";
            testItem.evaluate();
            Assert.IsFalse((bool)testItem.pinInfo["myOutputPin"].value.getData());
        }

        [TestMethod]
        public void testFunctionalityOfPythonRuleItem()
        {
            pythonEngine myEng = new pythonEngine(filename);

            ruleItem_python testItem = new ruleItem_python(myEng);
            testItem.myEng.pinList["myInputPin"].value.setData(false);
            testItem.evaluate();
            Assert.IsTrue(((bool)testItem.pinInfo["myOutputPin"].value.getData()) == false);

            testItem.myEng.pinList["myInputPin"].value.setData(true);
            testItem.evaluate();
            Assert.IsTrue(((bool)testItem.myEng.pinList["myOutputPin"].value.getData()) == true);
        }


        // This will fail unless the emailItem is bodged to store your login credentials/settings.
        //[TestMethod]
        public void testFunctionalityOfEmailModule()
        {
            netGui.RuleEngine.ruleItems.ruleItem_Email testItem = new netGui.RuleEngine.ruleItems.ruleItem_Email();

            testItem.pinInfo["checkNow"].value.setData(false);
            testItem.pinInfo["checkNow"].value.setData(true);
        }   
    }
}
