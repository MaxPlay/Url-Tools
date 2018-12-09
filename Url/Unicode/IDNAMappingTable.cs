using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using G8G.UrlTools.Extensions;

namespace G8G.UrlTools.Unicode
{
    public class IDNAMappingTable
    {
        CodePointEntry[] entries;

        public CodePointEntry GetEntry(char input)
        {
            CodePointEntry entry = new CodePointEntry();
            for (int i = 0; i < entries.Length; i++)
            {
                if(entries[i].Range.Contains(input))
                {
                    entry = entries[i];
                    break;
                }
            }

            return entry;
        }

        public void Load(Stream stream)
        {
            List<CodePointEntry> codePointEntries = new List<CodePointEntry>();
            StreamReader reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                CodePointEntry entry = new CodePointEntry();
                string input = reader.ReadLine();
                string[] result = GetTokens(input);

                if (result[1] == null)
                    continue;

                if (!entry.Range.Generate(result[0]))
                    continue;

                entry.Status = (CodePointStatus)Enum.Parse(typeof(CodePointStatus), result[1].FirstLetterToUpperCase());

                if (result[2] != null)
                {
                    entry.Mapping = new string(result[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select((s) =>
                    {
                        return (char)int.Parse(s.Trim(), System.Globalization.NumberStyles.HexNumber);
                    }).ToArray());

                    if (result[3] != null)
                        entry.IDNA2008Status = (IDNA2008Status)Enum.Parse(typeof(IDNA2008Status), result[3]);
                }
                else
                    entry.Mapping = string.Empty;

                codePointEntries.Add(entry);
            }
            entries = codePointEntries.ToArray();
        }

        private static string[] GetTokens(string input)
        {
            string[] result = new string[4];
            int target = 0;
            int start = 0;
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case ';':
                        result[target] = input.Substring(start, i - start).Trim();
                        target++;
                        start = i + 1;
                        break;
                    case '#':
                        result[target] = input.Substring(start, i - start).Trim();
                        return result;
                }
            }

            return result;
        }
    }
}
