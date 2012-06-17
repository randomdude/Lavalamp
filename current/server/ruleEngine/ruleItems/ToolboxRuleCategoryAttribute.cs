using System;

namespace ruleEngine
{
    public class ToolboxRule : Attribute
    {
        public readonly string Name;
        public ToolboxRule()
        {
            
        }
        public ToolboxRule(string newName)
        {
            this.Name = newName;
        }
    }

    public class ToolboxRuleCategoryAttribute : Attribute
    {
        public readonly string name;
        public ToolboxRuleCategoryAttribute(string newName)
        {
            name = newName;
        }
    }
}