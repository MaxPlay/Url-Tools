using System;
using System.IO;
using System.Text;

namespace G8G.UrlTools.Unicode
{
    public static class IDNA
    {
        public static IDNAMappingTable MappingTable { get; private set; }

        static IDNA()
        {
            LoadMapping();
        }

        public static void LoadMapping()
        {
            MappingTable = new IDNAMappingTable();
            Stream stream = GetMappingTableContentAsStream();
            MappingTable.Load(stream);
        }

        private static Stream GetMappingTableContentAsStream()
        {
            Stream stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(Properties.Resources.IdnaMappingTable);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        //Based on https://www.unicode.org/reports/tr46/#Processing
        public static bool Unicode_ToASCII(
            string domain_name,
            bool CheckHyphens,
            bool CheckBidi,
            bool CheckJoiners,
            bool UseSTD3ASCIIRules,
            bool Transitional_Processing,
            bool VerifyDnsLength,
            out string result)
        {
            result = string.Empty;
            string mappedDomainName = Map(domain_name, Transitional_Processing, UseSTD3ASCIIRules, out bool error);
            if (error)
                return false;
            return true;
        }

        private static string Map(string domainName, bool TransitionalProcessing, bool UseStd3AsciiRules, out bool error)
        {
            error = false;
            StringBuilder buffer = new StringBuilder();
            foreach (char c in domainName)
            {
                CodePointEntry entry = MappingTable.GetEntry(c);
                switch (entry.Status)
                {
                    case CodePointStatus.Valid:
                        buffer.Append(c);
                        break;
                    case CodePointStatus.Ignored:
                        break;
                    case CodePointStatus.Mapped:
                        buffer.Append(entry.Mapping);
                        break;
                    case CodePointStatus.Deviation:
                        if (TransitionalProcessing)
                            buffer.Append(entry.Mapping);
                        else
                            buffer.Append(c);
                        break;
                    case CodePointStatus.Disallowed:
                        error = true;
                        return string.Empty;
                    case CodePointStatus.Disallowed_STD3_valid:
                        if (UseStd3AsciiRules)
                        {
                            error = true;
                            return string.Empty;
                        }
                        goto case CodePointStatus.Valid;
                    case CodePointStatus.Disallowed_STD3_mapped:
                        if (UseStd3AsciiRules)
                        {
                            error = true;
                            return string.Empty;
                        }
                        goto case CodePointStatus.Mapped;
                }
            }
            return buffer.ToString();
        }
    }
}