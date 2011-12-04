using System;

namespace ruleEngine
{
    public class ruleItemInfo
    {
        public ruleItemType itemType;
        public Type ruleItemBaseType;
        public string pythonFileName;
        public string pythonCategory;

        public ruleItemInfo(Type type)
        {
            itemType = ruleItemType.RuleItem;
            ruleItemBaseType = type;
        }

        public ruleItemInfo()
        {
        }
    }

    public enum ruleItemType
    {
        RuleItem, scriptFile
    }
}
