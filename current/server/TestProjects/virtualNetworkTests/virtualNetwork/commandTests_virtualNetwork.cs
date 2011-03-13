using System;
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

        [TestMethod]
        public override void verifyNodeResturnsCorrectSensorCount()
        {
            base._verifyNodeResturnsCorrectSensorCount();
        }

        [TestMethod]
        public override void verifyNodeRespondsCorrectlyToDoIdentify()
        {
            base._verifyNodeRespondsCorrectlyToDoIdentify();
        }
    }
}