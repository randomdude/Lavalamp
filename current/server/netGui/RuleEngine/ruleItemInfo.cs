using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netGui.RuleEngine
{
    public class ruleItemInfo
    {
        public ruleItemType itemType;
        public Type RuleItemBaseType;
        public string pythonFileName;
        public string pythonCategory;
    }

    public enum ruleItemType
    {
        RuleItem, PythonFile
    }
}
