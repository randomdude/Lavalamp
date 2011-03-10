using System;

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
        #endregion

        public nodeState state;
        protected int p = 0x112233;

        public virtualNodeBase(int newId, string newName)
        {
            id = newId;
            name = newName;
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
            if (onStateChange != null)
                onStateChange.Invoke(this, newState);

            state = newState;
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
        ping
    }
}