using System;
using netGui;
using transmitterDriver;

namespace virtualNodeNetwork
{
    public class genericDigitalOutSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.generic_digital_out; } }

        public override void setValue(int newValue)
        {
            // Nothing to do here.
        }
    }

    public class genericDigitalInSensor : virtualNodeSensor
    {
        private int val = 0;

        public override sensorTypeEnum type { get { return sensorTypeEnum.generic_digital_in; } }

        public override void setValue(int newValue)
        {
            val = newValue;
        }

        public override int getVal()
        {
            return val;
        }
    }

    public class pwmOutSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.pwm_out; } }
    }

    public class triacOutSensor : virtualNodeSensor
    {
        public override sensorTypeEnum type { get { return sensorTypeEnum.triac_out; } }
    }

}