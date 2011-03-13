using System.Text;

namespace virtualNodeNetwork
{
    class initialChallengePacket : networkPacket
    {
        public initialChallengePacket(networkPacket packet) : base(packet) { }

        public int challenge { get { return toInt(base.rawBytes[2], base.rawBytes[1], base.rawBytes[0]); } }
        public int padding { get { return toInt(base.rawBytes[7], base.rawBytes[6], base.rawBytes[5]); } }
        public int unused { get { return toInt(base.rawBytes[3]); } }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            toRet.Append("Dest ID 0x" + destinationNodeID.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("Challenge 0x" + challenge.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("Random padding 0x" + padding.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("Unused byte 0x" + unused.ToString("x").PadLeft(2, '0'));

            return toRet.ToString();
        }
    }
}