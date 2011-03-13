namespace virtualNodeNetwork
{
    public class doIdentifyResponsePacket : challengeResponsePacket
    {
        public doIdentifyResponsePacket(int destId, int chalResponse) : base(destId, chalResponse) { }

        public byte nameByte0 { set { this.rawBytes[5] = value; } }
        public byte nameByte1 { set { this.rawBytes[6] = value; } }
        public byte nameByte2 { set { this.rawBytes[7] = value; } }
    }
}