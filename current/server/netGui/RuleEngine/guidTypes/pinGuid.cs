using System;

namespace netGui.RuleEngine
{
    public class pinGuid 
    {
        public Guid id = Guid.Empty;

        public pinGuid() {}

        public pinGuid(string newGuid)
        {
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
    }
}