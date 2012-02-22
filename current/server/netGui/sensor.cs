using System;
using transmitterDriver;


namespace netGui
{
    public class sensor
    {
        private readonly Node _parentNode;

        public sensor()
        {
            // Because we can't talk to a sensor without knowing what node it's on
            throw new Exception("You need to initialise the Sensor class with a parent instance of the Node class");
        }

        public sensor(Node newParentNode)
        {
            _parentNode = newParentNode;
        }

        public string name { get { return _parentNode.name + " : " + id; } }

        public Int16 id;

        // We cache the sensorType, as it's highly unlikely to change.
        private sensorType _cachedType;
        public sensorType type
        {
            get
            {
                if (null == _cachedType)
                    _cachedType = _parentNode.doGetSensorType(id);

                return _cachedType;
            }
        }

        public object getValue(bool silently)
        {
            return _parentNode.updateValue(id, silently);
        }

        public void setValue(object value,bool silently)
        {
            _parentNode.setValue(id,value,silently);
        }

    }
}
