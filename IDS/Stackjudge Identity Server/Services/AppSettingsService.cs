using System;
using Microsoft.Extensions.Configuration;

namespace Stackjudge_Identity_Server.Services
{
    public static class AppSettingsService
    {
        private static IConfigurationRoot settings;

        public static IConfigurationRoot Get()
        {
            if (null != settings)
            {
                return settings;
            }

            var env = Environment.GetEnvironmentVariable("env");

            var customEnv = $"appsettings.{env}.json";

            Console.WriteLine($"Loading custom environment variables: {customEnv}");

            settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile(customEnv, optional: true)
                .AddEnvironmentVariables()
                .Build();

            return settings;
        }
    }
}
