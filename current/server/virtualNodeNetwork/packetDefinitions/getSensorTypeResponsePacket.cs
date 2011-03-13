namespace virtualNodeNetwork
{
    public class getSensorTypeResponsePacket : challengeResponsePacket
    {
        public int errorCode { set { this.rawBytes[7] = (byte)value; } }
        public bool isErrored
        {
            set
            {
                if (value)
                    this.rawBytes[5] |= 0x01;
                else
                    this.rawBytes[5] &= 0xFE;  // FE being 0x11111110.
            }
        }

        public getSensorTypeResponsePacket(int destId, int chalResponse) : base(destId, chalResponse) { }
        
    }
}