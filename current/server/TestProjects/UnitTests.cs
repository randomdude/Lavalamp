using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netGui.RuleEngine;
using netGui.RuleEngine.ruleItems;

namespace TestProjects
{
    [TestClass]
    public class UnitTests
    {
        private TestContext testContextInstance;
        private string filenameParameters = @"C:\lavalamp\current\server\netGui\RuleEngine\ruleItems\pythonScripts\test-configuration.py";
        private const string filename = @"C:\lavalamp\current\server\netGui\RuleEngine\ruleItems\pythonScripts\test.py";
        private const string filenameCategory = @"C:\lavalamp\current\server\netGui\RuleEngine\ruleItems\pythonScripts\test-category.py";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void testLoadOfPythonModule()
        {
            pythonEngine myEng = new pythonEngine(filename);

            Assert.AreEqual("test python rule item", myEng.description);

            Assert.IsTrue(myEng.category == "");

            Assert.IsTrue(myEng.pinList.ContainsKey("myInputPin"));
            Assert.IsTrue(myEng.pinList.ContainsKey("myOutputPin"));

            Assert.IsTrue(myEng.pinList["myOutputPin"].direction == pinDirection.output);
            Assert.IsTrue(myEng.pinList["myInputPin"].direction == pinDirection.input);
        }

        [TestMethod]
        public void testLoadOfPythonRuleItem()
        {
            pythonEngine myEng = new pythonEngine(filename);

            ruleItem_python testItem = new ruleItem_python(myEng);

            Assert.AreEqual("test python rule item", testItem.ruleName());

            Dictionary<String, pin> pins = testItem.getPinInfo();

            Assert.IsTrue(testItem.category == "");

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

            testItem.pinStates["myInputPin"] = true;
            testItem.evaluate();
            Assert.IsTrue( (bool)testItem.pinStates["myOutputPin"] );

            testItem.parameters["clampToZero"] = "yes";
            testItem.evaluate();
            Assert.IsFalse((bool)testItem.pinStates["myOutputPin"]);
        }

        [TestMethod]
        public void testFunctionalityOfPythonRuleItem()
        {
            pythonEngine myEng = new pythonEngine(filename);

            ruleItem_python testItem = new ruleItem_python(myEng);
            testItem.pinStates["myInputPin"] = false;
            testItem.evaluate();
            Assert.IsTrue(((bool)testItem.pinStates["myOutputPin"]) == false);

            testItem.pinStates["myInputPin"] = true;
            testItem.evaluate();
            Assert.IsTrue(((bool)testItem.pinStates["myOutputPin"]) == true);
        }


        // This will fail unless the emailItem is bodged to store your login credentials/settings.
        //[TestMethod]
        public void testFunctionalityOfEmailModule()
        {
            netGui.RuleEngine.ruleItems.ruleItem_Email testItem = new netGui.RuleEngine.ruleItems.ruleItem_Email();

            testItem.pinStates["checkNow"] = false;
            testItem.pinStates["checkNow"] = true;
        }   
    }
}
