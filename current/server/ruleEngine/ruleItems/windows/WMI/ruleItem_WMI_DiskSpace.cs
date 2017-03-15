using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems.windows.WMI
{
    [ToolboxRule]
    [ToolboxRuleCategory("System Information")]
    public class ruleItem_WMI_DiskSpace : ruleItemBase
    {
        public WMIDiskOptions options = new WMIDiskOptions();

        public override string typedName
        {
            get
            {
                return "WMIDiskSpace";
            }
        }

        public override string ruleName()
        {
            return "Disk Space";
        }

        public override string caption()
        {
            return "Disk Space";
        }
        public override IFormOptions setupOptions()
        {
            return options;
        }

        public override System.Collections.Generic.Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("trigger", new pin() { description = "poll available space", direction = pinDirection.input, name = "trigger" });
            pins.Add("diskSpace", new pin() { name = "diskSpace", description = "Used space (MB)", direction = pinDirection.output, valueType = typeof(pinDataTypes.pinDataNumber) });
            return pins;
        }

        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;

            SelectQuery query = new SelectQuery("SELECT FreeSpace FROM Win32_LogicalDisk WHERE DeviceID =\"" + options.deviceID + "\"");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query) { Scope = options.openScope() };
            var results = searcher.Get();
            ulong freeSpace = 0;
            foreach (var result in results)
            {
                freeSpace = (ulong)result["FreeSpace"];
            }
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataNumber(freeSpace, this, pinInfo["diskSpace"])));
        }
    }

    public class WMIDiskOptions : WMIOptions
    {
        public string deviceID;
        private ComboBox _cbodisks;
        private readonly Label _lblName = new Label();
        private readonly Label _lblSize = new Label();
        private readonly Label _lblType = new Label();
        private List<ManagementObject> _diskList;

        public WMIDiskOptions()
        {
            onScopeOptsChanged += updateDiskList;
        }
        

        private void updateDiskList(ManagementScope scope)
        {
            
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query) { Scope = scope };
            var results = searcher.Get();
            if (_diskList == null)
                _diskList = new List<ManagementObject>(results.Count);
            else
                _diskList.Clear();
            _diskList.AddRange(results.Cast<ManagementObject>());

        }

        public override Control[] getCustomControls()
        {
            if (_diskList == null)
                updateDiskList(openScope());

            _cbodisks = new ComboBox()
                            {
                                DataSource = _diskList.Select(d => new {Display = d["Name"] , Value = d["DeviceID"]}).ToList() ,
                                DisplayMember = "Display" ,
                                ValueMember = "Value" ,
                                DropDownStyle = ComboBoxStyle.DropDownList
                            };
            _cbodisks.SelectedIndexChanged += _cbodisks_SelectedIndexChanged;
            _cbodisks_SelectedIndexChanged(_cbodisks , null);
            return new Control[]
                       {
                           new Label(){Text= "Disks"}, _cbodisks,
                           new Label(){Text= "Volume Name"}, _lblName,
                           new Label(){Text = "Disk Type"}, _lblType,
                           new Label(){Text= "Size"}, _lblSize
                       };

        }

        void _cbodisks_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            foreach(var disk in _diskList)
            {
                if((string)disk["DeviceID"] == (string)_cbodisks.SelectedValue)
                {
                    _lblName.Text = (string) disk["VolumeName"];
                    _lblType.Text = ((DriveType)((uint)disk["DriveType"])).ToString();
                    _lblSize.Text = disk["Size"] == null ? "" :disk["Size"].ToString();
                    break; 
                }
            }
        }

        public override void setCustomValues()
        {
            deviceID = (string) _cbodisks.SelectedValue;
        }

        public override void clearControls()
        {
            throw new System.NotImplementedException();
        }

        public override object Clone()
        {
            return new WMIDiskOptions()
            {
                deviceID = this.deviceID,
                username = this.username,
                _password = this._password,
                computer = this.computer,
            };
        }
    }
}