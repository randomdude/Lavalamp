namespace virtualNodeNetwork
{
    public class pingResponsePacket : challengeResponsePacket
    {
        public pingResponsePacket(int destId, int chalResponse) : base(destId, chalResponse) {}
    }
}