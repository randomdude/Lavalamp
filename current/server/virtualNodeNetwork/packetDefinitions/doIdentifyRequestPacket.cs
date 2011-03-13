using System.Text;

namespace virtualNodeNetwork
{
    public class doIdentifyRequestPacket : commandPacket
    {
        public doIdentifyRequestPacket(networkPacket packet) : base(packet) { }

        public int byteOffset
        {
            get { return toInt(rawBytes[6]); }
        }

        public int unused
        {
            get { return toInt(rawBytes[7]); }
        }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            toRet.Append(base.ToString());
            toRet.Append("Parameter meanings: offset 0x" + byteOffset.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("unused 0x" + unused.ToString("x").PadLeft(2, '0'));

            return toRet.ToString();
        }
    }
}