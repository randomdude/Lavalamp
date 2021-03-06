using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace netGui
{
    using System.IO.Pipes;

    public partial class FrmGeneralOptions : Form
    {
        public bool cancelled;
        public options MyOptions;

        public FrmGeneralOptions()
        {
            InitializeComponent();
            cancelled = false;
            if ( null == MyOptions )
                MyOptions = new options();

            txtKey.Enabled = chkUseEncryption.Checked;
			cboPort.Items.AddRange(SerialPort.GetPortNames());
			
        }

        public FrmGeneralOptions(options oldOptions)
        {
            InitializeComponent();
            cancelled = false;
            MyOptions = oldOptions;
            txtKey.Enabled = chkUseEncryption.Checked;
			cboPort.Items.AddRange(SerialPort.GetPortNames());
        }


        private void cmdOK_Click(object sender, EventArgs e)
        {
            MyOptions.portname = cboPort.Text;
            MyOptions.rulesPath = txtRulePath.Text;
            MyOptions.useEncryption = chkUseEncryption.Checked;
			
            try
            {
                MyOptions.myKey.setKey(txtKey.Text);
				MyOptions.save();
                DestroyHandle();
            } 
            catch (FormatException) 
            {
                MessageBox.Show(this, "Your network key must be a 32-character value, containing values from zero through nine, and 'A' through 'F'.");
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancelled = true;
            DestroyHandle();
        }
		
		private void cmdReload_Click(object sender, EventArgs e)
		{
			cboPort.Items.Clear();
			cboPort.Items.AddRange(SerialPort.GetPortNames());
		}
		/// <summary>
		/// Checks the valid port.
		/// in future this could use lavalamp's methods to detect if a node is on a port 
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		private void CheckVaildPort(object sender, EventArgs e)
		{
			try
			{
			    bool success;
                if (cboPort.Text.Contains("pipe"))
                {
                    int pipeNameIndex = cboPort.Text.LastIndexOf(@"\");
                    using (NamedPipeClientStream pipe = new NamedPipeClientStream(cboPort.Text.Substring(pipeNameIndex, (cboPort.Text.Length - pipeNameIndex))))
                    {
                        pipe.Connect(50);
                        success = pipe.IsConnected;
                        pipe.Close();
                    }
                }
                else
                {
                    using (SerialPort selectedPort = new SerialPort(cboPort.Text))
                    {
                        selectedPort.Open();
                        success = selectedPort.IsOpen;
                        selectedPort.Close();
                    }
                }

                if (success)
                {
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Free";
                }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                    lblStatus.Text = "Cannot Open Port";
                }
			}
			catch(Exception ex)
			{
				if (ex is AccessViolationException || ex is InvalidOperationException)
				{
					lblStatus.ForeColor = System.Drawing.Color.Orange;
					lblStatus.Text = "Already Open";
				}
				else if (ex is ArgumentException)
				{
					lblStatus.ForeColor = System.Drawing.Color.Red;
					lblStatus.Text = "Invaild Port";
				}
                else
				{
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                    lblStatus.Text = "Cannot Open Port";
                }
            }

        }

        private void FrmGeneralOptions_Load(object sender, EventArgs e)
        {
            cboPort.Text = MyOptions.portname;
            txtKey.Text = MyOptions.myKey.ToString();
            chkUseEncryption.Checked = MyOptions.useEncryption;
        }

        private void chkUseEncryption_CheckedChanged(object sender, EventArgs e)
        {
            this.txtKey.Enabled = chkUseEncryption.Checked;
        }

        private void btnOpenDlg_Click(object sender, EventArgs e)
        {
            dlgRulePath.ShowDialog(this);
        }

    }
}