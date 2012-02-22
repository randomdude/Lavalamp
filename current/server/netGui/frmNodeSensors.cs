using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using netGui.sensorControls;
using transmitterDriver;

namespace netGui
{
    public partial class frmNodeSensors : Form
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMenu(int hwnd, int bRevert);

        [DllImport("user32.dll")]
        private static extern int AppendMenu(int hMenu, int Flagsw, int IDNewItem, string lpNewItem);

        private Node target;
        private List<sensorFrm> childForms;

        public frmNodeSensors()
        {
            InitializeComponent();
        }

        public void loadNode(Node newtarget)
        {
            childForms = new List<sensorFrm>();
            target = newtarget;

            int n = 0;
            int formHeight = this.Height;
            int formWidth = this.Width;
            foreach (sensor thisSensor in target.sensors.Values)
            {
                sensorFrm newSensorForm;
                try
                {
                    newSensorForm = new sensorFrm(thisSensor);

                    newSensorForm.Visible = false;
                    newSensorForm.Show();
                    newSensorForm.MdiParent = this;

                    newSensorForm.Left = (newSensorForm.Width * n);
                    newSensorForm.Top = 0;
                    formWidth = newSensorForm.Width + newSensorForm.Left + SystemInformation.Border3DSize.Width;
                    formHeight = newSensorForm.Height + SystemInformation.Border3DSize.Height;

                    newSensorForm.Visible = true;
                    newSensorForm.OnSetIcon += setIcon;
                    childForms.Add(newSensorForm);
                }
                catch (commsException)
                {
                    MessageBox.Show("An exception occurred interrogating the node. Please retry.");
                }
                catch (Exception)
                {
                    MessageBox.Show("A serious, unhandled exception occurred interrogating the node. This is bad.");
                }

                n++;
            }
            int borderX = (this.Width - this.ClientRectangle.Width) + SystemInformation.Border3DSize.Height;
            int borderY = (this.Height - this.ClientRectangle.Height) + SystemInformation.Border3DSize.Width;
            this.Width = formWidth + borderX ;
            this.Height = formHeight + borderY ;
        }

        private void frmNodeSensors_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            UserInit();
        }

        #region controlBox menu code

        // For the howto on adding to the controlBox that I used, see 
        //  http://www.quantumsoftware.com.au/Support/KB/Article.aspx?ID=76
        
        public void UserInit()
        {
            // Add some menus to our controlbox
            int sysMenu = GetSystemMenu(this.Handle.ToInt32(), 0 );

            if (0==sysMenu || -1 == sysMenu)
            {
                MessageBox.Show("Unable to get system menu handle");
            } else {
                AppendMenu(sysMenu, 0xA00, 0, null);
                AppendMenu(sysMenu, 0x000, 0x100, "Minimize to &tray");
                AppendMenu(sysMenu, 0x000, 0x101, "&Always on top");
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x112) // WM_SYSCOMMAND
                WmSysCommand(m.WParam.ToInt32());
        }

        private void WmSysCommand(int wParam)
        {
            switch (wParam)
            {
                case 0x100: // minimize to tray
                    minimizeToTray_Click(this, new EventArgs());
                    break;
                case 0x101: // Always on top
                    alwaysOnTopToolStripMenuItem_Click(this, new EventArgs());
                    break;
            }

        }

        #endregion
        
        private void minimizeToTray_Click(object sender, System.EventArgs e)
        {
            minimiseToTray();
        }

        public void minimiseToTray()
        {
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Text = this.Text;
            this.notifyIcon1.Visible = true;
            this.Hide();
        }

        private void setIcon(Icon setToThis)
        {
            this.notifyIcon1.Icon = setToThis;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.notifyIcon1.Visible = false;
        }

        private void minimiseToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimiseToTray();
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alwaysOnTopToolStripMenuItem.Checked = !alwaysOnTopToolStripMenuItem.Checked;
            setAlwaysOnTop(alwaysOnTopToolStripMenuItem.Checked);
        }

        public void setAlwaysOnTop(bool newVal)
        {
            alwaysOnTopToolStripMenuItem.Checked = newVal;

            foreach (sensorFrm childForm in childForms)
            {
                childForm.setAlwaysOnTopTickState(newVal);
            }
            this.TopMost = newVal;
        }
    }
}
