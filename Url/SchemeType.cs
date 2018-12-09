namespace G8G.UrlTools
{
    public enum SchemeType
    {
        Blob,
        Ftp,
        Gopher,
        Http,
        Https,
        Ws,
        Wss,
        File,
        Other
    }

    public static class SchemeTypeExtension
    {
        public static string GetSchemeLiteral(this SchemeType schemeType)
        {
            switch (schemeType)
            {
                case SchemeType.Blob:
                    return "blob";
                case SchemeType.Ftp:
                    return "ftp";
                case SchemeType.Gopher:
                    return "gopher";
                case SchemeType.Http:
                    return "http";
                case SchemeType.Https:
                    return "https";
                case SchemeType.Ws:
                    return "ws";
                case SchemeType.Wss:
                    return "wss";
                case SchemeType.File:
                    return "file";
                case SchemeType.Other:
                    return "null";
                default:
                    return null;
            }
        }

        /// <summary>
        /// TODO: Summary
        /// </summary>
        /// <param name="schemeType"></param>
        /// <returns></returns>
        public static ushort GetDefaultPort(this SchemeType schemeType)
        {
            switch (schemeType)
            {
                case SchemeType.Ftp:
                    return 21;
                case SchemeType.Gopher:
                    return 70;
                case SchemeType.Ws:
                case SchemeType.Http:
                    return 80;
                case SchemeType.Https:
                case SchemeType.Wss:
                    return 443;
                default:
                    return 0;
            }
        }
    }
}