using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    public class networkTest
    {
        protected string pipeName;

        [TestInitialize]
        public void initialize()
        {
            pipeName = @"UnitTestNetworkPipe_" + Guid.NewGuid().ToString();
        }

        protected void startNetworkInNewThread(virtualNetworkBase testVirtualNetwork)
        {
            testVirtualNetwork.run();
        }    
    }
}