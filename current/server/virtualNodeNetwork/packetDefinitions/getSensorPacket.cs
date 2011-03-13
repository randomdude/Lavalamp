namespace virtualNodeNetwork
{
    public class getSensorPacket : commandPacket
    {
        public getSensorPacket(networkPacket packet) : base(packet) { }

        public int sensorToInterrogate
        {
            get { return toInt(rawBytes[6]); }
        }

        public bool isGetSensorCount
        {
            get { return (sensorToInterrogate == 0); }
        }
    }
}