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
        private ctlNodeSensorWidget sensorVis = null;
        private virtualNodeSensor parentSensor = null;

        public ctlNodeSensor()
        {
            InitializeComponent();
        }

        public void loadSensor(virtualNodeSensor sensor)
        {
            parentSensor = sensor;
            lblSensorID.Text = sensor.id.ToString();
            lblSensorType.Text = sensor.type.ToString();

            switch (sensor.type)
            {
                case sensorTypeEnum.generic_digital_out:
                    sensorVis = new ctlNodeSensorGenericDigitalOut();
                    break;
                case sensorTypeEnum.generic_digital_in:
                    sensorVis = new ctlNodeSensorGenericDigitalIn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sensorVis.onInputChanged += inputChanged;

            sensorVis.Top = lblSensorType.Top + lblSensorType.Height;
            Controls.Add(sensorVis);
        }

        public void updateValue(int newValue)
        {
            sensorVis.updateValue(newValue);
        }

        private void inputChanged(int newValue)
        {
            parentSensor.setValue(newValue);
        }
    }
}
