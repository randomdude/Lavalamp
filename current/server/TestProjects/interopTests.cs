using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netGui;
using transmitterDriver;

namespace TestProjects
{
    [TestClass]
    public class interopTests
    {
        [TestMethod]
        public void testConfig_tMarshalling()
        {
            // Get a test config from the other side of our interop boundary
            appConfig_t testConfig = transmitterDriver.transmitter.getTestConfig();

            // Bytes
            Assert.AreEqual(0x01, testConfig.nodeid);
            Assert.AreEqual(0x02, testConfig.sensorid);

            // Ints
            Assert.AreEqual(0x22, testConfig.verbose);
            Assert.AreEqual(0x14, testConfig.com_timeout);
            Assert.AreEqual(0x15, testConfig.retries);
            Assert.AreEqual(0x01020304, testConfig.key1);
            Assert.AreEqual(0x05060708, testConfig.key2);
            Assert.AreEqual(0x090a0b0c, testConfig.key3);
            Assert.AreEqual(0x0d0e0f00, testConfig.key4);

            // bools
            Assert.AreEqual(true, testConfig.useEncryption);
            Assert.AreEqual(false, testConfig.machineoutput);
            Assert.AreEqual(true, testConfig.assume_synced);
            Assert.AreEqual(false, testConfig.isSerialPort);
            Assert.AreEqual(true, testConfig.injectFaultInvalidResponse);

            // Strings
            Assert.AreEqual("some port", testConfig.portName);

            // IntPtrs
            Assert.AreEqual( (IntPtr) 0x10111213, testConfig.hnd);
        }
    }
}
