﻿namespace netGui
{
    public class _transmitter : transmitterDriver.transmitter, ITransmitter
    {
        public _transmitter(string strPortName, bool useEncryption, byte[] key) : base(strPortName, useEncryption, key) { }
    }
}
