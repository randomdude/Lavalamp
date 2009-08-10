using System;
using System.Text;

namespace WebService1
{
    public class generic_digital_out : sensor
    {
        public ClassPin pin;

        new public String generateCode()
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine(base.generateCode());
            code.AppendLine("#define SENSOR_" + id + "_TYPE SENSOR_ID_GENERIC_DIGITAL_OUT");
            code.AppendLine(pin.generateCode(base.id));
            return code.ToString();
        }
    }

}