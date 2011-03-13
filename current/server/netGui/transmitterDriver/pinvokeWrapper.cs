using System.Runtime.InteropServices;

namespace netGui
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

        public static appConfig_t getTestConfig()
        {
            return (appConfig_t)Marshal.PtrToStructure(pinvoke.getTestConfig_Unsafe(), typeof(appConfig_t));
        }
    }
}