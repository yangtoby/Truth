using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Socket Tool";
            Console.WriteLine("Test is Running.....");
            Watcher watcher = new Watcher();
            watcher.Start();
            while (true)
            {
                var readStr = Console.ReadLine();
                if (readStr.Equals("t", StringComparison.CurrentCultureIgnoreCase))
                {
                    SocketCache.Instance.Statistic();
                }
                if (readStr.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }
        }
    }
}
