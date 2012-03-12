using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ruleEngine;

namespace TestProjects
{
    [TestClass]
    public class loadSaveRuleTests
    {
        [TestMethod]
        public void renameRuleTest()
        {
            rule toRename = new rule("Heresy");
            toRename.changeName("","Love");
            Assert.IsFalse(File.Exists("Love.rule"));
            Assert.IsFalse(File.Exists("Heresy.rule"));
            toRename.changeName("","Hurt");
            toRename.saveToDisk("Hurt.rule");
            Assert.IsTrue(File.Exists("Hurt.rule"));
            toRename.changeName("","Pain");
            Assert.IsTrue(File.Exists("Pain.rule"));
            File.Delete("Pain.rule");
        }
    }
}
