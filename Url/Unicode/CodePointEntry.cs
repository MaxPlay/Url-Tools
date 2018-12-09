using G8G.UrlTools.Exceptions;
using System;

namespace G8G.UrlTools.Unicode
{
    public struct CodePointEntry
    {
        public CodePointRange Range;
        public CodePointStatus Status;
        public string Mapping;
        public IDNA2008Status IDNA2008Status;

        public override string ToString()
        {
            if (Mapping.Length == 0)
                return $"{Range}; {Status}";
            if (IDNA2008Status == IDNA2008Status.None)
                return $"{Range}; {Status}; {Mapping}";
            return $"{Range}; {Status}; {Mapping}; {IDNA2008Status}";
        }
    }

    public enum IDNA2008Status
    {
        None,
        NV8,
        XV8
    }

    public enum CodePointStatus
    {
        Valid,
        Ignored,
        Mapped,
        Deviation,
        Disallowed,
        Disallowed_STD3_valid,
        Disallowed_STD3_mapped
    }

    public struct CodePointRange
    {
        public char Start;
        public char End;

        public bool Contains(char input)
        {
            return input >= Start && input <= End;
        }
        public override string ToString()
        {
            return (Start == End) ? Start.ToString() : $"{Start}..{End}";
        }

        public bool Generate(string input)
        {
            int rangeSeparator = input.IndexOf('.');
            string first = (rangeSeparator == -1) ? input : input.Substring(0, rangeSeparator);

            if (first.Length > 4)
                return false;

            Start = (char)int.Parse(first, System.Globalization.NumberStyles.HexNumber);
            if (rangeSeparator == -1)
            {
                End = Start;
                return true;
            }

            if (input.Length > 10)
                End = char.MaxValue;
            else
            {
                string second = input.Substring(6, 4);
                End = (char)int.Parse(second, System.Globalization.NumberStyles.HexNumber);
            }
            return true;
        }
    }
}