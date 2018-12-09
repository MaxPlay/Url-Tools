using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G8G.UrlTools
{
    /// <summary>
    /// Provides static methods to verify characters as certain code points in the Unicode standard. <see cref="https://infra.spec.whatwg.org/#code-points"/> for the complete standard.
    /// </summary>
    public static class CodePoints
    {
        /// <summary>
        /// Returns true if the character is in range of the Unicode surrogates.
        /// </summary>
        /// <remarks>
        /// The Unicode standard permanently reserves the code point values from U+D800 to U+DFFF for the encoding of high and low surrogates.
        /// </remarks>
        /// <param name="input">The character to validate.</param>
        /// <returns></returns>
        public static bool IsSurrogate(char input)
        {
            return IsBetween(input, '\uD800', '\uDFFF');
        }

        public static bool IsAsciiHexDigit(byte input)
        {
            return IsAsciiHexDigit((char)input);
        }

        public static bool IsScalarValue(char input)
        {
            return !IsSurrogate(input);
        }

        public static bool IsNonCharacter(char input)
        {
            if (IsBetween(input, '\uFDD0', '\uFDEF'))
                return true;

            switch (input)
            {
                case '\uFFFE':
                case '\uFFFF':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAsciiCodePoint(char input)
        {
            return IsBetween(input, '\u0000', '\u007F');
        }

        public static bool IsTabOrNewline(char input)
        {
            switch (input)
            {
                case '\u0009':
                case '\u000A':
                case '\u000D':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsWhitespace(char input)
        {
            return IsSpace(input) || IsTabOrNewline(input);
        }

        public static bool IsSpace(char input)
        {
            return input == '\u0020';
        }

        public static bool IsC0Control(char input)
        {
            return IsBetween(input, '\u0000', '\u001F');
        }

        private static bool IsBetween(char input, char start, char end)
        {
            return input >= start && input <= end;
        }

        public static bool IsC0ControlOrSpace(char input)
        {
            return IsSpace(input) || IsC0Control(input);
        }

        public static bool IsControl(char input)
        {
            return IsC0Control(input) || IsBetween(input, '\u007F', '\u009F');
        }

        public static bool IsAsciiDigit(char input)
        {
            return IsBetween(input, '\u0030', '\u0039');
        }

        public static bool IsAsciiUpperHexDigit(char input)
        {
            if (IsAsciiDigit(input))
                return true;

            switch (input)
            {
                case '\u0041':
                case '\u0042':
                case '\u0043':
                case '\u0044':
                case '\u0045':
                case '\u0046':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAsciiLowerHexDigit(char input)
        {
            if (IsAsciiDigit(input))
                return true;

            switch (input)
            {
                case '\u0061':
                case '\u0062':
                case '\u0063':
                case '\u0064':
                case '\u0065':
                case '\u0066':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAsciiHexDigit(char input)
        {
            return IsAsciiLowerHexDigit(input) || IsAsciiUpperHexDigit(input);
        }

        public static bool IsAsciiUpperAlpha(char input)
        {
            return IsBetween(input, '\u0041', '\u005A');
        }

        public static bool IsAsciiLowerAlpha(char input)
        {
            return IsBetween(input, '\u0061', '\u007A');
        }

        public static bool IsAsciiAlpha(char input)
        {
            return IsAsciiUpperAlpha(input) || IsAsciiLowerAlpha(input);
        }

        public static bool IsAlphaNumeric(char input)
        {
            return IsAsciiDigit(input) || IsAsciiAlpha(input);
        }
    }
}
