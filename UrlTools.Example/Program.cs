using G8G.UrlTools;
using G8G.UrlTools.Hosts;
using G8G.UrlTools.Unicode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlTools.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = new IPv4Address("192:0:0:1")
            {
                Value = 1338961360
            };
            Console.WriteLine(ip);

            Console.ReadLine();
        }
    }
}
