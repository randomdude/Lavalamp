using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netGui;
using transmitterDriver;
using virtualNodeNetwork;

namespace TestProjects.virtualNetworkTests
{

    public abstract class commandTests<networkTypeToTest> : networkTest
        where networkTypeToTest : virtualNetworkBase
    {
        public abstract void verifyNodeIsPingable();
        protected void _verifyNodeIsPingable()
        {
            // Create a new virtual network and node. Ping the node and verify that we get a successful response.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");

                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);
                driver.doPing(virtualNodeID);
                Thread.Sleep(1000);

                if (testNode.state != nodeState.idle)
                    Assert.Fail("Node did not return to idle state after a successful ping");
            }
        }

        public abstract void verifyNodeNotPingableWithIncorrectAuthResponse();
        protected void _verifyNodeNotPingableWithIncorrectAuthResponse()
        {
            // Create a new virtual network and node. 
            // We should send the initial packet to the node, and when the node challenges us, we should deliberately
            // return an incorrect value. Once we've done this, verify that the node swallows the packet, and then
            // verify that it has set its state to the initial 'idle', to check that it can recover correctly.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);
                driver.setInjectFaultInvalidResponse(true);

                // We expect a commsTimeoutException from the controller, and an event onCryptoError on the node.
                bool cryptoErrorAsExpected = false;
                testNode.onCryptoError = new Action<virtualNodeBase>((node) => cryptoErrorAsExpected = true);
                bool exceptionAsExpected = false;
                try
                {
                    driver.doPing(virtualNodeID);
                    Thread.Sleep(1000);
                }
                catch (commsTimeoutException)
                {
                    exceptionAsExpected = true;
                }

                if (!cryptoErrorAsExpected)
                    Assert.Fail("Node did not signal a crypto error");
                if (testNode.state != nodeState.idle)
                    Assert.Fail("Node did not return to idle state after a bad crypto response");
                if (!exceptionAsExpected)
                    Assert.Fail("Controller did not timeout when accessing a node with a bad crypto response");
            }
        }

        public abstract void verifyNodeRespondsCorrectlyToDoIdentify();
        protected void _verifyNodeRespondsCorrectlyToDoIdentify()
        {
            // Make a new node on a new network with a test name, and verify that that name can be read
            // correctly.
            const int virtualNodeID = 0x01;

            // Do this repeatedly with various node names.
            foreach (string testNodeName in new[] { "test node", "", "a", "01234567890abcdef01234567890abcde" })
            {
                using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
                {
                    virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, testNodeName);
                    startNetworkInNewThread(testVirtualNetwork);

                    // Connect to this network with a new driver class
                    transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(),
                                                                     false, null);

                    string recievedName = driver.doIdentify(virtualNodeID);
                    Thread.Sleep(1000);

                    Assert.AreEqual(testNode.name, recievedName, "Node identified itself with an incorrect name");

                    Assert.AreEqual(nodeState.idle, testNode.state, "Node did not return to idle state after doIdentify");
                }
            }
        }

        public abstract void verifyNodeIgnoresPacketsAddressedToOthers();
        protected void _verifyNodeIgnoresPacketsAddressedToOthers()
        {
            // Create a new virtual network and node. 
            // Send data to a different node on the network, which doesn't actually exist, and verify that the
            // first node does not process it.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Ping test node");
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                // We expect a commsTimeoutException from the controller, and an event onCryptoError on the node.
                bool exceptionAsExpected = false;
                try
                {
                    driver.doPing(virtualNodeID + 1);
                    Thread.Sleep(1000);
                }
                catch (commsTimeoutException)
                {
                    exceptionAsExpected = true;
                }

                if (!exceptionAsExpected)
                    Assert.Fail("Network did not timeout when a non-existent node was accessed");
                if (testNode.state != nodeState.idle)
                    Assert.Fail("Node did not remain in idle state while something else on the network was accessed");
            }
        }

        public abstract void verifyNodeReturnsCorrectSensorCount();
        protected void _verifyNodeReturnsCorrectSensorCount()
        {
            // Make a new node on a new network, add some sensors to it, and ensure that
            // the correct sensor count is returned.
            const int virtualNodeID = 0x01;

            // Do this a number of times with different amounts of sensors.
            foreach (int sensorsToAddCount in new[] { 0, 1, 10 })
            {
                using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
                {
                    // Make a list of sensors to add to our node
                    List<virtualNodeSensor> sensorsToAdd = new List<virtualNodeSensor>();
                    for (int i = 0; i < sensorsToAddCount; i++)
                        sensorsToAdd.Add(new genericDigitalOutSensor() { id = i });

                    // make our node
                    virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sensor count node", sensorsToAdd);
                    startNetworkInNewThread(testVirtualNetwork);

                    // Connect to this network with a new driver class
                    transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                    int recievedCount = driver.doGetSensorCount(virtualNodeID);
                    Thread.Sleep(1000);

                    Assert.AreEqual(sensorsToAddCount, recievedCount, "Node reported wrong sensor count");

                    Assert.AreEqual(nodeState.idle, testNode.state, "Node did not return to idle state after doIdentify");
                }
            }
        }

        public abstract void verifyNodeReturnsCorrectSensorTypes();
        protected void _verifyNodeReturnsCorrectSensorTypes()
        {
            // Make a new node on a new network, add some sensors to it, and ensure that the correct sensor
            // types are returned.
            const int virtualNodeID = 0x01;

            // Do this a number of times with different amounts of sensors.
            foreach (sensorTypeEnum typeToTest in Enum.GetValues(typeof(sensorTypeEnum)))
            {
                using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
                {
                    // Add a sensor of the given type to the node
                    List<virtualNodeSensor> sensorsToAdd = new List<virtualNodeSensor>();
                    sensorsToAdd.Add(virtualNodeSensor.makeSensor(typeToTest, 0x01));

                    // make our node
                    virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sensor type node", sensorsToAdd);
                    startNetworkInNewThread(testVirtualNetwork);

                    // Connect to this network with a new driver class
                    transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                    // Ask it for the type of the first sensor
                    sensorType recievedType = driver.doGetSensorType(virtualNodeID, 1);
                    Thread.Sleep(1000);

                    // Verify that the correct sensor type is recieved
                    Assert.AreEqual(typeToTest, recievedType.enumeratedType, "Node reported incorrect sensor type");

                    Assert.AreEqual(nodeState.idle, testNode.state, "Node did not return to idle state after doIdentify");
                }
            }
        }

        public abstract void verifyNodeReturnsCorrectSensorTypesForSecondSensor();
        protected void _verifyNodeReturnsCorrectSensorTypesForSecondSensor()
        {
            // Make a new node on a new network. Add a first sensor, then a second. verify that the second
            // is identified correctly. This should pick up situations where the indexing is broken.
            const int virtualNodeID = 0x01;

            // Do this a number of times with different amounts of sensors.
            foreach (sensorTypeEnum typeToTest in Enum.GetValues(typeof(sensorTypeEnum)))
            {
                using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
                {
                    // Add a dummy sensor, then a sensor of the given type to the node
                    List<virtualNodeSensor> sensorsToAdd = new List<virtualNodeSensor>();
                    sensorsToAdd.Add(virtualNodeSensor.makeSensor(sensorTypeEnum.generic_digital_in, 1));
                    sensorsToAdd.Add(virtualNodeSensor.makeSensor(typeToTest, 2));

                    // make our node
                    virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Sensor type node", sensorsToAdd);
                    startNetworkInNewThread(testVirtualNetwork);

                    // Connect to this network with a new driver class
                    transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                    // Ask it for the type of the first 'dummy' sensor, which is always generic_digital_in
                    sensorType recievedType = driver.doGetSensorType(virtualNodeID, 1);
                    Thread.Sleep(1000);
                    Assert.AreEqual(sensorTypeEnum.generic_digital_in, recievedType.enumeratedType, "Node reported incorrect sensor type");

                    // And now check the type of the second sensor.
                    recievedType = driver.doGetSensorType(virtualNodeID, 2);
                    Thread.Sleep(1000);
                    Assert.AreEqual(typeToTest, recievedType.enumeratedType, "Node reported incorrect sensor type");

                    Assert.AreEqual(nodeState.idle, testNode.state, "Node did not return to idle state after doIdentify");
                }
            }
        }

        public abstract void verifyNodeCanSetGenericDigitalOutCorrectly();
        protected void _verifyNodeCanSetGenericDigitalOutCorrectly()
        {
            // Create a new virtual network and node, with a single generic digital output sensor.
            // Verify that the sensor fires the correct events when asked to set the output value.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                List<virtualNodeSensor> sensorsToAdd = new List<virtualNodeSensor>();
                sensorsToAdd.Add(virtualNodeSensor.makeSensor(sensorTypeEnum.generic_digital_out, 1));

                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Digital output test node", sensorsToAdd);
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                // apply a callback to signal when the sensor changes state
                int sensorState = 0;
                testNode.onChangeSensor = new Action<virtualNodeBase, virtualNodeSensor, int>(
                    (node, sensor, newVal) =>
                        {
                            // Verify that the callback was called with the correct values
                            Assert.AreEqual(virtualNodeID, node.id, "node state callback provided wrong node");
                            Assert.AreEqual(1, sensor.id, "node state callback provided wrong sensor");

                            // Store the new state. Don't bother locking since it is a straight write and
                            // not a read-modify-write operation.
                            sensorState = newVal;
                        }
                    );

                // Now set the sensor, and observe the sensorState var get updated by the fired event.
                // Loop through a few different values, verifying after each.
                foreach (int stateToSetTo in new[] { 0, 1, 1, 0, 0, 1 })
                {
                    driver.doSetGenericOut(virtualNodeID, (short) stateToSetTo, 1);
                    Thread.Sleep(500);

                    Assert.AreEqual(stateToSetTo, sensorState, "Node did not set sensor output value to be correct");

                    if (testNode.state != nodeState.idle)
                        Assert.Fail("Node did not return to idle state after a bad crypto response");
                }

            }
        }

        public abstract void verifyNodeCanGetGenericDigitalInCorrectly();
        protected void _verifyNodeCanGetGenericDigitalInCorrectly()
        {
            // Create a new virtual network and node, with a single generic digital output sensor.
            // Verify that the sensor fires the correct events when asked to set the output value.
            const int virtualNodeID = 0x01;

            using (virtualNetworkBase testVirtualNetwork = virtualNetworkCreator.makeNew<networkTypeToTest>(pipeName))
            {
                List<virtualNodeSensor> sensorsToAdd = new List<virtualNodeSensor>();
                genericDigitalInSensor sensorIn = new genericDigitalInSensor() {id = 1};
                sensorsToAdd.Add(sensorIn);

                virtualNodeBase testNode = testVirtualNetwork.createNode(virtualNodeID, "Digital input test node", sensorsToAdd);
                startNetworkInNewThread(testVirtualNetwork);

                // Connect to this network with a new driver class
                transmitterDriver.transmitter driver = new transmitterDriver.transmitter(testVirtualNetwork.getDriverConnectionPointName(), false, null);

                // Check we can read the correct value...
                Assert.AreEqual(false, driver.doGetGenericDigitalIn(virtualNodeID, 1), "Node did not read a default 'false'");

                // Set to 1, and make sure we can read
                sensorIn.setValue(1);
                Assert.AreEqual(true, driver.doGetGenericDigitalIn(virtualNodeID, 1), "Node did not reflect a change to 'true'");

                // Aaand flip back again.
                sensorIn.setValue(0);
                Assert.AreEqual(false, driver.doGetGenericDigitalIn(virtualNodeID, 1), "Node did not reflect a change to 'false'");

            }
        }

    }
}
