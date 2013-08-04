namespace ruleEngine.guidTypes
{
    using System;
    using System.Diagnostics.Contracts;

    public class ctlRuleItemWidgetGuid
    {
        public Guid id = Guid.Empty;

        public ctlRuleItemWidgetGuid()
        {
            id = Guid.Empty;
        }

        public ctlRuleItemWidgetGuid(string newGuid)
        {
            Contract.Requires(newGuid != null);
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
            return other.id.Equals(this.id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (ctlRuleItemWidgetGuid)) return false;
            return this.Equals((ctlRuleItemWidgetGuid) obj);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    }
}