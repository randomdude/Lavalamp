using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netGui;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{

    public abstract class commandTests<networkTypeToTest> : networkTest
        where networkTypeToTest : virtualNetworkBase
    {
        public abstract void verifyNodeIsPingable();
        protected void _verifyNodeIsPingable() 
        {
            // Create a new virtual network and node. Ping the node and verify that we get a successful response.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");

                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver driver = new transmitterDriver(testVirtualNetwork.getDriverConnectionPointName(), false, null);
                driver.doPing(virtualNodeID);
                Thread.Sleep(1000);

                if (testNode.state != nodeState.idle)
                    Assert.Fail("Node did not return to idle state after a successful ping");
            }
        }

        public abstract void verifyNodeNotPingableWithIncorrectAuthResponse();
        protected void _verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            // Create a new virtual network and node. 
            // We should send the initial packet to the node, and when the node challenges us, we should deliberately
            // return an incorrect value. Once we've done this, verify that the node swallows the packet, and then
            // verify that it has set its state to the initial 'idle', to check that it can recover correctly.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver driver = new transmitterDriver(testVirtualNetwork.getDriverConnectionPointName(), false, null);
                driver.setInjectFaultInvalidResponse(true);

                // We expect a commsTimeoutException from the controller, and an event onCryptoError on the node.
                bool cryptoErrorAsExpected = false;
                testNode.onCryptoError = new Action<virtualNodeBase>((node) => cryptoErrorAsExpected = true);
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
            }
        }

        public abstract void verifyNodeRespondsCorrectlyToDoIdentify();
        protected void _verifyNodeRespondsCorrectlyToDoIdentify()
        {
            // Make a new node on a new network with a test name, and verify that that name can be read
            // correctly.
            const int virtualNodeID = 0x01;

            // Do this repeatedly with various node names.
            foreach (string testNodeName in new[] { "test node", "", "a", "01234567890abcdef01234567890abcde" })
            {
                using ( virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
                {
                    virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, testNodeName);
                    startNetworkInNewThread(testVirtualNetwork);

                    // Connect to this network with a new driver class
                    transmitterDriver driver = new transmitterDriver(testVirtualNetwork.getDriverConnectionPointName(),
                                                                     false, null);

                    string recievedName = driver.doIdentify(virtualNodeID);
                    Thread.Sleep(1000);

                    Assert.AreEqual(testNode.name, recievedName, "Node identified itself with an incorrect name");

                    Assert.AreEqual(nodeState.idle, testNode.state, "Node did not return to idle state after doIdentify");
                }
            }
        }
        

        public abstract void verifyNodeIgnoresPacketsAddressedToOthers();
        protected void _verifyNodeIgnoresPacketsAddressedToOthers()
        {
            // Create a new virtual network and node. 
            // Send data to a different node on the network, which doesn't actually exist, and verify that the
            // first node does not process it.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName) )
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver driver = new transmitterDriver(testVirtualNetwork.getDriverConnectionPointName(), false, null);

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
            }
        }
    }
}
