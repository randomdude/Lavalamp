using System;
using System.Text;

namespace WebService1
{
    public class generic_digital_in : sensor
    {
        public ClassPin pin;

        new public String generateCode()
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine("#define SENSOR_" + id + "_TYPE SENSOR_ID_GENERIC_DIGITAL_IN");
            code.AppendLine(base.generateCode());
            code.AppendLine(pin.generateCode(base.id));

            return code.ToString();
        }
    }
}