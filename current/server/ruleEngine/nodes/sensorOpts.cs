using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.nodes
{
    using ruleEngine.ruleItems;

    using transmitterDriver;

    public class pwm_speed
    {
        public Int16 fadespeed;

        public pwm_speed(Int16 newfadespeed)
        {
            fadespeed = newfadespeed;
        }
    }

    public class pwm_brightness
    {
        public Int16 fadeto;

        public pwm_brightness(Int16 newfadeto)
        {
            fadeto = newfadeto;
        }
    }

    public class SensorOptions : BaseOptions
    {
        public sensorTypeEnum thisSensorType;

        public sensor thisSensor;

        public SensorOptions()
        {
            
        }

        public SensorOptions(sensorTypeEnum genericDigitalIn, sensor connectedSensor)
        {
            thisSensorType = genericDigitalIn;
            thisSensor = connectedSensor;
        }


        public override string typedName
        {
            get
            {
                return "Sensor";
            }
        }

    }
}
