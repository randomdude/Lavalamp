﻿using System;
using System.Runtime.InteropServices;

namespace transmitterDriver
{
    /// <summary>
    /// The PInvoke stuff to the actual DLL. This is wrapped by the pinvokeWrapper class and usually used
    /// from there.
    /// </summary>
    public abstract class pinvoke
    {
        [DllImport("lavalampdll.dll", EntryPoint = "initPort")]
        protected static extern bool initPort(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "getTestConfig")]
        protected static extern IntPtr getTestConfig_Unsafe();

        [DllImport("lavalampdll.dll", EntryPoint = "closePort")]
        protected static extern void closePort(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "isPortOpen")]
        protected static extern bool isPortOpen(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "syncNetwork")]
        protected static extern void syncNetwork(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdPing")]
        protected static extern IntPtr cmdPing_unsafe(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdReload")]
        protected static extern cmdResponseGeneric_t cmdReload(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdIdentify")]
        protected static extern IntPtr cmdIdentify_unsafe(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdCountSensors")]
        protected static extern IntPtr cmdCountSensors_unsafe(ref appConfig_t config);

        //[DllImport("lavalampdll.dll", EntryPoint = "cmdGetGenericDigitalSensor")]
        //protected static extern cmdResponseGeneric_t cmdGetGenericDigitalSensor(appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdSetGenericDigitalSensor")]
        protected static extern IntPtr cmdSetGenericDigitalSensor_unsafe(ref appConfig_t config, Byte tothis);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdGetGenericDigitalSensor")]
        protected static extern IntPtr cmdGetGenericDigitalSensor_unsafe(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdSetSensorFadeSpeed")]
        protected static extern cmdResponseGeneric_t cmdSetSensorFadeSpeed(ref appConfig_t config, byte tothis);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdGetSensorType")]
        protected static extern IntPtr cmdGetSensorType_unsafe(ref appConfig_t config);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdSetP")]
        protected static extern cmdResponseGeneric_t cmdSetP(ref appConfig_t config, byte byte1, byte byte2, bool isHigh);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdSetNodeId")]
        protected static extern cmdResponseGeneric_t cmdSetNodeId(ref appConfig_t config, byte tothis);

        [DllImport("lavalampdll.dll", EntryPoint = "cmdSetNodeKeyByte")]
        protected static extern cmdResponseGeneric_t cmdSetNodeKeyByte(ref appConfig_t config, byte index, byte newVal);

        [DllImport("lavalampdll.dll", EntryPoint = "geterrormessage")]
        protected static extern IntPtr getErrorMessage(int errorcode);

        [DllImport("lavalampdll.dll", EntryPoint = "cmd_free")]
        protected static extern void cmd_free(IntPtr stuff);

        [DllImport("lavalampdll.dll", EntryPoint = "injectFaultDesync")]
        protected static extern bool injectFaultDesync(ref appConfig_t config);
    }

    /* 	error codes below 0x20 are reserved for the low-level comms subsystem. Above is errors returned from the PIC
     *	Note that errors returned from the PIC are command-dependant and thus need parsing at the PC-side, depending
     *	which command they are returned from.
     */

    public enum errorcode_enum
    {
        errcode_none = 0x00, 
        errcode_timeout, 
        errcode_crypto, 
        errcode_internal, 
        errcode_portstate,

        errcode_sensor_not_found = 0x20, 
        errcode_sensor_wrong_type
    };

    /// <summary>
    /// Add any new sensor types to this enum, with the appropriate value.
    /// </summary>
    public enum sensorTypeEnum
    {
        generic_digital_in = 0x01,
        generic_digital_out = 0x02,
        pwm_out = 0x03,
        triac_out = 0x04,
        barLED = 0x05,
        mulitplexedLEDs = 0x06
    }
}