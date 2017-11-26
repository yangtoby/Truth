using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DeamonTool
{
    class Program
    {
        static void Main(string[] args) 
        {


           
            new Watcher().Start();
            while(true)
            {
                var readStr = Console.ReadLine();
                if (readStr.Equals("t", StringComparison.CurrentCultureIgnoreCase))
                {
                    SocketCache.Instance.Statistic();
                }
                if(readStr.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }
        }
    }
}
