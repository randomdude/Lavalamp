using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    /// <summary>
    /// A network of PIC-level simulated nodes. We use a third-party simulator to simulate operation of the
    /// PIC chip which runs this network.
    /// </summary>
    public class simulatedPICNetwork : virtualNetworkBase
    {
        private readonly string _pipename;
        private readonly NamedPipeServerStream _pipe;
        private Form frmEventForm;

        /// <summary>
        /// Nodes indexed by ID.
        /// </summary>
        private readonly Dictionary<int, simulatedPICNode> nodes = new Dictionary<int, simulatedPICNode>();

        public simulatedPICNetwork(string newPipeName)
        {
            _pipename = newPipeName;

            _pipe = new NamedPipeServerStream(_pipename, PipeDirection.InOut, 10, PipeTransmissionMode.Byte,
                                              PipeOptions.Asynchronous);

            frmEventForm = new Form();
            Thread foo = new Thread(eventFormThread);
            foo.Name = "PIC network form";
            foo.Start();

            while (!frmEventForm.IsHandleCreated)
            {
                Thread.Sleep(100);
            }
        }

// ReSharper disable MemberCanBeMadeStatic.Local
        private void eventFormThread(object foo)
// ReSharper restore MemberCanBeMadeStatic.Local
        {
            Application.Run(frmEventForm);
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

                    lock (nodes)
                    {
                        foreach (simulatedPICNode destNod in nodes.Values)
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
            simulatedPICNode newNode = new simulatedPICNode(newId, newName, newSensors);
            addEvents(newNode);
            return newNode;
        }

        public override virtualNodeBase createNode(int newId, string newName)
        {
            simulatedPICNode newNode = new simulatedPICNode(newId, newName, frmEventForm, Properties.Settings.Default.lavalampPICObject);
            addEvents(newNode);
            return newNode;
        }

        private void addEvents(simulatedPICNode newNode)
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