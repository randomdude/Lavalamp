using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    public class virtualNetwork
    {
        private string pipename;
        private NamedPipeServerStream pipe;

        /// <summary>
        /// Lock on this object before accessing consecutiveSyncSymbols
        /// </summary>
        private object syncSymbolLock = new object();

        /// <summary>
        /// how many synch symbols we have seen since we saw something else
        /// </summary>
        private int consecutiveSyncSymbols = 0;

        /// <summary>
        /// The synchronization symbol which can be repeated in order to reset to the start of a packet.
        /// </summary>
        private const byte synchSymbol = 0xAA;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private Dictionary<int, virtualNode> nodes = new Dictionary<int, virtualNode>();

        public static int controllerID = 0;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global
        public Action<string> onLogString;
        public Action onSyncPacket;
        public Action onPacketSend;
        public Action onCryptoError;
        private IAsyncResult ar;
// ReSharper restore UnassignedField.Global
// ReSharper restore MemberCanBePrivate.Global

        public virtualNetwork(string newPipeName)
        {
            pipename = newPipeName;

            pipe = new NamedPipeServerStream(pipename, PipeDirection.InOut, 10, PipeTransmissionMode.Byte,
                                            PipeOptions.Asynchronous);
        }

        private void syncPacket()
        {
            log(logLevel.info, "Sync packet detected.");

            if (onSyncPacket != null)
                onSyncPacket.Invoke();
        }

        private void cryptoError(virtualNode sender)
        {
            log(logLevel.warn, "Crypto error detected.");

            if (onCryptoError != null)
                onCryptoError.Invoke();
        }

        private void sendPacket(networkPacket toSend)
        {
            log(logLevel.info, "Sending packet: " + toSend.ToString());

            if (onPacketSend != null)
                onPacketSend.Invoke();

            lock (pipe)
            {
                toSend.writeToPipe(pipe);
            }
        }

        private void nodeStateChange(virtualNode sender, nodeState newState)
        {
            log(logLevel.info, "Node state change on id " + sender.id + " to " + newState );
        }

        private void log(string toLog)
        {
            log(logLevel.info, toLog);
        }

        private void log(logLevel level, string toLog)
        {
            Debug.WriteLine(toLog);
            if (onLogString == null)
                return;

            onLogString.Invoke(Enum.GetName(typeof (logLevel), level).ToString() + ": " + toLog);
        }

        public void run()
        {
            log("Virtual network awaiting connection");
            ar = pipe.BeginWaitForConnection(handleConnection, null);
        }

        public void handleConnection(IAsyncResult foo)
        {
            //try
            {
                handleConnectionUnsafe(foo);
            }
            //catch (Exception)
            //{
                
            //}
        }

        public void handleConnectionUnsafe(IAsyncResult foo)
        {
            log("Client connected.");

            lock (pipe)
            {
                pipe.EndWaitForConnection(foo);
            }

            while (true)
            {
                log("Awaiting commands.");

                if (!pipe.IsConnected)
                    return;

                // Get an entire packet's worth of bytes. If we get a synchronization token, then we should keep
                // track of that, and if we receive 8 consecutive ones, we should discard any packet received so
                // far.
                byte[] rawPacketBytes = new byte[networkPacket.lengthInBytes];
                int bytesReadThisPacket = 0;
                while (bytesReadThisPacket < networkPacket.lengthInBytes)
                {
                    // We read one byte at a time to make sure we don't accidentally read past the end of a sync
                    // packet, which occur at any point. For example, with a sync token of 0xAA:
                    // Packet 1: 00 AA AA AA AA AA AA AA
                    // Packet 2: AA .. 
                    // We will abort reading before we read the second packet of packet 2.
                    int bytesReadThisRead = 0;
                    try
                    {
                        bytesReadThisRead = pipe.Read(rawPacketBytes, bytesReadThisPacket, 1);
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }

                    // If no data was waiting, wait and retry.
                    if (bytesReadThisRead == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    // We read a byte. Keep count of it, and if it was a sync symbol.
                    if (rawPacketBytes[bytesReadThisPacket] == synchSymbol)
                    {
                        lock (syncSymbolLock)
                        {
                            consecutiveSyncSymbols++;

                            if (consecutiveSyncSymbols == networkPacket.lengthInBytes)
                            {
                                // An entire packet of sync symbols! Discard this read.
                                bytesReadThisPacket = 0;
                                syncPacket();
                                consecutiveSyncSymbols = 0;
                            }
                            else
                            {
                                bytesReadThisPacket++;
                            }
                        }
                    }
                    else
                    {
                        bytesReadThisPacket++;
                    }
                }

                log("Command received.");

                networkPacket packet = new networkPacket(rawPacketBytes);

                log("Raw packet is " + packet.ToString());

                lock (nodes)
                {
                    if (!nodes.ContainsKey(packet.destinationNodeID))
                    {
                        // This is a packet addressed to another station. We should ignore it.
                        log(logLevel.warn, "Packet addressed to unknown node " + packet.destinationNodeID);
                    }
                    else
                    {
                        // Send the packet to the appropriate node to be processed.
                        nodes[packet.destinationNodeID].processPacket(packet);
                    }
                }
            }
        }

        public void AddNode(virtualNode newNode)
        {
            newNode.onLog += log;
            newNode.onSendPacket += sendPacket;
            newNode.onStateChange += nodeStateChange;
            newNode.onCryptoError += cryptoError;
            nodes.Add(newNode.id, newNode);
        }

        public void plzdie()
        {
            pipe.Close();
        }
    }

    public enum logLevel
    {
        info, warn
    }
}