using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using transmitterDriver;

namespace netGui.RuleEngine.windows
{
    public partial class frmSensorOptions : Form
    {
        private readonly sensorType _tpy;
        
        public frmSensorOptions(sensorTypeEnum tpy, sensor selectedSensor)
        {

            InitializeComponent();
            _tpy = new sensorType(tpy);
            List<Node> allNodes = FrmMain.getAllConnectedNodes();
            foreach (Node node in allNodes)
            {
                
                if (node.hasSensorOf(_tpy))
                {
                    cboNodes.Items.Add(node);
                }
            }
            if (cboNodes.Items.Count == 0)
            {
                lblError.Text = "No nodes of this type are connected";
                btnOK.Enabled = false;
                cboNodes.Enabled = false;
            }
            else
            {
                lblError.Text = "";
                cboNodes.SelectedIndex = 0;
            }
            if (selectedSensor != null)
                cboNodes.SelectedItem = selectedSensor;
        }

        [Pure]
        public sensor selectedSensor()
        {
            if (cboSensors.Items.Count == 0)
            {
                Node nd = (Node) cboNodes.SelectedItem;
                return nd.getSensorsOfType(_tpy)[0];
            }
            return (sensor) cboSensors.SelectedItem;
            Contract.Ensures(Contract.Result<sensor>() != null);
        }

        private void cboNodes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Node nd = (Node) cboNodes.SelectedItem;
            List<sensor> sensors = nd.getSensorsOfType(_tpy);
            cboSensors.Items.Clear();
            if (sensors.Count > 1)
            {
                foreach (sensor sensor in sensors)
                {
                    cboSensors.Items.Add(sensor);
                }
                lblSensors.Visible = true;
                cboSensors.Visible = true;
                this.Height = 196;
            }
            else
            {
                lblSensors.Visible = false;
                cboSensors.Visible = false;
                this.Height = 135;
            }
        }
    }
}
