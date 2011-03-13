using System.Text;

namespace virtualNodeNetwork
{
    public class challengeResponsePacket : networkPacket
    {
        public int payload
        {
            get { return toInt(rawBytes[7], rawBytes[6], rawBytes[5]); }
            set { fromInt(rawBytes, 7, value, 3); }
        }
        public int seq
        {
            get { return toInt(rawBytes[2], rawBytes[1], rawBytes[0]); }
            set { fromInt(rawBytes, 2, value, 3); }
        }
        public int unused
        {
            get { return toInt(rawBytes[3]); }
            set { rawBytes[3] = (byte) value; }
        }

        public challengeResponsePacket(int destId, int chalResponse)
            : base(destId)
        {
            seq = chalResponse;
            unused = 0;
        }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            toRet.Append("Dest ID 0x" + destinationNodeID.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("Seq number 0x" + seq.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("Payload bytes 0x" + payload.ToString("x").PadLeft(6, '0'));

            return toRet.ToString();
        }
    }
}