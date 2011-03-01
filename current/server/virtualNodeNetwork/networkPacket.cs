using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace virtualNodeNetwork
{
    public class networkPacket
    {
        public const int lengthInBytes = 8;
        private const int byteIndexDestinationID = 4;

        protected byte[] rawBytes;
        public int destinationNodeID
        {
            get { return rawBytes[byteIndexDestinationID]; }
            protected set { rawBytes[byteIndexDestinationID] = (byte)value; }
        }

        protected networkPacket(networkPacket newPacket)
        {
            commonConstructorStuff(newPacket.rawBytes);
        }

        public networkPacket(byte[] newPacketInBytes)
        {
            commonConstructorStuff(newPacketInBytes);
        }

        protected networkPacket(int destID)
        {
            rawBytes = new byte[lengthInBytes];
            destinationNodeID = destID;
        }

        public void commonConstructorStuff(byte[] newPacketInBytes)
        {
            if (newPacketInBytes.Length != lengthInBytes)
                throw new ArgumentException();

            // Copy each byte individually in to a new buffer so that the caller can modify them later.
            rawBytes = new byte[lengthInBytes];
            for (int i = 0; i < lengthInBytes; i++)
                rawBytes[i] = newPacketInBytes[i];
        }

        public int toInt(byte rawByte)
        {
            return (int) rawByte;
        }

        public int toInt(byte a, byte b)
        {
            int toRet = 0;

            toRet |= a << 0;
            toRet |= b << 8;

            return toRet;
        }

        public int toInt(byte a, byte b, byte c)
        {
            int toRet = 0;

            toRet |= a <<  0;
            toRet |= b <<  8;
            toRet |= c << 16;

            return toRet;
        }

        /// <summary>
        /// Set an existing a byte[] to the given int of the given size.
        /// </summary>
        protected void fromInt(byte[] target, int offset, int toConvert, int size)
        {
            // We set down from the offset.

            for (int i = 0; i < size; i++)
                target[offset--] = (byte)((toConvert >> (i * 8)) & 0xff);
        }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            for (int i = 0; i < lengthInBytes; i++)
            {
                toRet.Append("0x" + rawBytes[i].ToString("x").PadLeft(2, '0'));
                if (i < lengthInBytes - 1)
                    toRet.Append(" ");
            }

            return toRet.ToString();
        }

        public void writeToPipe(Stream pipe)
        {
            pipe.Write(rawBytes, 0, lengthInBytes);
        }

    }
}