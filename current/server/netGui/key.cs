using System;
using System.Text;

namespace netGui
{
    public class key
    {
        public byte[] keyArray = new byte[16];

        public static byte hexCharToValChar(char lower, char higher)
        {
            // Take a string representation of a hex digit (ie, "0" through "F") and return a char set to the
            // proper value (ie, 0 through F).
            ASCIIEncoding myEncoding = new ASCIIEncoding();
            byte lowerVal = myEncoding.GetBytes(new char[] { lower })[0];
            byte higherVal = myEncoding.GetBytes(new char[] { higher })[0];

            // make lowercase
            lowerVal = (byte)(lowerVal | ((byte)0x20));
            higherVal = (byte)(higherVal | ((byte)0x20));

            if (lowerVal < myEncoding.GetBytes("a".ToCharArray())[0])
            {
                lowerVal = (byte)
                    (
                    lowerVal - myEncoding.GetBytes("0".ToCharArray())[0]
                    );
            }
            else
            {
                lowerVal = (byte)
                    (
                    lowerVal - myEncoding.GetBytes("a".ToCharArray())[0]
                    );
                lowerVal += 10;
            }

            if (higherVal < myEncoding.GetBytes("a".ToCharArray())[0])
            {
                higherVal = (byte)
                    (
                    higherVal - myEncoding.GetBytes("0".ToCharArray())[0]
                    );
            }
            else
            {
                higherVal = (byte)
                    (
                    higherVal - myEncoding.GetBytes("a".ToCharArray())[0]
                    );
                higherVal += 10;
            }

            byte toRet = (byte)(((Int16)lowerVal) | (Int16)(higherVal << 4));

            return toRet;
        }

        public new string ToString()
        {
            StringBuilder toReturn = new StringBuilder();

            foreach (byte thisByte in keyArray)
                toReturn.Append(thisByte.ToString("X2"));

            return toReturn.ToString();
        }

        public static byte[] parseKey(String parseThis)
        {
            String toParse = parseThis.Trim();
            byte[] toReturn = new byte[toParse.Length/2];

            foreach (char thisChar in parseThis.ToCharArray())
            {
                if (thisChar < '0')
                    throw new FormatException();
                if (thisChar > '9' && thisChar < 'A')
                    throw new FormatException();
                if (thisChar > 'Z' && thisChar < 'a')
                    throw new FormatException();
                if (thisChar > 'z')
                    throw new FormatException();
            }

            int charPos = 0;
            Char[] toConvert = toParse.ToCharArray();
            for (int keyPos = 0; keyPos < toReturn.Length; keyPos++)
            {
                Char higherVal = toConvert[charPos + 0];
                Char lowerVal = toConvert[charPos + 1];
                toReturn[keyPos] = hexCharToValChar(lowerVal, higherVal);
                charPos += 2;
            }

            return toReturn;
        }

        public void setKey(String parseThis)
        {
            String toParse = parseThis.Trim();

            if (toParse.Length != 32)
                throw new FormatException();

            this.keyArray = parseKey(parseThis);
        }
    }
}
