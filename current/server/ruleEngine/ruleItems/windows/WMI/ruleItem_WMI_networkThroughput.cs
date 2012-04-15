using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Instrumentation;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems.windows.WMI
{
    [ToolboxRule]
    [ToolboxRuleCategory("System Information")]
    public class ruleItem_WMI_networkThroughput : ruleItemBase
    {
        [XmlElement]
        public WMINetworkOptions options = new WMINetworkOptions();

        public override string ruleName()
        {
            return "Network Throughput";
        }

        public override string caption()
        {
            return "Network Throughput";
        }

        public override Form ruleItemOptions()
        {
            frmWMIOptions wmiOptions = new frmWMIOptions(options);
            wmiOptions.Closed += new EventHandler(wmiOptions_Closed);
            return wmiOptions;
        }

        void wmiOptions_Closed(object sender, EventArgs e)
        {
            frmWMIOptions wmiOptions = (frmWMIOptions) sender;
            if (wmiOptions.DialogResult == DialogResult.OK)
                options = (WMINetworkOptions) wmiOptions.SelectedOptions();
        }

        public override Dictionary<string, pin> getPinInfo()
        {

            var pins = base.getPinInfo();
            pins.Add("trigger", new pin() { name = "trigger", description = "poll", direction = pinDirection.input, valueType = typeof(pinDataTypes.pinDataTrigger) });
            pins.Add("networkThroughput", new pin() { name = "networkThroughput", description = "networkThroughput (bps)", valueType = typeof(pinDataTypes.pinDataNumber), direction = pinDirection.output });
            return pins;
        }

        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;

            SelectQuery query = new SelectQuery("SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
            ManagementObjectSearcher search = new ManagementObjectSearcher(query) { Scope = options.openScope() };
            ManagementObjectCollection networkUsage = search.Get();
            ulong bandWidth = 0;
            foreach (var perSec in networkUsage)
            {
                if (normalizedName((string) perSec["Name"]) == normalizedName(options.chosenAdapterName))
                    bandWidth = (ulong) perSec["BytesTotalPerSec"];
            }
            bandWidth *= 8; // convert to bits

            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataNumber(bandWidth, this, pinInfo["networkThroughput"])));

        }

        private string normalizedName(string chosenAdapterName)
        {
            Regex normalize = new Regex(@"[^A-Za-z0-9]");
            return normalize.Replace(chosenAdapterName , "_");
        }
    }


    public class WMINetworkOptions : WMIOptions
    {
        public WMINetworkOptions()
        {
            onScopeOptsChanged += updateNetworkAdapters;
        }
        private enum NetConnectionStatus : ushort
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2,
            Disconnecting = 3,
            Hardware_not_present = 4,
            Hardware_disabled = 5,
            Hardware_malfunction = 6,
            Media_disconnected = 7,
            Authenticating = 8,
            Authentication_succeeded = 9,
            Authentication_failed = 10,
            Invalid_address = 11,
            Credentials_required = 12
        }

        private List<ManagementObject> _networkAdapterList = new List<ManagementObject>();

        public string chosenAdapterName
        {
            get
            {
                if (string.IsNullOrEmpty(_adapterName))
                {
                    //initialize network adapter list if needed
                    if (_networkAdapterList == null || _networkAdapterList.Count == 0)
                        updateNetworkAdapters(openScope());
                    //and default to first adapter found.
                    _adapterName = (string) _networkAdapterList[0]["Name"];
                }
                return _adapterName;
            }
            set { _adapterName = value; }
        }

        private ComboBox _cbonetworkAdapters;
        private readonly Label _lblDescription = new Label();
        private readonly Label _lblMac = new Label();
        private readonly Label _lblSpeed = new Label();
        private readonly Label _lblStatus = new Label();
        private string _adapterName;

        public override Control[] getCustomControls()
        {
            if (_networkAdapterList == null)
                updateNetworkAdapters(openScope());
            //we only look for network adapters which are user managed.
            var data = _networkAdapterList.Select(
                x => new {Display = (string)x["NetConnectionID"] , Value = (string)x["Name"]}).ToList();
            _cbonetworkAdapters = new ComboBox()
                                      {
                                          DataSource = data,
                                          DisplayMember = "Display" ,
                                          ValueMember = "Value",
                                          DropDownStyle = ComboBoxStyle.DropDownList
                                      };
            _cbonetworkAdapters.SelectedIndexChanged += networkAdapterChanged;
            networkAdapterChanged(_cbonetworkAdapters, null);
            return new Control[]
                                 {
                                     new Label(){Text = "Network Adapters:"},_cbonetworkAdapters,
                                     new Label() {Text = "Description" }, _lblDescription,
                                     new Label() {Text = "MAC"}, _lblMac,
                                     new Label() {Text = "Speed"}, _lblSpeed,
                                     new Label() {Text = "Status"}, _lblStatus
                                 };
            
        }

        public override void setCustomValues()
        {
            chosenAdapterName = (string) _cbonetworkAdapters.SelectedValue;
        }

        public override void clearControls()
        {
   
        }

        public override object Clone()
        {
            return new WMINetworkOptions()
            {
                chosenAdapterName = this.chosenAdapterName,
                username = this.username,
                _password = this._password,
                computer = this.computer,
            };
        }

        private void updateNetworkAdapters(ManagementScope scope)
        {
            _networkAdapterList.Clear();
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query) {Scope = scope};
            ManagementObjectCollection col = searcher.Get();
           
            _networkAdapterList.AddRange(col.Cast<ManagementObject>());
        }

        private void networkAdapterChanged(object sender, EventArgs e)
        {
            foreach (var networkAdapter in _networkAdapterList)
            {
                if (((((ComboBox)sender).SelectedValue != null && (string)networkAdapter["Name"] == ((ComboBox)sender).SelectedValue.ToString()) ||
                   (((string)networkAdapter["Name"] == chosenAdapterName && ((ComboBox)sender).SelectedValue == null))))
                {
                    _lblDescription.Text = (string) networkAdapter["Description"];
                    _lblMac.Text = (string) networkAdapter["MACAddress"];
                    _lblSpeed.Text = networkAdapter["Speed"].ToString() + "bps" ?? "Unknown";
                    _lblStatus.Text = ((NetConnectionStatus) networkAdapter["NetConnectionStatus"]).ToString();
                }
            }
             
        }

    }
}
