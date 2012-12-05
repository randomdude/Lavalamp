using System;

namespace transmitterDriver
{
    public class sensorType
    {
        public readonly String friendlyType;
        public readonly sensorTypeEnum enumeratedType;

        public sensorType(sensorTypeEnum type)
        {
            enumeratedType = type;
            this.friendlyType = Enum.GetName(typeof (sensorTypeEnum), enumeratedType);
        }

        // Operator overloading stuff taken from MSDN. See "Guidelines for Overloading Equals() 
        // and Operator == (C# Programming Guide)"
        // http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(sensorType))
            {
                return false;
            }
            return Equals((sensorType)obj);
        }

        public static bool operator ==(sensorType a, sensorType b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return (a.enumeratedType == b.enumeratedType);
        }

        public static bool operator !=(sensorType a, sensorType b)
        {
            return !(a == b);
        }

        public bool Equals(sensorType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.friendlyType, this.friendlyType) && Equals(other.enumeratedType, this.enumeratedType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.friendlyType != null ? this.friendlyType.GetHashCode() : 0) * 397) ^ this.enumeratedType.GetHashCode();
            }
        }
    }
}