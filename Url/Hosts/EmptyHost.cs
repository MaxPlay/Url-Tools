using System;
using System.Collections.Generic;
using System.Text;

namespace G8G.UrlTools.Hosts
{
    public class EmptyHost : Host
    {
        public override string Serialize()
        {
            return string.Empty;
        }
    }
}
