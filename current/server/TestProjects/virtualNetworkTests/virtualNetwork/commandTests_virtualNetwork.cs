using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    [TestClass]
    public class commandTests_virtualNetwork : commandTests<virtualNetwork>
    {
        [TestMethod]
        public override void verifyNodeIsPingable()
        {
            base._verifyNodeIsPingable();
        }

        [TestMethod]
        public override void verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            base._verifyNodeNotPingableWithIncorrectAuthResponse();
        }

        [TestMethod]
        public override void verifyNodeIgnoresPacketsAddressedToOthers()
        {
            base._verifyNodeIgnoresPacketsAddressedToOthers();
        }
    }

    [TestClass]
    public class commandTests_gpSimNetwork : commandTests<gpSimNetwork>
    {
        [TestMethod]
        public override void verifyNodeIsPingable()
        {
            base._verifyNodeIsPingable();
        }

        [TestMethod]
        public override void verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            base._verifyNodeNotPingableWithIncorrectAuthResponse();
        }

        [TestMethod]
        public override void verifyNodeIgnoresPacketsAddressedToOthers()
        {
            base._verifyNodeIgnoresPacketsAddressedToOthers();
        }
    }

}