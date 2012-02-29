using System;

namespace ruleEngine
{
    public class ToolboxRule : Attribute { }

    public class ToolboxRuleCategoryAttribute : Attribute
    {
        internal readonly string name;
        internal readonly bool isNode;
        public ToolboxRuleCategoryAttribute(string newName, bool isItemNode = false)
        {
            isNode = isItemNode;
            name = newName;
        }
    }
}