using System.Text;

namespace virtualNodeNetwork
{
    public class commandPacket : networkPacket
    {
        public commandPacket(networkPacket packet) : base(packet) { }

        public int challengeResponse
        {
            get { return toInt(rawBytes[2], rawBytes[1], rawBytes[0]); }
            set { fromInt(rawBytes, 2, value, 3); }
        }
        public int commandByte { get { return toInt(base.rawBytes[5]); } }
        public int parameters
        {
            get { return toInt( rawBytes[6], rawBytes[7]); }
            set { fromInt(rawBytes, 7, value, 2); }
        }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            toRet.Append("Dest ID 0x" + destinationNodeID.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("Challenge response 0x" + challengeResponse.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("Command byte 0x" + commandByte.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("Command byte meaning " + findCommandByteMeaning() + "; ");
            toRet.Append("Command parameters 0x" + parameters.ToString("x").PadLeft(4, '0'));

            return toRet.ToString();
        }

        public commandByte findCommandByteMeaning()
        {
            switch (commandByte)
            {
                case 0x01:
                    return virtualNodeNetwork.commandByte.ping;
                case 0x02:
                    return virtualNodeNetwork.commandByte.identify;
                default:
                    return virtualNodeNetwork.commandByte.unknown;
            }
        }
    }
}