using System;

namespace netGui.RuleEngine
{
    public class ruleItemInfo
    {
        public ruleItemType itemType;
        public Type ruleItemBaseType;
        public string pythonFileName;
        public string pythonCategory;
    }

    public enum ruleItemType
    {
        RuleItem, scriptFile
    }
}
