using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace netGui.sensorControls
{
    public partial class ctlReadout : graph
    {
        #region delegate stuff
        public delegate void setlblReadoutTextDelegateType(String toThis);
        private readonly setlblReadoutTextDelegateType setlblReadoutTextDelegate;
        public delegate void setlblReadoutForeColorDelegateType(Color toThis);
        private readonly setlblReadoutForeColorDelegateType setlblReadoutForeColorDelegate;

        public void setlblReadoutForeColour(Color toThis)
        {
            lblReadout.ForeColor = toThis;
        }

        public void setlblReadoutText(String toThis)
        {
            lblReadout.Text = toThis;
        }
        #endregion

        public ctlReadout()
        {
            InitializeComponent();

            setlblReadoutTextDelegate = new setlblReadoutTextDelegateType(setlblReadoutText);
            setlblReadoutForeColorDelegate = new setlblReadoutForeColorDelegateType(setlblReadoutForeColour);
        }

        private void ctlReadout_Resize(object sender, EventArgs e)
        {
            lblReadout.Width = this.Width;
            lblReadout.Height = this.Height;
        }

        public override void UpdateValue(bool toThis)
        {
            String toset = "set to " + toThis.ToString();
            Color newcol;

            Assembly a = Assembly.GetExecutingAssembly();
            Stream iconStream; 

            // Since we've been passed a boolean, colour it accordingly
            if ((Boolean)toThis)
            {
                iconStream = a.GetManifestResourceStream("_1");
                newcol = Color.Green;
            }
            else
            {
                iconStream = a.GetManifestResourceStream("_0");
                newcol = Color.Red;
            }

            // todo - rewrite to use Resources._1 etc
            ((frmNodeSensors) this.ParentForm).setIcon(new Icon(iconStream));

            if (this.InvokeRequired)
            {
                this.Invoke(setlblReadoutTextDelegate, toset);
                this.Invoke(setlblReadoutForeColorDelegate, newcol);
            }
            else
            {
                setlblReadoutText(toset);
                setlblReadoutForeColorDelegate( newcol);
            }
        }

        public override void UpdateValue(object toThis)
        {
            if (toThis.GetType() == typeof(Boolean))
                UpdateValue((Boolean)toThis);
            else if (toThis.GetType() == typeof(String))
                UpdateValue((String)toThis);
            else
                UpdateValue("Unable to display data of type " + toThis.GetType().ToString());
        }

        public override void UpdateValue(string toThis)
        {
            String toset = "set to " + toThis.ToString();
            Color newcol;

            newcol = Color.Black;

            if (this.InvokeRequired)
            {
                this.Invoke(setlblReadoutTextDelegate, toset);
                this.Invoke(setlblReadoutForeColorDelegate, newcol);
            }
            else
            {
                setlblReadoutText(toset);
                setlblReadoutForeColorDelegate(newcol);
            }
        }

        public override void SetError(string errorString)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(setlblReadoutTextDelegate, errorString);
                this.Invoke(setlblReadoutForeColorDelegate, Color.Red);
            }
            else
            {
                setlblReadoutText(errorString);
                setlblReadoutForeColour(Color.Red);
            }            
        }
    
    }
}
