using System;

namespace ruleEngine
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
