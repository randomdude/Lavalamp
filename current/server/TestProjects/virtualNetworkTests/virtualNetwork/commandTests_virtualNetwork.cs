using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    /// <summary>
    /// These tests are an implementation of commandTests which test the operation of a virtual network.
    /// This verifies end-to-end operation using the c# driver code, independantly of any PIC configuration.
    /// </summary>
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
        public override void verifyNodeReturnsCorrectSensorCount()
        {
            base._verifyNodeReturnsCorrectSensorCount();
        }

        [TestMethod]
        public override void verifyNodeReturnsCorrectSensorTypes()
        {
            base._verifyNodeReturnsCorrectSensorTypes();
        }

        [TestMethod]
        public override void verifyNodeReturnsCorrectSensorTypesForSecondSensor()
        {
            base._verifyNodeReturnsCorrectSensorTypesForSecondSensor();
        }

        [TestMethod]
        public override void verifyNodeRespondsCorrectlyToDoIdentify()
        {
            base._verifyNodeRespondsCorrectlyToDoIdentify();
        }

        [TestMethod]
        public override void verifyNodeCanSetGenericDigitalOutCorrectly()
        {
            base._verifyNodeCanSetGenericDigitalOutCorrectly();
        }

        [TestMethod]
        public override void verifyNodeCanGetGenericDigitalInCorrectly()
        {
            base._verifyNodeCanGetGenericDigitalInCorrectly();
        }
    }
}