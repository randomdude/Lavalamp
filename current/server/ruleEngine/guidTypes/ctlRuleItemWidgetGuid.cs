using System;

namespace ruleEngine
{
    public class ctlRuleItemWidgetGuid
    {
        public Guid id = Guid.Empty;

        public ctlRuleItemWidgetGuid() {}

        public ctlRuleItemWidgetGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }
}