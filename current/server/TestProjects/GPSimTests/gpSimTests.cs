using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
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
        private static readonly chipType _knownGoodPICType = new chipType(chipType.underlyingType_t.pic16f628);
        private static readonly string _knownGoodHexFile = Properties.Settings.Default.testDataPath + "\\knownGood\\knownGood";

        private readonly frmBlank _handerForm = new frmBlank();

        [TestInitialize]
        public void init()
        {
            if (!File.Exists(_knownGoodHexFile + ".cod") || 
                !File.Exists(_knownGoodHexFile + ".lst") )
                Assert.Inconclusive("Known-good .cod or .lst file not found");

            if (!gpSim.isConfiguredCorrectly())
                Assert.Inconclusive("GPSim is unable to run due to a configuration issue");

            Thread foo = new Thread(handlerFormThreadStart);
            foo.Name = "GPSim form thread";
            foo.Start();

            while (!_handerForm.IsHandleCreated)
            {
                Thread.Sleep(1000);
            }
        }

        private void handlerFormThreadStart()
        {
            Application.Run(_handerForm);            
        }

        [TestCleanup]
        public void tearDown()
        {
            Application.Exit();
            Application.DoEvents();
        }

        [TestMethod]
        public void testGPSimConstruction()
        {
            gpSim uut = null;
            try
            {
                uut = new gpSim(_knownGoodHexFile, _handerForm);
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
                uut = new gpSim(_knownGoodHexFile, _handerForm);

                Assert.AreEqual(_knownGoodPICType, uut.chipType);
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
                uut = new gpSim(_knownGoodHexFile, _handerForm);

                // Add a breakpoint on PORTB.
                uut.addWriteBreakpoint("portb", handler);

                // and then run the node.
                uut.run();

                // Now wait for our handler to be called.
                int timeout = 1000;
                while(!handlerCalled)
                {
                    Thread.Sleep(1000);
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
