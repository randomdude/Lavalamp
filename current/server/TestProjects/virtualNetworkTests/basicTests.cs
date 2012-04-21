using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netGui;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    /// <summary>
    /// The MS test runner requires methods to be defined in the TestClass, and not its base class, and that the TestClass
    /// is non-generic. Because of this, we need to create a class for each class we wish to test, with a generic type
    /// set to the class we are testing, with a TestMethod for each test method in the base. We have a number of abstract
    /// methods in the base class, so that it is harder to forget to implement any here.
    /// </summary>
    public abstract class basicTests<networkTypeToTest> : networkTest
        where networkTypeToTest : virtualNetworkBase
    {
        public abstract void verifyNodeRecognisesSync();

        protected void _verifyNodeRecognisesSync()
        {
            // Create a new virtual network and node. Send a sync packet and verify that the node fires the 
            // correct event.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName, true))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sync test node");

                startNetworkInNewThread(testVirtualNetwork);

                //Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter("pipe\\" + pipeName, false, null);

                // Make sure we get that onSync event
                bool eventWasFiredOK = false;
                testNode.onSyncPacket = (sender) => eventWasFiredOK = true;

                driver.doSyncNetwork();
                Thread.Sleep(1000);

                if (!eventWasFiredOK)
                    throw new AssertFailedException("Synchronization packet was not detected");
            }
        }

        public abstract void verifyNodeRecognisesSyncWhileDesynced();

        protected void _verifyNodeRecognisesSyncWhileDesynced()
        {
            // Create a new virtual network and node. 
            // Cause the network to be out of sync, and then send a sync
            // packet and verify that the node recovers, successfully firing the correct event.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName, true))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sync test node");

                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter("pipe\\" + pipeName, false, null);

                // Place network in to desynchronised state
                driver.injectFaultDesync();

                // Make sure we get that onSync event
                bool eventWasFiredOK = false;
                testNode.onSyncPacket = (sender) => eventWasFiredOK = true;

                driver.doSyncNetwork();
                Thread.Sleep(1000);

                if (!eventWasFiredOK)
                    throw new AssertFailedException("Synchronization packet was not detected");
            }
        }
    }
}
