using System;
using System.Management;
using System.Security;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems.windows.WMI
{
    [Serializable]
    public abstract class WMIOptions : BaseOptions, ICloneable
    {

        public WMIOptions()
        {
            computer = "localhost";
        }
        [XmlIgnore]
        public ConnectionOptions conOpts = new ConnectionOptions();

        protected SecureString _password = new SecureString();
        private string _wmiNamespace = "cimv2";


        [XmlElement]
        public string computer { get; set; }
        [XmlElement]
        public string username
        {
            get { return conOpts.Username; }
            set { conOpts.Username = value == "" ? null : value; }
        }
        [XmlElement]
        public string password
        {
            set { _password = password.ConvertToSecureString(); }
            get { return _password.ConvertToUnsecureString(); }
        }
        public abstract Control[] getCustomControls();

        public abstract void setCustomValues();

        public abstract void clearControls();

        public abstract object Clone();
      

        public delegate void scopeOptionChanged(ManagementScope conn);
        public event scopeOptionChanged onScopeOptsChanged;

        public ManagementScope openScope()
        {
            string wmiPath;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(computer))
                wmiPath = @"root\" + _wmiNamespace;
            else
                wmiPath = string.Format(@"\\{0}\root\" + _wmiNamespace, computer);

            ManagementScope scope = new ManagementScope(wmiPath, conOpts);

            return scope;
        }

        protected string WMINamespace { get { return _wmiNamespace; } }
        public bool changeWMINamespace(string newNameSpace)
        {
            if (newNameSpace == _wmiNamespace)
                return true;
            string oldNamespace = _wmiNamespace;
            try
            {
                _wmiNamespace = newNameSpace;
                ManagementScope scope = openScope();
           
                if (scope == null)
                    return false;
                scope.Connect();
                if (!scope.IsConnected)
                    return false;
                return true;
            }
            catch (Exception)
            {
                _wmiNamespace = oldNamespace;
                return false;
            }
        }
        public void InvokeScopeOptionsChanged(object sender, EventArgs eventArgs)
        {
                if (onScopeOptsChanged != null)
                    onScopeOptsChanged.Invoke(openScope());
        }

        public override string typedName
        {
            get
            {
                return "WMI";
            }
        }

    }
}
