using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer=System.Threading.Timer;

namespace netGui.RuleEngine.ruleItems.windows
{
    public partial class frmDesktopMessage : Form
    {
        public frmDesktopMessage()
        {
            myOptions = new desktopMessageOptions(); 
            
            InitializeComponent();
        }

        public frmDesktopMessage(desktopMessageOptions newOptions, string newMessage)
        {
            myOptions = newOptions;

            InitializeComponent();

            lblMessage.Text = newMessage;
        }

        // Our delays and other options
        private desktopMessageOptions myOptions;

        // This timer handles fade in/out and holding.
        private Timer myTimer;

        // Fade the notifier in, hold it, then fade it out
        public void showIt()
        {
            this.Show();
            this.Opacity = 0;
            this.Visible = true;
            this.BringToFront();    // TODO: 'always on top' behaviour
            myTimer = new System.Threading.Timer(fadeInCallback, null, 0, myOptions.fadeInSpeed * 100 / 100);
        }

        // Fade the form in slightly. Called from timer thread, so don't neglect
        // to .invoke().
        private void fadeInCallback(object state)
        {
            if (this.Opacity < 1.0)
                Invoke(new Action(delegate { Opacity+=0.01; }));
            else
            myTimer = new System.Threading.Timer(holdCallback, null, myOptions.holdSpeed * 100, 0);
        }

        // this is called once the hold timer expires
        private void holdCallback(object state)
        {
            myTimer = new System.Threading.Timer(fadeOutCallback, null, 0, myOptions.fadeOutSpeed * 100 / 100);
        }

        // Fade the form out until it disappears. Don't forget to .Invoke()
        // since we're called from the timer thread.
        private void fadeOutCallback(object state)
        {
            if (this.Opacity > 0)
            {
                Invoke(new Action(delegate { Opacity -= 0.01; }));
            }
            else
            {
                try {
                    myTimer.Dispose();
                    Invoke(new Action(delegate { this.Close(); }));
                } catch (ObjectDisposedException)
                {
                    // If this exception is thrown, then a previous call, originating
                    // from the Timer object, has caused us to dispos our form. Just 
                    // ignore the exception.
                }
            }
        }

    }
}

