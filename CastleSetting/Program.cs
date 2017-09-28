using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Collections.Generic;
using System.Configuration;

namespace CastleSetting
{
    class Program
    {
        static void Main(string[] args)
        {



            Class2.send("117657860@qq.com", "ddd", "test", null);
           // Console.WriteLine(val);
            Console.ReadKey();

        }

        private static void aa()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("Config/appsettings.json", false, true);
           var Configuration = builder.Build();

            var redisUrl = Configuration.GetSection("RedisUrl").Value;
            var host = Configuration.GetSection("AppHost").Value;
            var port = Convert.ToInt32(Configuration.GetSection("AppPort").Value);
            var debug = Convert.ToBoolean(Configuration.GetSection("Debug").Value);

          

            Console.WriteLine("listening on port " + port);
           
        }

        private static void readConfig()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("email.json");

            var config = builder.Build();

            var appConfig = new Root();
            config.GetSection("Root").Bind(appConfig);
        }
    }

        


    interface IConfig
    {
        List<TT> AA { get; set; }
    }

    public class Config:IConfig
    {
       public List<TT> AA { get; set; }
    }

    public class TT
    {

    }


}
