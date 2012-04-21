using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    public class networkTest
    {
        public string pipeName { get; private set; }

        [TestInitialize]
        public void initialize()
        {
            pipeName = @"UnitTestNetworkPipe_" + Guid.NewGuid().ToString();
        }

        protected void startNetworkInNewThread(virtualNetworkBase testVirtualNetwork)
        {
            Thread networkThread = new Thread(testVirtualNetwork.run);
            networkThread.Start();
        }    
    }
}