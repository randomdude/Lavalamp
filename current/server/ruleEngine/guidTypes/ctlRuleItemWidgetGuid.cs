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

        public static bool operator ==(ctlRuleItemWidgetGuid x, ctlRuleItemWidgetGuid y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.id == y.id;
        }

        public static bool operator !=(ctlRuleItemWidgetGuid x, ctlRuleItemWidgetGuid y)
        {
            return !(x == y);
        }

        public bool Equals(ctlRuleItemWidgetGuid other)
        {
            if (ReferenceEquals(null , other)) return false;
            if (ReferenceEquals(this , other)) return true;
            return other.id.Equals(id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (ctlRuleItemWidgetGuid)) return false;
            return Equals((ctlRuleItemWidgetGuid) obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}