using System;

namespace ruleEngine
{
    using System.Diagnostics.Contracts;

    public class pinGuid
    {
        public Guid id { get; set; } 

        public pinGuid() { id= Guid.Empty;}

        public pinGuid(string newGuid)
        {
            Contract.Requires(newGuid != null);
            this.id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return this.id.ToString();
        }

        public static bool operator ==(pinGuid x, pinGuid y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.id == y.id;
        }

        public static bool operator !=(pinGuid x, pinGuid y)
        {
            return !(x == y);
        }

        public bool Equals(pinGuid other)
        {
            if (ReferenceEquals(null , other)) return false;
            if (ReferenceEquals(this , other)) return true;
            return other.id.Equals(id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (pinGuid)) return false;
            return Equals((pinGuid) obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}