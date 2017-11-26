using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool
{
    class Log
    {
        public static Logger logger = LogManager.GetLogger("DeamonTool");
        public static void LogError(string msg)
        {
            logger.Error(msg);
        }

        public static void LogInfo(string msg)
        {
            logger.Info(msg);
        }

        public static void LogFatal(string msg)
        {
            logger.Fatal(msg);
        }

    }
}
