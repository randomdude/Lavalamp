using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    [TestClass]
    public class basicTests_virtualNetwork : basicTests<virtualNetwork>
    {
        [TestMethod]
        public override void verifyNodeRecognisesSync()
        {
            base._verifyNodeRecognisesSync();
        }
    }
}