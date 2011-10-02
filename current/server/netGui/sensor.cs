using System;
using transmitterDriver;


namespace netGui
{
    public class sensor
    {
        private readonly Node parentNode;

        public sensor()
        {
            // Because we can't talk to a sensor without knowing what node it's on
            throw new Exception("You need to initialise the Sensor class with a parent instance of the Node class");
        }

        public sensor(Node newParentNode)
        {
            parentNode = newParentNode;
        }

        public Int16 id;

        // We cache the sensorType, as it's highly unlikely to change.
        private sensorType cachedType = null;
        public sensorType type
        {
            get
            {
                if (null == cachedType)
                    cachedType = parentNode.doGetSensorType(this.id);

                return cachedType;
            }
        }

        public object getValue(bool silently)
        {
            return parentNode.updateValue(this.id, silently);
        }

    }
}
