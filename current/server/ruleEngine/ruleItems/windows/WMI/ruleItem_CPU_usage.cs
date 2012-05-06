using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;
using System.Linq;


namespace ruleEngine.ruleItems.windows.WMI
{
    [ToolboxRule]
    [ToolboxRuleCategory("System Information")]
    public class ruleItem_CPU_usage : ruleItemBase
    {
     
        [XmlElement]
        public CPUUsageOptions options = new CPUUsageOptions();

        public override string ruleName()
        {
            return "CPU Usage";
        }

        public override string caption()
        {
            return "CPU Usage";
        }

        public override Dictionary<string, pin> getPinInfo()
        {

            var pins = base.getPinInfo();
            pins.Add("trigger", new pin() { name = "trigger", description = "poll", direction = pinDirection.input, valueType = typeof(pinDataTypes.pinDataTrigger) });
            pins.Add("CPUUsage", new pin() { name = "CPUUsage", description = "processor load (%)", valueType = typeof(pinDataTypes.pinDataNumber), direction = pinDirection.output });
            return pins;
        }

        public override void evaluate()
        {
            if (!(pinInfo["trigger"].value.asBoolean()))
                return;

            SelectQuery wQuery = new SelectQuery("SELECT LoadPercentage FROM Win32_Processor WHERE DeviceID=\""+ options.deviceID + "\"");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wQuery) {Scope = options.openScope()};
            ManagementObjectCollection processors = searcher.Get();
            ushort toRet = 0;
            foreach (var processor in processors)
            {
                 toRet = (ushort)processor["LoadPercentage"];
            }
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataNumber(toRet, this, pinInfo["CPUUsage"])));
        }

        public override IFormOptions setupOptions()
        {

            return options;
        }

        public override string ToString()
        {
            return "Options...";
        }
    }

    
    public class CPUUsageOptions : WMIOptions
    {
        [XmlElement]
        public string deviceID;

        private List<ManagementBaseObject> _processorList = new List<ManagementBaseObject>();

        private Label manuLabel = new Label();
        private Label descLabel = new Label();
        private Label archiLabel = new Label();
        private Label addressLabel = new Label();
        private Label coreNumLabel = new Label();
        private Label dataWidthLabel = new Label();
        private Label clockLabel = new Label();
        private Label familyLabel = new Label();
        private ComboBox cboProcessorList;

        public CPUUsageOptions()
        {
            onScopeOptsChanged += updateProcessorList;

        }
        void updateProcessorList(ManagementScope scope)
        {
            _processorList.Clear();
            SelectQuery wQuery = new SelectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wQuery) { Scope = scope };
            ManagementObjectCollection processors = searcher.Get();
            foreach (ManagementBaseObject processor in processors)
                _processorList.Add(processor);
            deviceID = _processorList.First()["DeviceID"].ToString();

        }

        public override Control[] getCustomControls()
        {
            if (_processorList == null)
                updateProcessorList(openScope());
            cboProcessorList = new ComboBox()
            {
                DataSource = _processorList.Select(m => new { Name = m["Name"], ID = m["DeviceID"] }).ToList(),
                                            DisplayMember = "Name", ValueMember = "ID",
                                            DropDownStyle = ComboBoxStyle.DropDownList 
            };
          
            cboProcessorList.DisplayMemberChanged += processorChanged;
            return new Control[]
                       {
                           new Label() {Text = "Processor:"} , cboProcessorList ,
                           new Label() {Text = "Manufacturer:"} , manuLabel ,
                           new Label() {Text = "Description:"} , descLabel ,
                           new Label() {Text = "Architecture:"} , archiLabel ,
                           new Label() {Text = "Address Width:"} , addressLabel ,
                           new Label() {Text = "Number Of Cores:"} , coreNumLabel ,
                           new Label() {Text = "Data Width:"} , dataWidthLabel ,
                           new Label() {Text = "Family:"} , familyLabel ,
                           new Label() {Text = "Max Clock Speed:"} , clockLabel
                       };
        }

        private void processorChanged(object sender , EventArgs e)
        {
            foreach (var processor in _processorList)
            {
                if (((((ComboBox)sender).SelectedValue != null && (string)processor["DeviceID"] == ((ComboBox)sender).SelectedValue.ToString()) ||
                   (((string)processor["DeviceID"] == deviceID && ((ComboBox)sender).SelectedValue == null))))
                {
                    manuLabel.Text = processor["Manufacturer"].ToString();
                    descLabel.Text = processor["Description"].ToString();
                    archiLabel.Text = processor["Architecture"].ToString();
                    addressLabel.Text = processor["AddressWidth"].ToString();
                    coreNumLabel.Text = processor["NumberOfCores"].ToString();
                    dataWidthLabel.Text = processor["DataWidth"].ToString();
                    familyLabel.Text = processor["Family"].ToString();
                    clockLabel.Text = processor["MaxClockSpeed"].ToString();
                    deviceID = processor["DeviceID"].ToString();
                    break;

                }
                
        }

    ;
        }

        public override void setCustomValues()
        {
            deviceID = (string) cboProcessorList.SelectedValue;
        }

        public override void clearControls()
        {

        }

        public override object Clone()
        {
            CPUUsageOptions clone = new CPUUsageOptions()
                                        {
                                            deviceID = this.deviceID,
                                            username = this.username,
                                            _password = this._password,
                                            computer = this.computer
                                        };
            return clone;
        }
    }
}
