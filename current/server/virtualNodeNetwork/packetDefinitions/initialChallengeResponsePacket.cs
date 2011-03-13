using System;
using System.Text;

namespace virtualNodeNetwork
{
    public class initialChallengeResponsePacket : challengeResponsePacket
    {
        public int challenge 
        { 
            get { return toInt(base.rawBytes[7], base.rawBytes[6], base.rawBytes[5]); }
            set { fromInt(rawBytes, 7, value, 3); }
        }

        public initialChallengeResponsePacket(int destID, int response, int destChallenge)
            : base(destID, response)
        {
            // set our response and new challenge bytes from the ints we have.
            challenge = destChallenge;
        }

        public override string ToString()
        {
            StringBuilder toRet = new StringBuilder();

            toRet.Append("Dest ID 0x" + destinationNodeID.ToString("x").PadLeft(2, '0') + "; ");
            toRet.Append("Challenge response 0x" + seq.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("New challenge 0x" + challenge.ToString("x").PadLeft(6, '0') + "; ");
            toRet.Append("Unused byte 0x" + unused.ToString("x").PadLeft(2, '0'));

            return toRet.ToString();
        }
    }
}