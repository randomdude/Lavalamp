using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WebService1;

namespace WebService1
{

    [XmlInclude(typeof(generic_digital_in))]
    [XmlInclude(typeof(generic_digital_out))]
    [XmlRoot]
    public class node
    {
        public node()
        {
            key = new char[16];
            sensor = new List<object>();
        }

        [XmlAttribute]
        public string name;

        public char[] key;
        public int id;

        public List<object> sensor;

        public String generateCode()
        {
            StringBuilder code = new StringBuilder();

            // Generate #define's for key
            int byteCount = 0;
            foreach (char keyByte in key)
            {
                code.AppendLine("#define KEY_" + Convert.ToString(byteCount, 16).ToUpper() + " 0x" + Convert.ToString(keyByte, 16));
                byteCount++;
            }
            code.AppendLine();

            // Generate #define for node ID
            code.AppendLine("#define NODEID 0x" + Convert.ToString(id, 16));
            code.AppendLine();

            code.AppendLine("#define SENSOR_COUNT " + sensor.Count);

            return code.ToString();
        }
    }
}