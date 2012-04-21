using System;

namespace ruleEngine
{
    public class lineChainGuid
    {
        public Guid id = Guid.NewGuid();

        public lineChainGuid() { }

        public lineChainGuid(string newGuid)
        {
            id = new Guid(newGuid);
        }

        public new string ToString()
        {
            return id.ToString();
        }

        public static bool operator ==(lineChainGuid x, lineChainGuid y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.id == y.id;
        }

        public static bool operator !=(lineChainGuid x, lineChainGuid y)
        {
            return !(x == y);
        }

        public bool Equals(lineChainGuid other)
        {
            if (ReferenceEquals(null , other)) return false;
            if (ReferenceEquals(this , other)) return true;
            return other.id.Equals(id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null , obj)) return false;
            if (ReferenceEquals(this , obj)) return true;
            if (obj.GetType() != typeof (lineChainGuid)) return false;
            return Equals((lineChainGuid) obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}