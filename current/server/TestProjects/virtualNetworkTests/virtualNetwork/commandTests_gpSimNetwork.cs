namespace TestProjects.virtualNetworkTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using virtualNodeNetwork;
    /// <summary>
    /// These tests verify correct end-to-end operation from the C#-level transmitter driver all the way
    /// down to simulated PIC code. They are intended to test the PIC code itself (see 
    /// commandTests_virtualNetwork for tests which test only the transmitting driver code).
    /// </summary>
     [Ignore]
    [TestClass]
    public class commandTests_gpSimNetwork : commandTests<simulatedPICNetwork>
    {
        //[TestMethod]
        //public void verifyNodeIsPingable_wat()
        //{
        //    // Create a new virtual network and node. Ping the node and verify that we get a successful response.
        //    const int virtualNodeID = 0x01;

        //    using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<simulatedPICNetwork>("wat"))
        //    {
        //        virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");

        //        //startNetworkInNewThread(testVirtualNetwork);

        //        // Connect to this network with a new driver class
        //        transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);
        //        driver.doPing(virtualNodeID);
        //        Thread.Sleep(1000);

        //        if (testNode.state != nodeState.idle)
        //            Assert.Fail("Node did not return to idle state after a successful ping");
        //    }

        //}

        [TestMethod]
        public override void verifyNodeIsPingable()
        {
            //throw new NotImplementedException();
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