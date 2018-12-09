using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools.Hosts
{
    public class IPv4Address : Host
    {
        private const char SEPARATOR = '\u002e';

        public IPv4Address(string input)
        {
        }

        public int Value { get; set; }

        public override string Serialize()
        {
            StringBuilder output = new StringBuilder();
            int n = Value;
            for (int i = 1; i <= 4; i++)
            {
                output.Append(n % 256);
                if (i != 4)
                    output.Append(SEPARATOR);
                n = (int)Math.Floor(n / 256.0);
            }
            return output.ToString();
            //return $"{GetValueByteOffset(24)}{SEPARATOR}{GetValueByteOffset(16)}{SEPARATOR}{GetValueByteOffset(8)}{SEPARATOR}{GetValueByteOffset(0)}";
        }

        private int GetValueByteOffset(int offset)
        {
            int modifiedValue = Value & (255 << offset);
            return modifiedValue >> offset;
        }
    }
}
