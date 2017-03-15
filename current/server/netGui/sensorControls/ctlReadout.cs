using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using netGui.Properties;

namespace netGui.sensorControls
{
    public partial class ctlReadout : graph
    {
        #region delegate stuff
        public delegate void setlblReadoutTextDelegateType(String toThis);
        private readonly setlblReadoutTextDelegateType setlblReadoutTextDelegate;
        public delegate void setlblReadoutForeColorDelegateType(Color toThis);
        private readonly setlblReadoutForeColorDelegateType setlblReadoutForeColorDelegate;

        public event Action<Icon> onSetIcon;
        private void setIcon(Icon newIcon)
        {
            if (onSetIcon != null)
                onSetIcon.Invoke(newIcon);
        }

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

            // Since we've been passed a boolean, colour it accordingly
            if ((Boolean)toThis)
            {
                //setIcon(new Icon(Properties.Resources._1));
                newcol = Color.Green;
            }
            else
            {
                //setIcon(Properties.Resources._0);
                newcol = Color.Red;
            }

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
