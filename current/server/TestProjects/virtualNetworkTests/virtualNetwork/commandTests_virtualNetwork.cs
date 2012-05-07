using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{
    /// <summary>
    /// These tests are an implementation of commandTests which test the operation of a virtual network.
    /// This verifies end-to-end operation using the c# driver code, independently of any PIC configuration.
    /// </summary>
    [TestClass]
    public class commandTests_virtualNetwork : commandTests<CSharpNetwork>
    {
        [TestMethod]
        public override void verifyNodeIsPingable()
        {
            try
            {
                base._verifyNodeIsPingable();
            }catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            try
            {
                base._verifyNodeNotPingableWithIncorrectAuthResponse();
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
            

        [TestMethod]
        public override void verifyNodeIgnoresPacketsAddressedToOthers()
        {
            try
            {
                base._verifyNodeIgnoresPacketsAddressedToOthers();
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeReturnsCorrectSensorCount()
        {
            try
            {
                base._verifyNodeReturnsCorrectSensorCount();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeReturnsCorrectSensorTypes()
        {
            try
            {
                base._verifyNodeReturnsCorrectSensorTypes();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeReturnsCorrectSensorTypesForSecondSensor()
        {
            try
            {
                base._verifyNodeReturnsCorrectSensorTypesForSecondSensor();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeRespondsCorrectlyToDoIdentify()
        {
            try
            {
                base._verifyNodeRespondsCorrectlyToDoIdentify();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public override void verifyNodeCanSetGenericDigitalOutCorrectly()
        {
            try
            {
                base._verifyNodeCanSetGenericDigitalOutCorrectly();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            
        }

        [TestMethod]
        public override void verifyNodeCanGetGenericDigitalInCorrectly()
        {
            try
            {
                base._verifyNodeCanGetGenericDigitalInCorrectly();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}