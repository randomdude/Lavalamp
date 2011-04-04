using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    public partial class ctlVirtualNode : UserControl
    {
        private ISynchronizeInvoke mainForm;
        private virtualNodeBase ourNode;
        private Dictionary<int, ctlNodeSensor> sensorCtlsByID = new Dictionary<int, ctlNodeSensor>();

        public ctlVirtualNode()
        {
            InitializeComponent();
            mainForm = this;
        }

        public void loadNode(virtualNodeBase newNode)
        {
            ourNode = newNode;

            lblNodeName.Text = ourNode.name;
            lblNodeID.Text = ourNode.id.ToString();
            lblNodeState.Text = ourNode.state.ToString();

            foreach (virtualNodeSensor sensor in ourNode.sensors.Values)
            {
                ctlNodeSensor newSensorCtl = new ctlNodeSensor();

                newSensorCtl.loadSensor(sensor);

                newSensorCtl.Left = grpNodeSensors.DisplayRectangle.Left;
                newSensorCtl.Top = grpNodeSensors.DisplayRectangle.Top;
                newSensorCtl.Top += (sensor.id - 1) * newSensorCtl.Height;

                sensorCtlsByID.Add(sensor.id, newSensorCtl);

                grpNodeSensors.Controls.Add(newSensorCtl);
            }

            ourNode.onChangeSensor += onChangeSensor;
            ourNode.onStateChange += onNodeStateChange;
        }

        private void onChangeSensor(virtualNodeBase senderNode, virtualNodeSensor changingSensor, int newValue)
        {
            if (senderNode.id != ourNode.id)
                return;

            sensorCtlsByID[changingSensor.id].updateValue(newValue);
        }

        private void onNodeStateChange(virtualNodeBase sender, nodeState newState)
        {
            if (sender != ourNode)
                return;

            mainForm.Invoke(new Action( () => lblNodeState.Text = newState.ToString() ) , null);
        }
    }
}
