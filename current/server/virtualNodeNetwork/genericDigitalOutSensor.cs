using System;
using netGui;

namespace virtualNodeNetwork
{
    public class genericDigitalOutSensor : virtualNodeSensor
    {
        private int val = 0;

        public override sensorTypeEnum type { get { return sensorTypeEnum.generic_digital_out; } }

        public override void setValue(int newValue)
        {
            val = newValue;
        }
    }

    public class genericDigitalInSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.generic_digital_in; } }
        public override void setValue(int newValue)
        {
            throw new NotImplementedException();
        }
    }

    public class pwmOutSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.pwm_out; } }
        public override void setValue(int newValue)
        {
            throw new NotImplementedException();
        }
    }

    public class triacOutSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.triac_out; } }
        public override void setValue(int newValue)
        {
            throw new NotImplementedException();
        }
    }
}