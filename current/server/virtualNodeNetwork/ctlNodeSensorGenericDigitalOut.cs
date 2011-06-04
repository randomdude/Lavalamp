using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

namespace virtualNodeNetwork
{
    public partial class ctlNodeSensorGenericDigitalOut : ctlNodeSensorWidget
    {
        public ctlNodeSensorGenericDigitalOut()
        {
            InitializeComponent();

            _updateValue(0);
        }

        public override void updateValue(int newValue)
        {
            this.Invoke(new Action(() => _updateValue(newValue)));
        }

        private void _updateValue(int newValue)
        {
            if (newValue == 0)
            {
                lblValue.Text = "OFF";
                lblValue.BackColor = Color.LightPink;
            }
            else if (newValue == 1)
            {
                lblValue.Text = "ON";
                lblValue.BackColor = Color.LightGreen;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
