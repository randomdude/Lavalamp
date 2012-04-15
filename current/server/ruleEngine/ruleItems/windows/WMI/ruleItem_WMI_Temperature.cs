using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.pinDataTypes;

namespace ruleEngine.ruleItems.windows.WMI
{
    [ToolboxRule]
    [ToolboxRuleCategory("System Information")]
    public class ruleItem_WMI_Temperature : ruleItemBase
    {
        WMITemperatureOptions options = new WMITemperatureOptions();
        public override string ruleName()
        {
            return "Temperature Monitor";
        }
        public override string caption()
        {
            return "Temperature Monitor";
        }
        public override System.Windows.Forms.Form ruleItemOptions()
        {
            frmWMIOptions frmOpts = new frmWMIOptions(options);
            frmOpts.Closed += frmOpts_Closed;
            return frmOpts;
        }

        void frmOpts_Closed(object sender, System.EventArgs e)
        {
            frmWMIOptions frmOpts = (frmWMIOptions) sender;
            if (frmOpts.DialogResult == DialogResult.OK)
                options = (WMITemperatureOptions) frmOpts.SelectedOptions();
        }

        public override System.Collections.Generic.Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("trigger", new pin() { name = "trigger", description = "poll", direction = pinDirection.input});
            pins.Add("temperature", new pin(){name = "temperature", description = "temperature", direction = pinDirection.output, valueType = typeof(pinDataTypes.pinDataNumber)});
            return pins;
        }


        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;
            TemperatureSensor sensor = options.SelectedSensor;
            if(!options.changeWMINamespace(sensor.Namespace))
                errorHandler(new Exception("Couldn't change to WMI Namespace" + sensor.Namespace + " where the sensor is located"));

