using System;
using System.Text;

namespace WebService1
{
    // Definition for a 'sensor'.
    public class sensor
    {
        public int id;

        public String generateCode()
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine();
            code.AppendLine("; config for sensor " + id);
            code.AppendLine("#define SENSOR_" + id + "_PRESENT");

            return code.ToString();
        }
    }

    public enum SensorTypes
    {
        none,
        generic_digital_in,
        generic_digital_out,
    }

    public enum enumPort
    {
        PORTA, PORTB, PORTC, PORTD, PORTE
    }

    public class ClassPin
    {
        public ClassPin() { }

        public ClassPin(enumPort initPort, int initPin)
        {
            pin = initPin;
            port = initPort;
        }

        public enumPort port;
        public enumPort tristate;
        public int pin;

        public string generateCode(int nodeid)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("#define SENSOR_" + nodeid + "_PORT " + port);
            code.AppendLine("#define SENSOR_" + nodeid + "_TRIS " + tristate);
            code.AppendLine("#define SENSOR_" + nodeid + "_PIN " + pin);

            return code.ToString();
        }
    }
}