using System;
using System.Management;
using System.Security;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems.windows.WMI
{
    [Serializable]
    public abstract class WMIOptions : ICloneable
    {
        public WMIOptions()
        {
            computer = "localhost";
        }
        [XmlIgnore]
        public ConnectionOptions conOpts = new ConnectionOptions();

        protected SecureString _password = new SecureString();


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
      

        public delegate void scopeOptionChanged(ConnectionOptions conn);
        public event scopeOptionChanged onScopeOptsChanged;

        public ManagementScope openScope()
        {
            string wmiPath;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(computer))
                wmiPath = @"root\cimv2";
            else
                wmiPath = string.Format(@"\\{0}\root\cimv2", computer);

            ManagementScope scope = new ManagementScope(wmiPath, conOpts);

            return scope;
        }

        public void InvokeScopeOptionsChanged(object sender , EventArgs eventArgs)
        {
            try
            {
                if (onScopeOptsChanged != null)
                    onScopeOptsChanged.Invoke(conOpts);
            }
            catch (Exception)
            {
                //  
            }

        }
    }
}
