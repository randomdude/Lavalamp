using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ruleEngine.ruleItems.windows.WMI
{
    [Serializable]
    public abstract class WMIOptions
    {
        public WMIOptions()
        {
            computer = "localhost";
        }
        public ConnectionOptions conOpts = new ConnectionOptions();
        public string computer { get; set; }
        public string username
        {
            get { return conOpts.Username; }
            set { conOpts.Username = value; }
        }

        public string password
        {
            set 
            { 
               SecureString password = new SecureString();
                foreach(char p in value)
                    password.AppendChar(p);
                conOpts.SecurePassword = password;
            }
        }

        public abstract Control[] getCustomControls();

        public abstract void setCustomControl(Control ctl);

        public abstract void clearControls();


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

    }
}
