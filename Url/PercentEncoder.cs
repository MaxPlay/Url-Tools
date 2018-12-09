using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools
{
    public static class PercentEncoder
    {
        public static bool IsValid(string percentEncodedBytes)
        {
            if (percentEncodedBytes.Length != 3)
                return false;

            if (percentEncodedBytes[0] != '\u0025')
                return false;

            return CodePoints.IsAsciiHexDigit(percentEncodedBytes[1]) && CodePoints.IsAsciiHexDigit(percentEncodedBytes[2]);
        }

        public static string DecodePercent(string input)
        {
            return Encoding.UTF8.GetString(DecodePercent(Encoding.UTF8.GetBytes(input)));
        }

        public static byte[] DecodePercent(byte[] input)
        {
            List<byte> output = new List<byte>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != '\x25')
                    output.Add(input[i]);
                else if (!(input.Length > i + 2 && CodePoints.IsAsciiHexDigit(input[i + 1]) && CodePoints.IsAsciiHexDigit(input[i + 2])))
                    output.Add(input[i]);
                else
                {
                    char bytePoint = HexCharsToValue(input[i + 1], input[i + 2]);
                    output.Add(Encoding.UTF8.GetBytes(new char[] { bytePoint })[0]);
                    i += 2;
                }
            }
            return output.ToArray();
        }

        private static char HexCharsToValue(byte hvalue, byte lvalue)
        {
            return (char)(ToHValue(hvalue) | ToLValue(lvalue));
        }

        /// <summary>
        /// Returns the hvalue representation of an ASCII encoded hex-digit.
        /// </summary>
        /// <remarks>
        /// A hex digit is a representation of half a byte of memory, meaning two hex digits can describe the value of a byte.
        /// It is usually split up into the hvalue (higher value) and lvalue (lower value), each displayed as a single hex digit.
        /// </remarks>
        /// <example>
        /// The string %2B is composed of an hvalue 2 and an lvalue B. In code they could be written as 0x2b and stored in a single byte.
        /// When Decoded into an ASCII character, it is displayed as "+" which is 43 in decimal and 2B in hexadecimal.
        /// </example>
        /// <param name="input">The ASCII representation of a hex digit that needs conversion.</param>
        /// <returns>The given input as hvalue.</returns>
        private static int ToHValue(byte input)
        {
            return ToLValue(input) << 4;
        }

        /// <summary>
        /// Returns the lvalue representation of an ASCII encoded hex-digit.
        /// </summary>
        /// <remarks>
        /// A hex digit is a representation of half a byte of memory, meaning two hex digits can describe the value of a byte.
        /// It is usually split up into the hvalue (higher value) and lvalue (lower value), each displayed as a single hex digit.
        /// </remarks>
        /// <example>
        /// The string %2B is composed of an hvalue 2 and an lvalue B. In code they could be written as 0x2b and stored in a single byte.
        /// When Decoded into an ASCII character, it is displayed as "+" which is 43 in decimal and 2B in hexadecimal.
        /// </example>
        /// <param name="input">The ASCII representation of a hex digit that needs conversion.</param>
        /// <returns>The given input as lvalue.</returns>
        private static int ToLValue(byte input)
        {
            switch (input)
            {
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                    return input - 48;
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                    return input - 55;
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                    return input - 87;
            }
            return input;
        }

        public static string EncodePercent(byte input)
        {
            char value = Encoding.UTF8.GetChars(new byte[] { input })[0];
            return EncodePercent(value);
        }

        public static string EncodePercent(char input)
        {
            if (!IsInEncodeSet(input))
                return input.ToString();

            //Since we only explicitly escape ASCII characters, we can safely parse the input character into a single byte.
            byte[] parsing = new byte[2];
            parsing[0] = FromHValue((byte)input);
            parsing[1] = FromLValue((byte)input);
            char[] output = Encoding.UTF8.GetChars(parsing);
            return $"%{output[0]}{output[1]}";
        }

        /// <summary>
        /// Checks whether a given character is in one of the percent-encode sets.
        /// </summary>
        /// <param name="input">The character to check</param>
        /// <returns>Returns true if the given character is in one of the percent-encode sets.</returns>
        public static bool IsInEncodeSet(char input)
        {
            return InC0EncodeSet(input) ||
                InFragmentEncodeSet(input) ||
                InPathEncodeSet(input) ||
                InUserinfoEncodeSet(input);
        }

        public static bool InUserinfoEncodeSet(char input)
        {
            switch (input)
            {
                case '\u002f':
                case '\u003a':
                case '\u003b':
                case '\u003d':
                case '\u0040':
                case '\u005b':
                case '\u005c':
                case '\u005d':
                case '\u005e':
                case '\u007c':
                    return true;
                default:
                    return false;
            }
        }

        public static bool InPathEncodeSet(char input)
        {
            switch (input)
            {
                case '\u0023':
                case '\u003f':
                case '\u007b':
                case '\u007d':
                    return true;
                default:
                    return false;
            }
        }

        public static bool InFragmentEncodeSet(char input)
        {
            switch(input)
            {
                case '\u0020':
                case '\u0022':
                case '\u003c':
                case '\u003e':
                case '\u0060':
                    return true;
                default:
                    return false;
            }
        }

        public static bool InC0EncodeSet(char input)
        {
            return CodePoints.IsC0Control(input) || input > '\u007e';
        }

        public static string EncodePercent(string input)
        {
            StringBuilder buffer = new StringBuilder();
            byte[] encodedInput = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < input.Length; i++)
                buffer.Append(EncodePercent(encodedInput[i]));
            return buffer.ToString();
        }

        /// <summary>
        /// Converts the lower 4 bits of the char into its hex representation in ASCII.
        /// </summary>
        private static byte FromLValue(byte value)
        {
            int lvalue = (value & 0x0f);
            switch (lvalue)
            {
                case 0x0:
                case 0x1:
                case 0x2:
                case 0x3:
                case 0x4:
                case 0x5:
                case 0x6:
                case 0x7:
                case 0x8:
                case 0x9:
                    return (byte)(lvalue + 48);
                case 0xA:
                case 0xB:
                case 0xC:
                case 0xD:
                case 0xE:
                case 0xF:
                    return (byte)(lvalue + 55);
            }
            return 0;
        }

        /// <summary>
        /// Converts the upper 4 bits of the char into its hex representation in ASCII.
        /// </summary>
        private static byte FromHValue(byte value)
        {
            int hvalue = value & 0xf0;
            return FromLValue((byte)(hvalue >> 4));
        }
    }
}
