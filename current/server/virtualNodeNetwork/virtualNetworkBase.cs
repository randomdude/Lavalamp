using System;
using System.Diagnostics;

namespace virtualNodeNetwork
{
    public abstract class virtualNetworkBase : IDisposable
    {
        public abstract void run();
        public abstract virtualNodeBase createNode(int newId, string newName);

        /// <summary>
        /// Get the port we can pass to our transmitterDriver
        /// </summary>
        /// <returns></returns>
        public abstract string getDriverConnectionPointName();

        public abstract void Dispose();

        /// <summary>
        /// The ID byte of the PC-side network controller
        /// </summary>
        public static int controllerID = 0;

#region events
        /// <summary>
        /// Fired when a string is logged
        /// </summary>
        public Action<string> onLogString;

        /// <summary>
        /// Fired when a sync frame is detected
        /// </summary>
        public Action onSyncPacket;

        /// <summary>
        /// Fired when this controller sends a packet to a node
        /// </summary>
        public Action onSendPacket;

        /// <summary>
        /// Fired when a challenge/response is invalid
        /// </summary>
        public Action onCryptoError;

        /// <summary>
        /// Log a string at the default log level
        /// </summary>
        /// <param name="toLog"></param>
        protected void log(string toLog)
        {
            log(logLevel.info, toLog);
        }

        /// <summary>
        /// Log a string with the given log level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="toLog"></param>
        protected void log(logLevel level, string toLog)
        {
            Debug.WriteLine(toLog);
            if (onLogString == null)
                return;

            onLogString.Invoke(Enum.GetName(typeof(logLevel), level).ToString() + ": " + toLog);
        }

        /// <summary>
        /// Fire the onSyncPacket event
        /// </summary>
        protected void syncPacket()
        {
            log(logLevel.info, "Sync packet detected.");

            if (onSyncPacket != null)
                onSyncPacket.Invoke();
        }

        /// <summary>
        /// Fire the onCryptoError event
        /// </summary>
        /// <param name="sender"></param>
        protected void cryptoError(virtualNodeBase virtualNodeBase)
        {
            log(logLevel.warn, "Crypto error detected.");

            if (onCryptoError != null)
                onCryptoError.Invoke();
        }

        /// <summary>
        /// Fire the onSendPacket event
        /// </summary>
        /// <param name="toSend"></param>
        protected void sendPacket(networkPacket toSend)
        {
            log(logLevel.info, "Controller sending packet: " + toSend.ToString());

            if (onSendPacket != null)
                onSendPacket.Invoke();
        }
#endregion
    }

    public enum logLevel
    {
        info, warn
    }

}