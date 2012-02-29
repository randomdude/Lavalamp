using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using transmitterDriver;

namespace netGui.RuleEngine.windows
{
    public partial class frmSensorOptions : Form
    {
        private readonly sensorType _tpy;
        public frmSensorOptions(sensorSettings settings)
        {
            InitializeComponent();

            _tpy = settings.selectedType;
            List<Node> allNodes = FrmMain.getAllConnectedNodes();
            foreach (Node node in allNodes)
            {
                if (node.hasSensorOf(settings.selectedType))
                {
                    cboNodes.Items.Add(node);
                }
            }
            if (cboNodes.Items.Count == 0)
            {
                lblError.Text = "No nodes of this type are connected";
            }
            if (settings.selectedSensor != null)
                cboNodes.SelectedItem = settings.selectedSensor;
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
