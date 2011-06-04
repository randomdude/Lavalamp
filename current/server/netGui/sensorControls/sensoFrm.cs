using System;
using System.Drawing;
using System.Windows.Forms;

namespace netGui.sensorControls
{
    public partial class sensorFrm : Form
    {
        public sensorFrm()
        {
            InitializeComponent();
        }

        public sensorFrm(Node target, short newSensorIndex)
        {
            InitializeComponent();

            this.Controls.Remove(this.ctlSensor1);
            this.ctlSensor1 = new ctlSensor(target, newSensorIndex);
            ctlSensor1.onSetIcon += setIcon;
            this.Controls.Add(this.ctlSensor1);

            this.Text = this.ctlSensor1.lblTitle.Text;
        }

        public event Action<Icon> OnSetIcon;
        private void setIcon(Icon newIcon)
        {
            OnSetIcon(newIcon);
        }

        private void sensorFrm_Resize(object sender, EventArgs e)
        {
            ctlSensor1.Size = this.ClientSize;
            ctlSensor1.adjustControls();
        }

        public void minimiseToTray()
        {
            if (null != this.ParentForm && null != this.MdiParent)
            {
                // otherwise we minimize the parent
                ((frmNodeSensors)this.ParentForm).minimiseToTray();
            }
            else
            {
                // if we're not docked as MDI, we just minimize this sensor
                this.notifyIcon1.ContextMenuStrip = this.ctlSensor1.contextMenuStrip1;
                this.notifyIcon1.Icon = this.Icon;
                this.notifyIcon1.Text = this.ctlSensor1.lblTitle.Text;
                this.notifyIcon1.Visible = true;
                this.Hide();
            } 
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.notifyIcon1.Visible = false;
        }

        public void setAlwaysOnTopTickState(bool val)
        {
            this.ctlSensor1.alwaysOnTopToolStripMenuItem.Checked = val;
        }
    }
}
