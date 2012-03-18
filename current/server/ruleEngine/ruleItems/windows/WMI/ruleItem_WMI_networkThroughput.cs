using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Instrumentation;
namespace ruleEngine.ruleItems.windows.WMI
{
    public class ruleItem_WMI_networkThroughput : ruleItemBase
    {
        public override string ruleName()
        {
            throw new NotImplementedException();
        }

        public override void evaluate()
        {
            //base method sets up wmi
             //_wmiScope.Connect();
            
             SelectQuery query = new SelectQuery("Win32_Environment",
            "UserName=\"<SYSTEM>\"");
        }
    }
}
