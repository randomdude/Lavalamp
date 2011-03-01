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
            pipeName = @"UnitTestNetworkPipe3_" + DateTime.Now.TimeOfDay.ToString().Replace(":", "_");
        }

        protected void startNetworkInNewThread(virtualNetwork testVirtualNetwork)
        {
            testVirtualNetwork.run();
        }    
    }
}