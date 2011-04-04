using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using netGui;

namespace virtualNodeNetwork
{
    public partial class ctlNodeSensor : UserControl
    {
        private ctlNodeSensorGenericDigitalOut sensorVis = null;

        public ctlNodeSensor()
        {
            InitializeComponent();
        }

        public void loadSensor(virtualNodeSensor sensor)
        {
            lblSensorID.Text = sensor.id.ToString();
            lblSensorType.Text = sensor.type.ToString();

            switch (sensor.type)
            {
                case sensorTypeEnum.generic_digital_out:
                    sensorVis = new ctlNodeSensorGenericDigitalOut();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sensorVis.Top = lblSensorType.Top + lblSensorType.Height;
            Controls.Add(sensorVis);
        }

        public void updateValue(int newValue)
        {
            sensorVis.updateValue(newValue);
        }
    }
}
