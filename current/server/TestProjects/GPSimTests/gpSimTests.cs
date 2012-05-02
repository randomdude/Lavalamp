using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using virtualNodeNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjects.GPSimTests
{
    using System.ComponentModel;

    using Moq;

    /// <summary>
    /// Here we test our GPSim class by firing it up with some known-good .hex files. The focus here is to
    /// test the GPSim interaction, not to test the PIC code itself, so we avoid using any lavalamp PIC code.
    /// </summary>
    [Ignore]
    [TestClass]
    public class gpSimTests
    {
        private static readonly chipType _knownGoodPICType = new chipType(chipType.underlyingType_t.pic16f628);
        private static readonly string _knownGoodHexFile = Properties.Settings.Default.testDataPath + "\\knownGood\\knownGood";

        private readonly Mock<ISynchronizeInvoke> _handerForm = new Mock<ISynchronizeInvoke>();

        [TestInitialize]
        public void init()
        {
            _handerForm.Setup(s => s.InvokeRequired).Returns(true);
            _handerForm.Setup(s => s.Invoke(It.IsAny<gpSim.breakpointHandler>(), It.IsAny<object[]>())).Returns(new object());
            _handerForm.Setup(s => s.BeginInvoke(It.IsAny<gpSim.breakpointHandler>(), It.IsAny<object[]>())).Returns(new Mock<IAsyncResult>(MockBehavior.Loose).Object);
            _handerForm.Setup(s => s.EndInvoke(It.IsAny<IAsyncResult>()));
            if (!File.Exists(_knownGoodHexFile + ".cod") || 
                !File.Exists(_knownGoodHexFile + ".lst") )
                Assert.Inconclusive("Known-good .cod or .lst file not found");

            if (!gpSim.isConfiguredCorrectly())
                Assert.Inconclusive("GPSim is unable to run due to a configuration issue");

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
                uut = new gpSim(_knownGoodHexFile, _handerForm.Object);
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
                uut = new gpSim(_knownGoodHexFile, _handerForm.Object);
                
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
                uut = new gpSim(_knownGoodHexFile, _handerForm.Object);

                // Add a breakpoint on PORTB.
                breakpoint brk = new breakpoint(){callback = this.handler,location = "portb"};
                uut.addWriteBreakpoint(brk);
               
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
                _handerForm.Verify(i => i.BeginInvoke(It.IsAny<gpSim.breakpointHandler>(), It.Is<object[]>(x => x == new object[] { uut, brk })), Times.Once());
               
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
