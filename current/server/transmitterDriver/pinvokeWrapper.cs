using System.Runtime.InteropServices;

namespace transmitterDriver
{
    /// <summary>
    /// Wrappers to perform marshalling and fiddle about with type conversion
    /// </summary>
    public class pinvokeWrapper : pinvoke
    {
        protected static cmdResponseGeneric_t cmdPing(ref appConfig_t config)
        {
            return new cmdResponseGeneric_t(pinvoke.cmdPing_unsafe(ref config));
        }

        protected static cmdResponseIdentify_t cmdIdentify(ref appConfig_t config)
        {
            return new cmdResponseIdentify_t(pinvoke.cmdIdentify_unsafe(ref config));
        }

        protected static cmdResponseGeneric_t cmdCountSensors(ref appConfig_t config)
        {
            return new cmdResponseGeneric_t(pinvoke.cmdCountSensors_unsafe(ref config));
        }

        protected static cmdResponseGetSensorType_t cmdGetSensorType(ref appConfig_t config)
        {
            return new cmdResponseGetSensorType_t(pinvoke.cmdGetSensorType_unsafe(ref config));
        }

        protected static cmdResponseGeneric_t cmdSetGenericDigitalSensor(ref appConfig_t config, byte toThis)
        {
            return new cmdResponseGeneric_t(pinvoke.cmdSetGenericDigitalSensor_unsafe(ref config, toThis));
        }

        protected static cmdResponseGeneric_t cmdGetGenericDigitalSensor(ref appConfig_t config)
        {
            return new cmdResponseGeneric_t(pinvoke.cmdGetGenericDigitalSensor_unsafe(ref config));
        }

        public static appConfig_t getTestConfig()
        {
            return (appConfig_t)Marshal.PtrToStructure(pinvoke.getTestConfig_Unsafe(), typeof(appConfig_t));
        }

        public new static bool injectFaultDesync(ref appConfig_t config)
        {
            return pinvoke.injectFaultDesync(ref config);
        }
    }
}