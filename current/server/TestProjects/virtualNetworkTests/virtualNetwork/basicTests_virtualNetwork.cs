using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    [TestClass]
    public class basicTests_virtualNetwork : basicTests<CSharpNetwork>
    {
        [TestMethod]
        public override void verifyNodeRecognisesSync()
        {
            base._verifyNodeRecognisesSync();
        }

        [TestMethod]
        public override void verifyNodeRecognisesSyncWhileDesynced()
        {
            base._verifyNodeRecognisesSyncWhileDesynced();            
        }
    }

    [Ignore]
    [TestClass]
    public class basicTests_picNetwork : basicTests<simulatedPICNetwork>
    {
        [TestMethod]
        public override void verifyNodeRecognisesSync()
        {
            base._verifyNodeRecognisesSync();
        }

        [TestMethod]
        public override void verifyNodeRecognisesSyncWhileDesynced()
        {
            base._verifyNodeRecognisesSyncWhileDesynced();
        }
    }
}