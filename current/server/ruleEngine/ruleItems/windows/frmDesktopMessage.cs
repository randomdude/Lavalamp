using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace ruleEngine.ruleItems.windows
{
    public partial class frmDesktopMessage : Form
    {
        public frmDesktopMessage()
        {
            InitializeComponent();

            myOptions = new desktopMessageOptions();
            WindowState = FormWindowState.Normal;
        }

        public frmDesktopMessage(desktopMessageOptions newOptions)
        {
            InitializeComponent();

            myOptions = newOptions;
            lblMessage.Text = newOptions.message;
            WindowState = FormWindowState.Normal;
        }

        // Our delays and other options
        private desktopMessageOptions myOptions;

        // This timer handles fade in/out and holding.
        // Careful to always Dispose() it.
        private Timer myTimer;

        // Fade the form in slightly. Called from timer thread, so don't neglect
        // to .invoke().
        private void fadeInCallback(object state)
        {
            if (this.Disposing)
                return;

            if (this.Opacity < 1.0)
            {
                Invoke(new Action(delegate { Opacity += 0.01; }));
            }
            else
            {
                myTimer.Dispose();
                myTimer = new System.Threading.Timer(holdCallback, null, myOptions.holdSpeed * 100, 0);
            }
        }

        // this is called once the hold timer expires
        private void holdCallback(object state)
        {
            if (this.Disposing)
                return;

            myTimer.Dispose();
            myTimer = new System.Threading.Timer(fadeOutCallback, null, 0, myOptions.fadeOutSpeed * 100 / 100);
        }

        // Fade the form out until it disappears. Don't forget to .Invoke()
        // since we're called from the timer thread.
        private void fadeOutCallback(object state)
        {
            if (this.Disposing)
                return;

            if (this.Opacity > 0)
            {
                Invoke(new Action(delegate { Opacity -= 0.01; }));
            }
            else
            {
                myTimer.Dispose();
                if (this.InvokeRequired)
                    Invoke(new Action(delegate { this.Close(); }));
            }
        }

        private void control_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right)
            {
                myTimer.Dispose();
                Invoke(new Action(delegate { this.Close(); }));
            }
        }

        private void frmDesktopMessage_Shown(object sender, EventArgs e)
        {
            // Prepare our form, making it transparent so that we can fade in
            this.Opacity = 0;
            this.Visible = false;

            // Position us 
            Rectangle screenSize = Screen.PrimaryScreen.WorkingArea;

            if (myOptions.location == desktopMessageLocation.BottomLeft ||
                myOptions.location == desktopMessageLocation.BottomMiddle ||
                myOptions.location == desktopMessageLocation.BottomRight)
                this.Top = screenSize.Height - this.Height;

            if (myOptions.location == desktopMessageLocation.TopLeft ||
                myOptions.location == desktopMessageLocation.TopMiddle ||
                myOptions.location == desktopMessageLocation.TopRight)
                this.Top = 0;

            if (myOptions.location == desktopMessageLocation.BottomLeft ||
                myOptions.location == desktopMessageLocation.TopLeft)
                this.Left = 0;

            if (myOptions.location == desktopMessageLocation.BottomMiddle ||
                myOptions.location == desktopMessageLocation.TopMiddle)
                this.Left = (screenSize.Width / 2) - (this.Width / 2);

            if (myOptions.location == desktopMessageLocation.BottomRight ||
                myOptions.location == desktopMessageLocation.TopRight)
                this.Left = screenSize.Width - this.Width;

            lblMessage.ForeColor = myOptions.foreground;
            this.BackColor = myOptions.background;

            this.Visible = true;

            // Set our fade in going!
            myTimer = new System.Threading.Timer(fadeInCallback, null, 0, myOptions.fadeInSpeed * 100 / 100);
        }

    }
}

