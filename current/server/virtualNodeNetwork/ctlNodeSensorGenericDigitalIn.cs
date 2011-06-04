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
    public partial class ctlNodeSensorGenericDigitalIn : ctlNodeSensorWidget
    {
        public ctlNodeSensorGenericDigitalIn()
        {
            InitializeComponent();
            this.chkState.CheckedChanged += chkStateChanged;
        }

        /// <summary>
        /// This sensor does not support writes.
        /// </summary>
        /// <param name="newValue"></param>
        public override void updateValue(int newValue)
        {
            throw new NotSupportedException();
        }

        private void chkStateChanged(object sender, EventArgs e)
        {
            inputChanged(chkState.Checked ? 1 : 0);
        }

    }
}
