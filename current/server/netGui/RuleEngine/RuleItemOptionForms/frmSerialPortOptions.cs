namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.IO.Ports;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public partial class frmSerialPortOptions : Form, IOptionForm
    {
        public serialPortOptions opts;

        public frmSerialPortOptions()
        {
            this.InitializeComponent();

            string[] parityNames = Enum.GetNames(typeof(Parity));
            foreach (string parityName in parityNames)
            {
                this.cmboParity.Items.Add(parityName);
            }
            this.cmboParity.SelectedItem = 0;

            string[] handshakeNames = Enum.GetNames(typeof(Handshake));
            foreach (string handshakeName in handshakeNames)
            {
                this.cmboHandshaking.Items.Add(handshakeName);
            }
            this.cmboHandshaking.SelectedItem = 0;

            string[] portnames = SerialPort.GetPortNames();
            foreach (string portname in portnames)
            {
                this.cmboPortName.Items.Add(portname);
            }
            this.cmboPortName.SelectedItem = 0;

            string[] baudRates = new [] { "1200", "2400", "4800", "9600", "19200", "38400" };
            foreach (string baudRate in baudRates)
            {
                this.cmboBaudRate.Items.Add(baudRate);
            }
            this.cmboBaudRate.SelectedItem = 0;

            this.chkUseInterCharDelay.Checked = false;
            this.txtCharDelay.Enabled = false;
            this.txtCharDelay.Text = "0";

            string[] prePostOptions = new string[] { "{Newline}", "{Linefeed}" };
            foreach (string prePostOption in prePostOptions)
            {
                this.cmboPreData.Items.Add(prePostOption);
                this.cmboPostData.Items.Add(prePostOption);
            }
            this.Closing += this.formClosing;
        }

        public frmSerialPortOptions(IFormOptions newOpts) : this()
        {
            // Load up the new options.
            opts = (serialPortOptions)newOpts;
            this.cmboBaudRate.Text = opts.baudRate.ToString();
            if (this.cmboParity.Items.Contains(opts.Parity.ToString()))
                this.cmboParity.SelectedItem = opts.Parity.ToString();
            if (this.cmboHandshaking.Items.Contains(opts.handshake.ToString()))
                this.cmboHandshaking.SelectedItem = opts.handshake.ToString();
            if (this.cmboPortName.Items.Contains(opts.portName))
                this.cmboPortName.SelectedItem = opts.portName;

            if (opts.interCharDelayMS == 0)
            {
                this.chkUseInterCharDelay.Checked = false;
                this.txtCharDelay.Enabled = false;
                this.txtCharDelay.Text = "0";                
            }
            else
            {
                this.chkUseInterCharDelay.Checked = true;
                this.txtCharDelay.Enabled = true;
                this.txtCharDelay.Text = opts.interCharDelayMS.ToString();                
            }

            this.cmboPreData.Text = this.unsubstitute(opts.preSend);
            this.cmboPostData.Text = this.unsubstitute(opts.postSend);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            this.opts.portName = this.cmboPortName.Text.Trim();
            if (this.cmboPortName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a port name");
                return;
            }

            bool baudRateOK = int.TryParse(this.cmboBaudRate.Text, out this.opts.baudRate);
            if (!baudRateOK)
            {
                MessageBox.Show("Please enter a numeric baud rate");
                return;
            }

            if (this.chkUseInterCharDelay.CheckState == CheckState.Unchecked ||
                this.txtCharDelay.Text.Trim() == "")
            {
                this.opts.interCharDelayMS = 0;
            }
            else
            {
                bool interCharDelayOK = int.TryParse(this.txtCharDelay.Text, out this.opts.interCharDelayMS);
                if (!interCharDelayOK)
                {
                    MessageBox.Show("Please enter a numeric inter-character delay rate");
                    return;
                }
            }

            this.opts.handshake = (Handshake)Enum.Parse(typeof(Handshake), this.cmboHandshaking.Text);
            this.opts.Parity = (Parity)Enum.Parse(typeof(Parity), this.cmboParity.Text);

            this.opts.postSend = this.substitute(this.cmboPostData.Text);
            this.opts.preSend = this.substitute(this.cmboPreData.Text);
            opts.setChanged();
            this.DialogResult = DialogResult.OK;
        }

        private string substitute(string input)
        {
            string toRet = Regex.Replace(input, Regex.Escape("{newline}"), "\n", RegexOptions.IgnoreCase);
            toRet = Regex.Replace(toRet, Regex.Escape("{linefeed}"), "\r", RegexOptions.IgnoreCase);

            return toRet;
        }

        private string unsubstitute(string input)
        {
            string toRet = Regex.Replace(input, Regex.Escape("\n"), "{newline}", RegexOptions.IgnoreCase);
            toRet = Regex.Replace(toRet, Regex.Escape("\r"), "{linefeed}", RegexOptions.IgnoreCase);

            return toRet;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkUseInterCharDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkUseInterCharDelay.CheckState == CheckState.Checked)
                this.txtCharDelay.Enabled = true;
            else
                this.txtCharDelay.Enabled = false;
        }

        public IFormOptions SelectedOptions()
        {
            return opts;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
           // throw new NotImplementedException();
        }
    }
}
