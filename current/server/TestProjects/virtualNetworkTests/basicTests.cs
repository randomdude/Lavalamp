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
        where networkTypeToTest : virtualNetwork
    {
        public abstract void verifyNodeRecognisesSync();

        protected void _verifyNodeRecognisesSync()
        {
            // Create a new virtual network and node. Send a sync packet and verify that the node fires the 
            // correct event.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = new virtualNetwork(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sync test node");

                startNetworkInNewThread(testVirtualNetwork);

                //Connect to this network with a new driver class
                transmitterDriver driver = new transmitterDriver("pipe\\" + pipeName, false, null);

                // Make sure we get that onSync event
                bool eventWasFiredOK = false;
                testVirtualNetwork.onSyncPacket = () => eventWasFiredOK = true;

                driver.doSyncNetwork();
                Thread.Sleep(1000);

                if (!eventWasFiredOK)
                    throw new AssertFailedException("Synchronization packet was not detected");
            }
        }

    }
}
