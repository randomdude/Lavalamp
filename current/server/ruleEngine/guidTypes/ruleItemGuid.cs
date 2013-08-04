using System;

namespace ruleEngine
{
    using System.Diagnostics.Contracts;

    public class ruleItemGuid 
    {
        public Guid id = Guid.Empty;

        public ruleItemGuid()
        {
            id = Guid.Empty;
        }

        public ruleItemGuid(string newGuid)
        {
            Contract.Requires(newGuid != null);
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }
        public static bool operator ==(ruleItemGuid x, ruleItemGuid y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.id == y.id;
        }

        public static bool operator !=(ruleItemGuid x, ruleItemGuid y)
        {
            return !(x == y);
        }

        public bool Equals(ruleItemGuid other)
        {
            if (ReferenceEquals(null , other)) return false;
            if (ReferenceEquals(this , other)) return true;
            return other.id.Equals(id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (ruleItemGuid)) return false;
            return Equals((ruleItemGuid) obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}