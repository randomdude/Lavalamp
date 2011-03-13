namespace virtualNodeNetwork
{
    public class getSensorTypePacket : commandPacket
    {
        public getSensorTypePacket(networkPacket packet) : base(packet) { }

        public int sensorToInterrogate
        {
            get { return toInt(rawBytes[7]); }
        }
    }
}