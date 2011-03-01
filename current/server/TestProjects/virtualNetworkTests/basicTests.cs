using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbridge;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    [TestClass]
    public class basicTests : networkTest
    {
        [TestMethod]
        public void verifyNodeRecognisesSync()
        {
            // Create a new virtual network and node. Send a sync packet and verify that the node fires the 
            // correct event.
            const int virtualNodeID = 0x01;

            virtualNetwork testVirtualNetwork = new virtualNetwork(pipeName);
            virtualNode testNode = new virtualNode(virtualNodeID, "Sync test node");
            testVirtualNetwork.AddNode(testNode);

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

            testVirtualNetwork.plzdie();
        }

    }
}
