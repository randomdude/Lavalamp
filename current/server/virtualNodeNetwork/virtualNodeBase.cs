using System;
using System.Collections.Generic;

namespace virtualNodeNetwork
{
    public abstract class virtualNodeBase
    {
        public int id;
        public string name;

        #region events
        /// <summary>
        /// Fired when an event is logged
        /// </summary>
        public Action<logLevel, string> onLog;

        /// <summary>
        /// Fired when the node sends a packet
        /// </summary>
        public Action<networkPacket> onSendPacket;

        /// <summary>
        /// Fired when the node 'state' changes
        /// </summary>
        public Action<virtualNodeBase, nodeState> onStateChange;

        /// <summary>
        /// Fired when a challenge response is incorrect
        /// </summary>
        public Action<virtualNodeBase> onCryptoError;

        /// <summary>
        /// Fired when a sensor's output value is changed on this node
        /// </summary>
        public Action<virtualNodeBase, virtualNodeSensor, int> onChangeSensor;

        /// <summary>
        /// Fired when a sync packet is detected
        /// </summary>
        public Action<virtualNodeBase> onSyncPacket;
        
        #endregion

        public nodeState state;
        protected int p = 0x112233;
        public Dictionary<int, virtualNodeSensor> sensors = new Dictionary<int, virtualNodeSensor>();

        public virtualNodeBase(int newId, string newName)
        {
            id = newId;
            name = newName;
        }

        public virtualNodeBase(int newId, string newName, virtualNodeSensor newSensor)
        {
            id = newId;
            name = newName;

            sensors.Add(newSensor.id, newSensor);
        }

        public virtualNodeBase(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors)
        {
            id = newId;
            name = newName;

            foreach (virtualNodeSensor sensorToAdd in newSensors)
                sensors.Add(sensorToAdd.id, sensorToAdd);
        }

        /// <summary>
        /// Log an event at the default log level
        /// </summary>
        /// <param name="toLog"></param>
        protected void log(string toLog)
        {
            log(logLevel.info, toLog );
        }

        /// <summary>
        /// Log an event at the given log level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="toLog"></param>
        protected void log(logLevel level, string toLog)
        {
            if (onLog != null)
                onLog.Invoke(level, toLog);
        }

        /// <summary>
        /// Fire the onSendPacket event
        /// </summary>
        /// <param name="toSend"></param>
        protected void sendPacket(networkPacket toSend)
        {
            if (onSendPacket != null)
                onSendPacket(toSend);
        }

        /// <summary>
        /// Fire the onStateChange event
        /// </summary>
        /// <param name="newState"></param>
        protected void stateChange(nodeState newState)
        {
            state = newState;
            if (onStateChange != null)
                onStateChange.Invoke(this, newState);

           
        }

        /// <summary>
        /// Fire the onSyncPacket event
        /// </summary>
        protected void syncPacket()
        {
            if (onSyncPacket != null)
                onSyncPacket.Invoke(this);
        }

        /// <summary>
        /// Fire the onChangeSensor event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newVal"></param>
        protected void changeSensor(virtualNodeSensor sender, int newVal)
        {
            if (onChangeSensor != null)
                onChangeSensor.Invoke(this, sender, newVal);
        }

        /// <summary>
        /// Fire the onCryptoError event
        /// </summary>
        protected void cryptoError()
        {
            if (onCryptoError != null)
                onCryptoError.Invoke(this);
        }
    }

    public enum nodeState
    {
        idle,                       // The node is idle.
        firstHandshakeInProgress    // The node has sent its response to the first challenge packet, and is waiting for the controller to reply to it with a command.
    }

    public enum commandByte
    {
        unknown,
        ping = 0x01,
        identify = 0x02,
        getSensor = 0x03,
        setSensor = 0x04,
        getSensorType = 0x05,
    }
}