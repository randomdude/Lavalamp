using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    using System.ComponentModel;

    /// <summary>
    /// A network of PIC-level simulated nodes. We use a third-party simulator to simulate operation of the
    /// PIC chip which runs this network.
    /// </summary>
    public class simulatedPICNetwork : virtualNetworkBase
    {
        private readonly string _pipename;
        private readonly NamedPipeServerStream _pipe;
        private readonly ISynchronizeInvoke _eventHandler;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private readonly Dictionary<int, simulatedPICNode> _nodes = new Dictionary<int, simulatedPICNode>();

        public simulatedPICNetwork(string newPipeName, ISynchronizeInvoke eInvoke)
        {
            _pipename = newPipeName;

            _eventHandler = eInvoke;
            _pipe = new NamedPipeServerStream(_pipename, PipeDirection.InOut, 10, PipeTransmissionMode.Byte,
                                              PipeOptions.Asynchronous);

        }

        public override void run()
        {
            log("Virtual network awaiting connection");
            _pipe.BeginWaitForConnection(handleConnection, null);
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

                // Pass every byte straight to all nodes.
                while (true)
                {
                    byte[] byteRead = new byte[1];
                    int bytesReadThisRead = 0;
                    try
                    {
                        bytesReadThisRead = _pipe.Read(byteRead, 0, 1);
                    }
                    catch (ObjectDisposedException)
                    {
                        // The pipe has been closed, so finish listening for any connection.
                        return;
                    }

                    // If no data was waiting, wait and retry.
                    if (bytesReadThisRead == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    log("Byte received.");

                    lock (_nodes)
                    {
                        foreach (simulatedPICNode destNod in _nodes.Values)
                            destNod.processByte(byteRead);
                    }
                }
            }
        }

        public override string getDriverConnectionPointName()
        {
            return "pipe\\" + _pipename;
        }

        public override virtualNodeBase createNode(int newId, string newName, IEnumerable<virtualNodeSensor> newSensors)
        {
            simulatedPICNode newNode = new simulatedPICNode(newId, newName, newSensors, _eventHandler, Properties.Settings.Default.lavalampPICObject);
            addEvents(newNode);
            return newNode;
        }

        public override virtualNodeBase createNode(int newId, string newName)
        {
            simulatedPICNode newNode = new simulatedPICNode(newId, newName, _eventHandler, Properties.Settings.Default.lavalampPICObject);
            addEvents(newNode);
            return newNode;
        }

        private void addEvents(simulatedPICNode newNode)
        {
            newNode.onLog += log;
            newNode.onSendPacket += sendPacket;
            newNode.onCryptoError += cryptoError;
            _nodes.Add(newNode.id, newNode);
        }

        public override void Dispose()
        {
            _pipe.Close();
        }
    }
}