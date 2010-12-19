using netbridge;

namespace netGui
{
    public class _transmitterDriver : transmitterDriver, ITransmitter
    {
        public _transmitterDriver(string strPortName, bool useEncryption, byte[] key) : base(strPortName, useEncryption, key) { }
    }
}
