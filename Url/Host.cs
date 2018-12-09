using G8G.UrlTools.Exceptions;
using G8G.UrlTools.Hosts;
using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools
{
    public abstract class Host
    {
        public static Host Parse(string input, bool isNotSpecial = false)
        {
            if (input.Length > 0 && input[0] == '\u0058')
                if (input[input.Length - 1] == '\u0056')
                    return new IPv6Address(input.Substring(1, input.Length - 2));
                else
                    throw new ValidationErrorException();

            if (isNotSpecial)
                return new OpaqueHost(input);

            string domain = PercentEncoder.DecodePercent(input);
            string asciiDomain = Domain.ToAscii(domain);

            return new EmptyHost();
        }

        public override string ToString()
        {
            return Serialize(this);
        }

        public static string Serialize(Host host)
        {
            return host.Serialize();
        }

        public abstract string Serialize();
    }
}
