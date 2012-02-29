using System;

namespace transmitterDriver
{
    public class sensorType
    {
        public String FriendlyType;
        public sensorTypeEnum enumeratedType;

        public sensorType(sensorTypeEnum type)
        {
            enumeratedType = type;
            FriendlyType = Enum.GetName(typeof (sensorTypeEnum), enumeratedType);
        }

        // Operator overloading stuff taken from MSDN. See "Guidelines for Overloading Equals() 
        // and Operator == (C# Programming Guide)"
        // http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
                return false;

            // If parameter cannot be cast to Point return false.
            sensorType p = obj as sensorType;
            if (p == null)
                return false;

            // Return true if the fields match:
            return (this.enumeratedType == p.enumeratedType);
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
    }
}