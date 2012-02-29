using System;
using System.Runtime.Serialization;
using transmitterDriver;


namespace netGui
{
    [Serializable]
    public class sensor : ISerializable 
    {
        private readonly Node _parentNode;

        public sensor()
        {
            // Because we can't talk to a sensor without knowing what node it's on
            throw new Exception("You need to initialize the Sensor class with a parent instance of the Node class");
        }

        protected sensor(SerializationInfo info, StreamingContext context)
        {
            id = info.GetInt16("ID");
            _cachedType = new sensorType((sensorTypeEnum) info.GetInt32("nodeType"));
            short parentNodeId = info.GetInt16("parentNodeID");
            string port = info.GetString("driverPort");
            bool encrypt = info.GetBoolean("driverEncryption");
            byte[] key = new byte[16];
            if (encrypt)
            {
                for (int i = 0; i < 16; i++)
                    key[i] = info.GetByte("key" + i);
            }
            _transmitter trans = new _transmitter(port, encrypt, key);
            _parentNode = new Node(trans, parentNodeId);

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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id);
            info.AddValue("nodeType", (int)type.enumeratedType);
            info.AddValue("parentNodeID", _parentNode.id);
            info.AddValue("driverPort", _parentNode.Mydriver.getPort());
            info.AddValue("driverEncryption",_parentNode.Mydriver.usesEncryption());
            if (_parentNode.Mydriver.usesEncryption())
            {
                byte[] key = _parentNode.Mydriver.getKey();
                for(int i = 0; i < 16; i++)
                    info.AddValue("key" + i,key[i]);
            }
        }
    }
}
