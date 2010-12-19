using System;

namespace netGui.RuleEngine
{
    public class ruleItemGuid 
    {
        public Guid id = Guid.Empty;

        public ruleItemGuid() {}

        public ruleItemGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }
}