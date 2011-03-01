using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbridge;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    [TestClass]
    public class commandTests : networkTest
    {
        [TestMethod]
        public void verifyNodeIsPingable()
        {
            // Create a new virtual network and node. Ping the node and verify that we get a successful response.
            const int virtualNodeID = 0x01;

            virtualNetwork testVirtualNetwork = new virtualNetwork(pipeName);
            virtualNode testNode = new virtualNode(virtualNodeID, "Ping test node");
            testVirtualNetwork.AddNode(testNode);

            startNetworkInNewThread(testVirtualNetwork);

            // Connect to this network with a new driver class
            transmitterDriver driver = new transmitterDriver("pipe\\" + pipeName, false, null);
            driver.doPing(virtualNodeID);
            Thread.Sleep(1000);

            if (testNode.state != nodeState.idle)
                Assert.Fail("Node did not return to idle state after a successful ping");

            testVirtualNetwork.plzdie();
        }

        [TestMethod]
        public void verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            // Create a new virtual network and node. 
            // We should send the initial packet to the node, and when the node challenges us, we should deliberately
            // return an incorrect value. Once we've done this, verify that the node swallows the packet, and then
            // verify that it has set its state to the initial 'idle', to check that it can recover correctly.
            const int virtualNodeID = 0x01;

            virtualNetwork testVirtualNetwork = new virtualNetwork(pipeName);
            virtualNode testNode = new virtualNode(virtualNodeID, "Ping test node");
            testVirtualNetwork.AddNode(testNode);
            startNetworkInNewThread(testVirtualNetwork);

            // Connect to this network with a new driver class
            transmitterDriver driver = new transmitterDriver("pipe\\" + pipeName, false, null);
            driver.setInjectFaultInvalidResponse(true);

            // We expect a commsTimeoutException from the controller, and an event onCryptoError on the node.
            bool cryptoErrorAsExpected = false;
            testNode.onCryptoError = new Action<virtualNode>((node) => cryptoErrorAsExpected = true);
            bool exceptionAsExpected = false;
            try
            {
                driver.doPing(virtualNodeID);
                Thread.Sleep(1000);
            }
            catch (commsTimeoutException)
            {
                exceptionAsExpected = true;
            }

            if (!cryptoErrorAsExpected)
                Assert.Fail("Node did not signal a crypto error");
            if (testNode.state != nodeState.idle)
                Assert.Fail("Node did not return to idle state after a bad crypto response");
            if (!exceptionAsExpected)
                Assert.Fail("Controller did not timeout when accessing a node with a bad crypto response");

            testVirtualNetwork.plzdie();
        }

        [TestMethod]
        public void verifyNodeIgnoresPacketsAddressedToOthers()
        {
            // Create a new virtual network and node. 
            // Send data to a different node on the network, which doesn't actually exist, and verify that the
            // first node does not process it.
            const int virtualNodeID = 0x01;

            virtualNetwork testVirtualNetwork = new virtualNetwork(pipeName);
            virtualNode testNode = new virtualNode(virtualNodeID, "Ping test node");
            testVirtualNetwork.AddNode(testNode);
            startNetworkInNewThread(testVirtualNetwork);

            // Connect to this network with a new driver class
            transmitterDriver driver = new transmitterDriver("pipe\\" + pipeName, false, null);
            
            // We expect a commsTimeoutException from the controller, and an event onCryptoError on the node.
            bool exceptionAsExpected = false;
            try
            {
                driver.doPing(virtualNodeID + 1);
                Thread.Sleep(1000);
            }
            catch (commsTimeoutException)
            {
                exceptionAsExpected = true;
            }

            if (!exceptionAsExpected)
                Assert.Fail("Network did not timeout when a non-existent node was accessed");
            if (testNode.state != nodeState.idle)
                Assert.Fail("Node did not remain in idle state while something else on the network was accessed");

            testVirtualNetwork.plzdie();
        }
    }
}
