using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;

namespace virtualNodeNetwork
{
    public class virtualNetwork : virtualNetworkBase
    {
        // This simulated network runs via a named pipe. This is its name and the pipe itself.
        private readonly string _pipename;

        /// <summary>
        /// The named pipe we use for comms
        /// </summary>
        private readonly NamedPipeServerStream _pipe;

        /// <summary>
        /// Lock on this object before accessing consecutiveSyncSymbols
        /// </summary>
        private readonly object _syncSymbolLock = new object();

        /// <summary>
        /// how many synch symbols we have seen since we saw something else
        /// </summary>
        private int _consecutiveSyncSymbols = 0;

        /// <summary>
        /// The synchronization symbol which can be repeated in order to reset to the start of a packet.
        /// </summary>
        private const byte _synchSymbol = 0xAA;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private readonly Dictionary<int, virtualNode> nodes = new Dictionary<int, virtualNode>();

        public virtualNetwork(string newPipeName)
        {
            _pipename = newPipeName;

            _pipe = new NamedPipeServerStream(_pipename, PipeDirection.InOut, 10, PipeTransmissionMode.Byte,
                                            PipeOptions.Asynchronous);
        }

        public override void run()
        {
            log("Virtual network awaiting connection");
            _pipe.BeginWaitForConnection(handleConnection, null);
        }

        /// <summary>
        /// Called when a node on this network changes state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        private void nodeStateChange(virtualNodeBase sender, nodeState newState)
        {
            log(logLevel.info, "Node state change on id " + sender.id + " to " + newState);
        }

        /// <summary>
        /// Called when we need to send a networkPacket to a child node.
        /// </summary>
        /// <param name="toSend"></param>
        private new void sendPacket(networkPacket toSend)
        {
            base.sendPacket(toSend);

            lock (_pipe)
            {
                toSend.writeToPipe(_pipe);
            }
        }

        public override string getDriverConnectionPointName()
        {
            return "pipe\\" + _pipename;
        }

        private void handleConnection(IAsyncResult ar)
        {
            log("Client connected.");

            lock (_pipe)
            {
                _pipe.EndWaitForConnection(ar);
            }

            while (true)
            {
                log("Awaiting commands.");

                if (!_pipe.IsConnected)
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
                        bytesReadThisRead = _pipe.Read(rawPacketBytes, bytesReadThisPacket, 1);
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
                    if (rawPacketBytes[bytesReadThisPacket] == _synchSymbol)
                    {
                        lock (_syncSymbolLock)
                        {
                            _consecutiveSyncSymbols++;

                            if (_consecutiveSyncSymbols == networkPacket.lengthInBytes)
                            {
                                // An entire packet of sync symbols! Discard this read.
                                bytesReadThisPacket = 0;
                                syncPacket();
                                _consecutiveSyncSymbols = 0;
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

        public override virtualNodeBase createNode(int newId, string newName)
        {
            virtualNode newNode = new virtualNode(newId, newName);

            newNode.onLog += log;
            newNode.onSendPacket += sendPacket;
            newNode.onStateChange += nodeStateChange;
            newNode.onCryptoError += cryptoError;
            nodes.Add(newNode.id, newNode);

            return newNode;
        }

        public override void Dispose()
        {
            _pipe.Close();
        }
    }
}