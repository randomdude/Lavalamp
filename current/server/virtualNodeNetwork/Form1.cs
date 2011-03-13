using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    public partial class Form1 : Form
    {
        private Thread networkThread = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(networkThreadStart);
            networkThread = new Thread(ts);
            networkThread.Name = "Network thread";
            networkThread.Start();
        }

        private void networkThreadStart(object obj)
        {
            virtualNetworkBase net = new virtualNetwork("vnet");

            net.onLogString = appendLog;

            //virtualNodeBase testNode = net.createNode(1, "virtual node");
            virtualNodeBase testNode = net.createNode(1, "01234567890abcdef01234567890abcde");

            net.run();
        }

        private void appendLog(string newLog)
        {
            if(!this.InvokeRequired)
            {
                txtLog.AppendText(newLog + Environment.NewLine);
                return;
            }
            this.Invoke( new Action<string>(appendLog), newLog);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // todo: proper thread control
            networkThread.Abort();
        }

    }
}
