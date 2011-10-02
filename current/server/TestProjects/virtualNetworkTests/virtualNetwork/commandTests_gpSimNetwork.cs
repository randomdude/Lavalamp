using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    /// <summary>
    /// These tests verify correct end-to-end operation from the C#-level transmitter driver all the way
    /// down to simulated PIC code. They are intended to test the PIC code itself (see 
    /// commandTests_virtualNetwork for tests which test only the transmitting driver code).
    /// </summary>
    [Ignore]
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
        public override void verifyNodeRespondsCorrectlyToDoIdentify()
        {
            base._verifyNodeRespondsCorrectlyToDoIdentify();
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
        public override void verifyNodeCanSetGenericDigitalOutCorrectly()
        {
            base._verifyNodeCanSetGenericDigitalOutCorrectly();
        }

        [TestMethod]
        public override void verifyNodeIgnoresPacketsAddressedToOthers()
        {
            base._verifyNodeIgnoresPacketsAddressedToOthers();
        }

        [TestMethod]
        public override void verifyNodeCanGetGenericDigitalInCorrectly()
        {
            base._verifyNodeCanGetGenericDigitalInCorrectly();
        }
    }
}