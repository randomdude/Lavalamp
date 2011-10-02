using System;
using netGui;
using transmitterDriver;

namespace virtualNodeNetwork
{
    /// <summary>
    /// All virtual node sensors inherit from this class. 
    /// </summary>
    public abstract class virtualNodeSensor
    {
        public int id = 1;

        /// <summary>
        /// The type of this sensor. Override this in the each inheriting class.
        /// </summary>
        public abstract sensorTypeEnum type { get; }

        /// <summary>
        /// Create a sensor of the given enum type.
        /// </summary>
        /// <param name="typeToTest"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        public static virtualNodeSensor makeSensor(sensorTypeEnum typeToTest, int newID)
        {
            if (newID == 0)
            {
                // This is illegal.
                throw new NotImplementedException();
            }

            switch (typeToTest)
            {
                case sensorTypeEnum.generic_digital_in:
                    return new genericDigitalInSensor() { id = newID };
                case sensorTypeEnum.generic_digital_out:
                    return new genericDigitalOutSensor() { id = newID };
                case sensorTypeEnum.pwm_out:
                    return new pwmOutSensor() { id = newID };
                case sensorTypeEnum.triac_out:
                    return new triacOutSensor() { id = newID };
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual void setValue(int newValue)
        {
            throw new NotSupportedException();
        }

        public virtual int getVal()
        {
            throw new NotSupportedException();
        }
    }
}