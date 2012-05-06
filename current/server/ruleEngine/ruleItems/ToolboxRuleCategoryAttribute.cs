using System;

namespace ruleEngine
{
    public class ToolboxRule : Attribute { }

    public class ToolboxRuleCategoryAttribute : Attribute
    {
        public readonly string name;
        public ToolboxRuleCategoryAttribute(string newName)
        {
            name = newName;
        }
    }
}