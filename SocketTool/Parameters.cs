using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool
{
    public class Parameters
    {
        public static string Host = GetConfig("Host", "127.0.0.1");
        public static int Port = Convert.ToInt32(GetConfig("Port", "8988"));
     
        public static int HeartInterval = Convert.ToInt32(GetConfig("HeartInterval", "40"));

        public static int BatchSocketCount = Convert.ToInt32(GetConfig("BatchSocketCount", "10"));
        public static int MaxSocketCount = Convert.ToInt32(GetConfig("MaxSocketCount", "1000"));
        public static int BatchSocketInterval = Convert.ToInt32(GetConfig("BatchSocketInterval", "30"));
        public static int IsDebug = Convert.ToInt32(GetConfig("IsDebug", "0"));
        private static string GetConfig(string key, string defautValue)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(configValue))
                return defautValue;
            return configValue;
        }
    }
}
