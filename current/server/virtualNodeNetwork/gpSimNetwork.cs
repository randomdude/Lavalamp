using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;

namespace virtualNodeNetwork
{
    public class gpSimNetwork : virtualNetworkBase
    {
        private readonly string _pipename;
        private readonly NamedPipeServerStream _pipe;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private readonly Dictionary<int, gpSimNode> nodes = new Dictionary<int, gpSimNode>();

        public gpSimNetwork(string newPipeName)
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
                        foreach (gpSimNode destNod in nodes.Values)
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
            gpSimNode newNode = new gpSimNode(newId, newName, newSensors);
            addEvents(newNode);
            return newNode;
        }

        public override virtualNodeBase createNode(int newId, string newName)
        {
            gpSimNode newNode = new gpSimNode(newId, newName);
            addEvents(newNode);
            return newNode;
        }

        private void addEvents(gpSimNode newNode)
        {
            newNode.onLog += log;
            newNode.onSendPacket += sendPacket;
            newNode.onCryptoError += cryptoError;
            nodes.Add(newNode.id, newNode);

        }

        public override void Dispose()
        {
            _pipe.Close();
        }
    }
}