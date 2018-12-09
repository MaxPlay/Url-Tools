using G8G.UrlTools.Exceptions;
using G8G.UrlTools.Unicode;
using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools.Hosts
{
    public class Domain : Host
    {
        public Domain()
        {
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }

        public static string ToAscii(string domain, bool beStrict = false)
        {
            if (IDNA.Unicode_ToASCII(domain, false, true, true, beStrict, false, beStrict, out string result))
                throw new ValidationErrorException();
            return result;
        }

        public static string ToUnicode(string domain)
        {
            return string.Empty;
        }
    }
}
