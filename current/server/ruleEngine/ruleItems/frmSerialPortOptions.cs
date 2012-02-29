using System;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ruleEngine.ruleItems
{
    public partial class frmSerialPortOptions : Form
    {
        public serialPortOptions opts;

        public frmSerialPortOptions()
        {
            InitializeComponent();

            string[] parityNames = Enum.GetNames(typeof(Parity));
            foreach (string parityName in parityNames)
            {
                cmboParity.Items.Add(parityName);
            }
            cmboParity.SelectedItem = 0;

            string[] handshakeNames = Enum.GetNames(typeof(Handshake));
            foreach (string handshakeName in handshakeNames)
            {
                cmboHandshaking.Items.Add(handshakeName);
            }
            cmboHandshaking.SelectedItem = 0;

            string[] portnames = SerialPort.GetPortNames();
            foreach (string portname in portnames)
            {
                cmboPortName.Items.Add(portname);
            }
            cmboPortName.SelectedItem = 0;

            string[] baudRates = new [] { "1200", "2400", "4800", "9600", "19200", "38400" };
            foreach (string baudRate in baudRates)
            {
                cmboBaudRate.Items.Add(baudRate);
            }
            cmboBaudRate.SelectedItem = 0;

            chkUseInterCharDelay.Checked = false;
            txtCharDelay.Enabled = false;
            txtCharDelay.Text = "0";

            string[] prePostOptions = new string[] { "{Newline}", "{Linefeed}" };
            foreach (string prePostOption in prePostOptions)
            {
                cmboPreData.Items.Add(prePostOption);
                cmboPostData.Items.Add(prePostOption);
            }
        }

        public frmSerialPortOptions(serialPortOptions newOpts) : this()
        {
            // Load up the new options.
            cmboBaudRate.Text = newOpts.baudRate.ToString();
            if (cmboParity.Items.Contains(newOpts.Parity.ToString()))
                cmboParity.SelectedItem = newOpts.Parity.ToString();
            if (cmboHandshaking.Items.Contains(newOpts.handshake.ToString()))
                cmboHandshaking.SelectedItem = newOpts.handshake.ToString();
            if (cmboPortName.Items.Contains(newOpts.portName))
                cmboPortName.SelectedItem = newOpts.portName;

            if (newOpts.interCharDelayMS == 0)
            {
                chkUseInterCharDelay.Checked = false;
                txtCharDelay.Enabled = false;
                txtCharDelay.Text = "0";                
            }
            else
            {
                chkUseInterCharDelay.Checked = true;
                txtCharDelay.Enabled = true;
                txtCharDelay.Text = newOpts.interCharDelayMS.ToString();                
            }

            cmboPreData.Text = unsubstitute(newOpts.preSend);
            cmboPostData.Text = unsubstitute(newOpts.postSend);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            opts = new serialPortOptions();

            opts.portName = cmboPortName.Text.Trim();
            if (cmboPortName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a port name");
                return;
            }

            bool baudRateOK = int.TryParse(cmboBaudRate.Text, out opts.baudRate);
            if (!baudRateOK)
            {
                MessageBox.Show("Please enter a numeric baud rate");
                return;
            }

            if (chkUseInterCharDelay.CheckState == CheckState.Unchecked ||
                txtCharDelay.Text.Trim() == "")
            {
                opts.interCharDelayMS = 0;
            }
            else
            {
                bool interCharDelayOK = int.TryParse(txtCharDelay.Text, out opts.interCharDelayMS);
                if (!interCharDelayOK)
                {
                    MessageBox.Show("Please enter a numeric inter-character delay rate");
                    return;
                }
            }

            opts.handshake = (Handshake)Enum.Parse(typeof(Handshake), cmboHandshaking.Text);
            opts.Parity = (Parity)Enum.Parse(typeof(Parity), cmboParity.Text);

            opts.postSend = substitute(cmboPostData.Text);
            opts.preSend = substitute(cmboPreData.Text);

            DialogResult = DialogResult.OK;
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
            DialogResult = DialogResult.Cancel;
        }

        private void chkUseInterCharDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseInterCharDelay.CheckState == CheckState.Checked)
                txtCharDelay.Enabled = true;
            else
                txtCharDelay.Enabled = false;
        }
    }
}
