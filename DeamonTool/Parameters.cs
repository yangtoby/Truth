using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DeamonTool
{
    public class Parameters
    {
        // public static string OpendoorUrl = "http://www.airtu.me/Rabbit/Rabbit.Server/api/Door/OpenDoor?cUserId=6726&doorId=3&locationX=31.241005&locationY=121.411453";
        public static string OpenDoorUrl = GetAppSettingConfig("OpenDoorUrl", "");
        // public static string WarningUrl = "http://115.159.71.54:10102/api/default/SendWarningMsgOne?title={0}&content={1}&remarks=请注意。";
        public static string WarningUrl = GetAppSettingConfig("WarningUrl", "");
        public static string RegisterCmd = @"@@@@00280001IDRQ0812345678083C04FF080000092D19FA";
        public static string HeartCmd = "@@@@00100001HBRQ092D1908";

        public static string Host = GetAppSettingConfig("Host", "127.0.0.1");
        public static int Port = Convert.ToInt32(GetAppSettingConfig("Port", "8988"));
        public static int BatchSocketCount = Convert.ToInt32(GetAppSettingConfig("BatchSocketCount", "10"));

        public static int HeartInterval = Convert.ToInt32(GetAppSettingConfig("HeartInterval", "40"));

        public static int MaxSocketCount = Convert.ToInt32(GetAppSettingConfig("MaxSocketCount", "1000"));

        public static int ConcurrentInterval = Convert.ToInt32(GetAppSettingConfig("ConcurrentInterval", "10"));

        public static string LogUrl = GetAppSettingConfig("LogUrl", "mongodb://localhost:27017");

        public static int IsDebug = Convert.ToInt32(GetAppSettingConfig("IsDebug", "0"));

        public static int IsLogMongo = Convert.ToInt32(GetAppSettingConfig("IsLogMongo", "0"));

        public static string SqlServerUrl = GetConConfig("SqlServerUrl", "");
        private static string GetAppSettingConfig(string key, string defaultValue)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;
            return configValue;
        }

        private static string GetConConfig(string name, string defaultValue)
        {
            var conName = ConfigurationManager.ConnectionStrings[name];
            if (conName != null)
            {
                var configValue = conName.ConnectionString;
                return configValue;
            }
            return defaultValue;
        }
    }
}
