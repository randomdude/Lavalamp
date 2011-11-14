using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;

namespace virtualNodeNetwork
{
    /// <summary>
    /// A virtual network implemented entirely in C#, used for verifying the driver code.
    /// </summary>
    public class CSharpNetwork : virtualNetworkBase
    {
        // This simulated network runs via a named pipe. This is its name and the pipe itself.
        private readonly string _pipename;

        /// <summary>
        /// The named pipe we use for comms
        /// </summary>
        private NamedPipeServerStream _pipe;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private readonly Dictionary<int, CSharpNode> nodes = new Dictionary<int, CSharpNode>();

        private bool disposing = false;

        public CSharpNetwork(string newPipeName)
        {
            _pipename = newPipeName;

            _pipe = new NamedPipeServerStream(_pipename, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
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

        public override void run()
        {
            log("Virtual network awaiting connection");
            _pipe.BeginWaitForConnection(handleConnection, null);
        }

        private void handleConnection(IAsyncResult ar)
        {
            _handleConnection(ar);
            try
            {
                _pipe.Disconnect();
                _pipe.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // swallow it.
            }
            if (!disposing)
            {
                _pipe = new NamedPipeServerStream(_pipename, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                _pipe.BeginWaitForConnection(handleConnection, null);
            }
        }

        private void _handleConnection(IAsyncResult ar)
        {
            log("Client connected.");

            lock (_pipe)
            {
                _pipe.EndWaitForConnection(ar);
            }

            // Loop around, taking data sent to the network and broadcasting it to the nodes.
            // Return only on object distruction.
            while (true)
            {
                log("Awaiting data bytes.");

                if (!_pipe.IsConnected)
                    return;

                // Get a single byte and propogate it to all nodes
                byte[] recievedBytes = new byte[1];
                int bytesReadThisPacket = 0;
                if (!_pipe.IsConnected)
                    return;
                int bytesReadThisRead = 0;
                try
                {
                    bytesReadThisRead = _pipe.Read(recievedBytes, bytesReadThisPacket, 1);
                }
                catch (ObjectDisposedException)
                {
                    // I hate needing to catch this exception when the pipe is disposed during
                    // a blocking read, but I think it's the only way.
                    return;
                }

                // If no data was waiting, wait and retry.
                if (bytesReadThisRead == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                log("Byte received.");

                lock (nodes)
                {
                    // Broadcast byte to all nodes.
                    foreach (KeyValuePair<int, CSharpNode> idAndNode in nodes)
                    {
                        idAndNode.Value.processByte(recievedBytes[0]);
                    }
                }
            }
        }

        public override virtualNodeBase createNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors)
        {
            CSharpNode newNode = new CSharpNode(newId, newName, newSensors);
            addEventsToNode(newNode);

            return newNode;
        }

        public override virtualNodeBase createNode(int newId, string newName)
        {
            CSharpNode newNode = new CSharpNode(newId, newName);
            addEventsToNode(newNode);

            return newNode;
        }

        private void addEventsToNode(CSharpNode newNode)
        {
            newNode.onLog += log;
            newNode.onSendPacket += sendPacket;
            newNode.onStateChange += nodeStateChange;
            newNode.onCryptoError += cryptoError;
            nodes.Add(newNode.id, newNode);
        }

        public override void Dispose()
        {
            if (_pipe.IsConnected)
                _pipe.Disconnect(); // racey!
            _pipe.Close();
            disposing = true;
        }
    }
}