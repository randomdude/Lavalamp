using System;

namespace ruleEngine
{
    public class ToolboxRule : Attribute { }

    public class ToolboxRuleCategoryAttribute : Attribute
    {
        internal readonly string name;
        public ToolboxRuleCategoryAttribute(string newName)
        {
            name = newName;
        }
    }
}