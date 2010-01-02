using System;
using netbridge;

namespace netGui
{
    public class _transmitterDriver : transmitterDriver, ITransmitter
    {
        public _transmitterDriver(string strPortName, byte[] key) : base(strPortName, key) {}
    }
}
