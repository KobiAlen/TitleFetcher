using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TitleFetcher.Slave
{
    public static class AppConfig
    {
        private static IConfigurationRoot config;        

        static AppConfig()
        {
            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);

            config = builder.Build();
        }

        public static string ConnectionFactoryEndpoint { get => config.GetSection("Queue").GetValue<string>("ConnectionFactoryEndpoint"); }
    }
}
