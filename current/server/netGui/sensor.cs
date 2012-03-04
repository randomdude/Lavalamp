using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using transmitterDriver;


namespace netGui
{
    [Serializable]
    public class sensor : IXmlSerializable
    {
        private Node _parentNode;

        public sensor()
        {
        }

        public sensor(Node newParentNode)
        {
            _parentNode = newParentNode;
        }

        [XmlIgnore]
        public string name { get { return _parentNode.name + " : " + id; } }

        [XmlIgnore]
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

        public void setValue(object value, bool silently)
        {
            _parentNode.setValue(id, value, silently);
        }

        #region IXmlSerialisable memebrs
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            short parentNodeId = 0;
            string port = null;
            bool encrypt = false;
            byte[] key = new byte[16];
            bool readElement = false;

            while (!readElement)
            {
                switch (reader.Name)
                {
                    case "ID":
                        id = (short)reader.ReadElementContentAsInt();
                        break;
                    case "nodeType":
                        _cachedType = new sensorType((sensorTypeEnum)reader.ReadElementContentAsInt());
                        break;
                    case "parentNodeID":
                        parentNodeId = (short)reader.ReadElementContentAsInt();
                        break;
                    case "driverPort":
                        port = reader.ReadElementContentAsString();
                        break;
                    case "driverEncrypt":
                        encrypt = reader.ReadElementContentAsBoolean();
                        break;
                    case "key":
                        reader.ReadElementContentAsBinHex(key, 0, 16);
                        break;
                    case "selectedSensor":
                        if (reader.NodeType == XmlNodeType.EndElement)
                            readElement = true;
                        else
                            reader.Read();
                        break;
                    default:
                        reader.Read();
                        break;
                }
            }
            _parentNode = new Node(parentNodeId);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ID");
            writer.WriteValue(id);
            writer.WriteEndElement();
            writer.WriteStartElement("nodeType");
            writer.WriteValue((int)_cachedType.enumeratedType);
            writer.WriteEndElement();
            writer.WriteStartElement("parentNodeID");
            writer.WriteValue(_parentNode.id);
            writer.WriteEndElement();
            writer.WriteStartElement("driverPort");
            writer.WriteValue(_parentNode.Mydriver.getPort());
            writer.WriteEndElement();
            writer.WriteStartElement("driverEncrypt");
            writer.WriteValue(_parentNode.Mydriver.usesEncryption());
            writer.WriteEndElement();
            if (_parentNode.Mydriver.usesEncryption())
            {
                writer.WriteStartElement("key");
                writer.WriteBinHex(_parentNode.Mydriver.getKey(), 0, 16);
                writer.WriteEndElement();
            }
            _parentNode.Mydriver.Dispose();

        }
        #endregion
    }
}
