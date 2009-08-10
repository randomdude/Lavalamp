using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace netGui.sensorControls
{
    public partial class ctlOnOff : graph
    {
        public ctlOnOff()
        {
            InitializeComponent();
        }

        public override void SetError(string errorString)
        {
            MessageBox.Show(errorString);
        }

        private void ctlOnOff_Resize(object sender, EventArgs e)
        {
            const int borderSize = 5;

            this.cmdOn.Visible = false;
            this.cmdOff.Visible = false;

            this.cmdOn.Width = this.ClientSize.Width - (borderSize * 2);
            this.cmdOn.Left = (this.ClientSize.Width / 2) - (this.cmdOn.Width / 2);
            this.cmdOn.Top = borderSize;
            this.cmdOn.Height = (this.ClientSize.Height /2 ) - borderSize ;

            this.cmdOff.Width = this.ClientSize.Width - (borderSize * 2);
            this.cmdOff.Left = (this.ClientSize.Width / 2) - (this.cmdOff.Width / 2);
            this.cmdOff.Top = ( this.ClientSize.Height / 2) + borderSize ;
            this.cmdOff.Height = (this.ClientSize.Height / 2) - borderSize;

            this.cmdOn.Visible = true;
            this.cmdOff.Visible = true;
        }

        private void cmdOn_Click(object sender, EventArgs e)
        {
            this.sendValueToNodeDelegate(true);
        }

        private void cmdOff_Click(object sender, EventArgs e)
        {
            this.sendValueToNodeDelegate(false);
        }
    }
}