            SelectQuery query = new SelectQuery(string.Format("SELECT {0} FROM {1} WHERE {2} = \"{3}\" ", sensor.ReadingProperty, sensor.Classname, sensor.IDProperty, sensor.ID));
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query){Scope = options.openScope()};
            float newReading = 0;
            foreach (var reading in searcher.Get())
            {
                newReading = (float) reading[sensor.ReadingProperty];
            }
            onRequestNewTimelineEvent(new timelineEventArgs(new pinDataNumber(newReading, this, pinInfo["temperature"])));
        }

        public override System.Drawing.Size preferredSize()
        {
            return new Size(100,75);
        }
    }

    public class WMITemperatureOptions : WMIOptions
    {
        private const string PREFERED_NAMESPACE = "OpenHardwareMonitor";
        private TemperatureSensor _sensor;
        [XmlElement]
        public TemperatureSensor SelectedSensor
        {
            get 
            { 
                if (_sensor == null)
                {
                    if (_sensorList.Count <= 0)
                        probeForSensors();
                    if (_sensorList.Count > 0)
                    {
                        _sensor = _sensorList[0];
                    }
                }
                return _sensor;
            }
            set { _sensor = value; }
        }
        [XmlIgnore]
        private List<TemperatureSensor> _sensorList = new List<TemperatureSensor>();
        private ComboBox _cboNamespaces = new ComboBox(){DisplayMember = "Display",ValueMember = "Value"};
        private ComboBox _cboSensors;
        private Label _lblMinimum = new Label();
        private Label _lblMaximum = new Label();
        private List<string> _erroredNamespaces = new List<string>();

        public override Control[] getCustomControls()
        {
            if (_sensorList == null || _sensorList.Count <= 0)
            {
                if (!probeForSensors())
                {
                    _cboSensors = new ComboBox(){DropDownStyle = ComboBoxStyle.DropDownList};
                    _cboSensors.Items.Add("No Sensors Found");
                    LinkLabel link = new LinkLabel()
                                          {
                                              Text =
                                                  "No Sensors found."
                                          };
                    link.Click += (sender, args) => Process.Start("http://openhardwaremonitor.org");
                    return new Control[] {
                                            new Label() {Text = "Present Sensors"} , link,               
                    };
                }
            }

            List<Control> controls = new List<Control>();
            if (_cboNamespaces.Items.Count > 1)
            {
                _cboNamespaces.Items.Add(new { Display = "All", Value = "All" });
                _cboNamespaces.SelectedIndexChanged += _cboNamespaces_SelectedIndexChanged;
                controls.Add(new Label() { Text = "Available WMI Namespaces" });
                controls.Add(_cboNamespaces);
            }

            
            _cboSensors = new ComboBox()
                              {
                                  DataSource = _sensorList.Select(s => new {Display = s.Name , Value = s.ID}).ToList() ,
                                  DisplayMember = "Display" ,
                                  ValueMember = "Value" ,
                                  DropDownStyle = ComboBoxStyle.DropDownList
                              };
            _cboSensors.SelectedIndexChanged += _cboSensors_SelectedIndexChanged;
            if (SelectedSensor == null)
                _cboSensors.SelectedIndex = 0;
            else
                _cboSensors.SelectedValue = SelectedSensor.ID;
            _cboSensors_SelectedIndexChanged(_cboSensors, null);

            controls.AddRange(new Control[]
                       {
                           new Label() {Text = "Present Sensors"} ,_cboSensors,
                           new Label() {Text = "Minimum Recorded Value"}, _lblMinimum,
                           new Label() {Text = "Maximum Recorded Value"}, _lblMaximum,
                       });
            
            return controls.ToArray();

        }


        void _cboSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var temperatureSensor in _sensorList)
            {
                if ((SelectedSensor == null) || temperatureSensor.ID == (string) SelectedSensor.ID)
                {
                    _lblMaximum.Text = temperatureSensor.MaximumVal.ToString();
                    _lblMinimum.Text = temperatureSensor.MinimumVal.ToString();
                    break;
                }
            }
        }

        void _cboNamespaces_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((string)_cboNamespaces.SelectedValue == "All")
                _cboSensors.DataSource = _sensorList.Select(s => new { Display = s.Name, Value = s.ID }).ToArray();
            else
                _cboSensors.DataSource = _sensorList.Where(s => s.Namespace == (string)_cboNamespaces.SelectedValue).Select(s => new { Display = s.Name, Value = s.ID }).ToArray();
            _cboSensors.ResetBindings();
            _cboSensors_SelectedIndexChanged(_cboSensors, null);
        }

        public override void setCustomValues()
        {
            SelectedSensor = _sensorList.Single(s => s.ID == (string) _cboSensors.SelectedValue);

        }

        public override void clearControls()
        {
            throw new System.NotImplementedException();
        }

        public override object Clone()
        {
            return new WMITemperatureOptions()
                       {
                           SelectedSensor = SelectedSensor,
                           password = password,
                           username = username,
                           computer = computer,
                           _erroredNamespaces = this._erroredNamespaces
                       };
        }

        private bool probeForSensors()
        {
            _sensorList.Clear();
            _cboNamespaces.Items.Clear();
            if (!_erroredNamespaces.Contains("cimv2") && changeWMINamespace("cimv2"))
            {
                // this probably won't work due to lack of platform support and if there are any sensors they may not actually produce
                // temperature readouts right now but its here for (hopeful) future support
                try
                {
                    SelectQuery temperatureProbes = new SelectQuery("SELECT * FROM Win32_TemperatureProbe");
                    ManagementObjectSearcher temperatureProbeSearcher = new ManagementObjectSearcher(temperatureProbes)
                                                                            {Scope = openScope()};
                    bool sensorsHere = false;
                    foreach (var sensor in temperatureProbeSearcher.Get())
                    {
                        _sensorList.Add(new TemperatureSensor((ManagementObject) sensor)
                                            {
                                                IDProperty = "DeviceID" ,
                                                NameProperty = "Name" ,
                                                ReadingProperty = "CurrentReading" ,
                                                MaximumValProperty = "NormalMax" ,
                                                MinimumValProperty = "NormalMin" ,
                                                Namespace = "cimv2" ,
                                                Classname = "Win32_TemperatureProbe"
                                            });
                        sensorsHere = true;

                    }
                    if (sensorsHere && !_cboNamespaces.Items.Contains(new { Display = "WMI Root (Not Recommended)", Value = "cimv2" }))
                        _cboNamespaces.Items.Add(new {Display = "WMI Root (Not Recommended)" , Value = "cimv2"});
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message + " happened when trying to get Temperature sensors from the Win32_TemperatureProbe object in cimv2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _erroredNamespaces.Add("cimv2");
                }
            }
            //try to probe published temperature senses from the OpenHardwareMonitor project if its installed.
            if (!_erroredNamespaces.Contains("OpenHardwareMonitor") && changeWMINamespace("OpenHardwareMonitor"))
            {
                 try // permissions violations can happen when we're querying
                 {
                     SelectQuery hardwareQuery = new SelectQuery("SELECT * FROM Hardware");
                     ManagementScope scope = openScope();
                     ManagementObjectSearcher hardwareSearcher = new ManagementObjectSearcher(hardwareQuery)
                                                                     {Scope = scope};
                     bool sensorsHere = false;
                     foreach (var hardware in hardwareSearcher.Get())
                     {
                         SelectQuery sensorQuery =
                             new SelectQuery("SELECT * FROM Sensor WHERE Parent=\"" + hardware["Identifier"] +
                                             "\" AND SensorType=\"Temperature\" ");
                         ManagementObjectSearcher sensorSearcher = new ManagementObjectSearcher(sensorQuery)
                                                                       {Scope = scope};
                         foreach (var sensor in sensorSearcher.Get())
                         {
                             _sensorList.Add(new TemperatureSensor((ManagementObject) sensor)
                                                 {
                                                     NameProperty = "Name" ,
                                                     ReadingProperty = "Value",
                                                     MaximumValProperty = "Max" ,
                                                     MinimumValProperty = "Min" ,
                                                     IDProperty = "Identifier" ,
                                                     Namespace = "OpenHardwareMonitor" ,
                                                     Classname = "Sensor"
                                                 });
                             sensorsHere = true;
                         }
                         if (sensorsHere && !_cboNamespaces.Items.Contains( new {Display = "Open Hardware Monitor (Recommended)" , Value = "OpenHardwareMonitor"}))
                             _cboNamespaces.Items.Add(
                                 new {Display = "Open Hardware Monitor (Recommended)" , Value = "OpenHardwareMonitor"});
                     }
                 }
                 catch (Exception e)
                {
                    // this needs passing to the gui somehow but we'll suppress it for now :$
                   // MessageBox.Show(e.Message + " happened when trying to get Temperature sensors from the sensor object in OpenHardwareMonitor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _erroredNamespaces.Add("OpenHardwareMonitor");
                 }
            }


            if (!_erroredNamespaces.Contains("WMI") && changeWMINamespace("WMI"))
            {
                try
                {
                    bool sensorsHere = false;
                    SelectQuery msThemalQuery = new SelectQuery("SELECT * FROM MSAcpi_ThermalZoneTemperature");
                    ManagementObjectSearcher msThemalSearcher = new ManagementObjectSearcher(msThemalQuery) { Scope = openScope() };
                    foreach (var themalSensor in msThemalSearcher.Get())
                    {
                        _sensorList.Add(new TemperatureSensor((ManagementObject)themalSensor)
                                            {
                                                IDProperty = "InstanceName",
                                                ReadingProperty = "CurrentTemperature",
                                                NameProperty = "InstanceName",
                                                Namespace = "WMI",
                                                Classname = "MSAcpi_ThermalZoneTemperature"
                                            });
                        sensorsHere = true;
                    }
                    if (sensorsHere && !_cboNamespaces.Items.Contains(new { Display = "WMI", Value = "WMI" }))
                        _cboNamespaces.Items.Add(new { Display = "WMI", Value = "WMI" });
                } 
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + " happened when trying to get Temperature sensors from the MSAcpi_ThermalZoneTemperature object in WMI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _erroredNamespaces.Add("WMI");
                }
            }
            return _sensorList.Count > 0;
        }
    }
    [Serializable]
    public class TemperatureSensor
    {
        private ManagementObject _sensor;

        public TemperatureSensor(ManagementObject sensor)
        {
            _sensor = sensor;
        }

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(IDProperty))
                    return "";
                else
                    return (string)_sensor[IDProperty];
            }
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(NameProperty))
                    return "";
                else 
                    return (string) _sensor[NameProperty];
            }
        }

        public float Reading
        {
            get
            {
                if (string.IsNullOrEmpty(ReadingProperty))
                    return 0;
                else
                    return (float) _sensor[ReadingProperty];
            }
        }

        public float MinimumVal 
        { 
            get
            {
                if (string.IsNullOrEmpty(MinimumValProperty))
                    return 0;
                else
                    return (float)_sensor[MinimumValProperty];
            }
        }

        public float MaximumVal
        {
            get
            {
                if (string.IsNullOrEmpty(MaximumValProperty))
                    return 0;
                else
                    return (float)_sensor[MaximumValProperty];
            }
        }


        public string IDProperty { get; set; }
        public string NameProperty{ get; set; }
        public string ReadingProperty { get; set; }
        
        public string MinimumValProperty { get; set; }
        public string MaximumValProperty { get; set; }

        public string Namespace{get; set; }

        public string Classname { get;set;}
    }

}