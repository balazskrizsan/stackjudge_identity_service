using System;
using Microsoft.Extensions.Configuration;

namespace StackjudgeIdentityServer.Services;

public static class AppSettingsService
{
    private static IConfigurationRoot settings;

    public static IConfigurationRoot Get()
    {
        if (null != settings)
        {
            return settings;
        }

        var settingsBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var env = Environment.GetEnvironmentVariable("env");

        if (null != env)
        {
            var customEnv = $"appsettings.{env}.json";

            Console.WriteLine($"Loading custom environment variables: {customEnv}");

            settingsBuilder.AddJsonFile(customEnv);
        }

        settings = settingsBuilder.Build();

        return settings;
    }
}