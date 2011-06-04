namespace virtualNodeNetwork
{
    public class getSensorPacket : commandPacket
    {
        public getSensorPacket(networkPacket packet) : base(packet) { }

        public int sensorToInterrogate
        {
            get { return toInt(rawBytes[7]); }
        }

        public bool isGetSensorCount
        {
            get { return (sensorToInterrogate == 0); }
        }
    }

    public class setSensorPacket : commandPacket
    {
        public setSensorPacket(networkPacket packet) : base(packet) { }

        public int sensorToInterrogate
        {
            get { return toInt(rawBytes[7]); }
        }

        public int newValue
        {
            get { return toInt(rawBytes[6]); }
        }
    }

}