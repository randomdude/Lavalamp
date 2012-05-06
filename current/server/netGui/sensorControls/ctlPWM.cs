using System;
using System.Windows.Forms;

namespace netGui.sensorControls
{
    using ruleEngine.nodes;

    public partial class ctlPWM : graph 
    {
        public ctlPWM()
        {
            InitializeComponent();
        }

        public void adjustControls()
        {
            const int borderSize = 5;

            this.trackBarBrightness.Visible = false;
            this.trackBarSpeed.Visible = false;
            this.lblBrightness.Visible = false;
            this.lblFadeSpeed.Visible  = false;

            this.trackBarBrightness.Width = this.ClientSize.Width - (borderSize * 2);
            this.trackBarBrightness.Left = (this.ClientSize.Width / 2) - (this.trackBarBrightness.Width / 2);
            this.trackBarBrightness.Top = borderSize;
            this.lblBrightness.Width = this.ClientSize.Width - (borderSize * 2);
            this.lblBrightness.Left = (this.ClientSize.Width / 2) - (this.lblBrightness.Width / 2);
            this.lblBrightness.Top = this.trackBarBrightness.Height + trackBarBrightness.Top;

            this.trackBarSpeed.Width = this.ClientSize.Width - (borderSize * 2);
            this.trackBarSpeed.Left = (this.ClientSize.Width / 2) - (this.trackBarBrightness.Width / 2);
            this.trackBarSpeed.Top = (this.ClientSize.Height / 2) + borderSize;
            this.lblFadeSpeed.Width = this.ClientSize.Width - (borderSize * 2);
            this.lblFadeSpeed.Left = (this.ClientSize.Width / 2) - (this.lblFadeSpeed.Width / 2);
            this.lblFadeSpeed.Top = this.trackBarSpeed.Height + trackBarSpeed.Top;

            this.trackBarBrightness.Visible = true;
            this.trackBarSpeed.Visible = true;
            if (this.trackBarSpeed.Top > lblBrightness.Top)
                this.lblBrightness.Visible = true;
            this.lblFadeSpeed.Visible = true;
        }

        private void ctlPWM_Resize(object sender, EventArgs e)
        {
            adjustControls();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            this.sendValueToNodeDelegate(new pwm_brightness((Int16)trackBarBrightness.Value));
        }

        public override void SetError(string errorString)
        {
            MessageBox.Show(errorString);
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            this.sendValueToNodeDelegate(new pwm_speed((Int16)trackBarSpeed.Value) );
        }
    }
}

