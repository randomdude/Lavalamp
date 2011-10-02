using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.GPSimTests
{
    /// <summary>
    /// Here we test our GPSim class by firing it up with some known-good .hex files. The focus here is to
    /// test the GPSim interaction, not to test the PIC code itself, so we avoid using any lavalamp PIC code.
    /// </summary>
    [TestClass]
    public class gpSimTests
    {
        private const chipType knownGoodPICType = chipType.p16f628;
        private static readonly string _knownGoodHexFile = Properties.Settings.Default.testDataPath + "\\knownGood\\knownGood";

        [TestMethod]
        public void testGPSimConstruction()
        {
            gpSim uut = null;
            try
            {
                uut = new gpSim(_knownGoodHexFile);
            }
            finally
            {
                if (uut != null)
                    uut.Dispose();
            }

        }

        [TestMethod]
        public void testGPSimLoadsToCorrectType()
        {
            gpSim uut = null;
            try
            {
                uut = new gpSim(_knownGoodHexFile);

                Assert.AreEqual(knownGoodPICType, uut.chipType);
            }
            finally
            {
                if (uut != null)
                    uut.Dispose();
            }

        }

        [TestMethod]
        public void testGPSimRunsToPortBreakpoint()
        {
            gpSim uut = null;
            try
            {
                uut = new gpSim(_knownGoodHexFile);

                // Add a breakpoint on PORTA.
                uut.addWriteBreakpoint("portb", handler);

                // and then run the node.
                uut.run();

                // Now wait for our handler to be called.
                int timeout = 10;
                while(!handlerCalled)
                {
                    Thread.Sleep(5000);
                    timeout--;

                    if (timeout == 0)
                        throw new TimeoutException();
                }

                // OK, the handler was called. Ace. Ensure that it was set to the
                // correct value.
                Assert.AreEqual(0x01, portState & 0x01);
            }
            finally
            {
                if (uut != null)
                    uut.Dispose();
            }

        }

        private bool handlerCalled = false;
        private int portState = 0;
        private void handler(gpSim sender, breakpoint hit)
        {
            portState = sender.readMemory("portb");
            handlerCalled = true;
        }
    }
}
