using System;
using Microsoft.Extensions.Configuration;

namespace StackjudgeIdentityServer.Services;

public static class AppSettingsService
{
    private static IConfigurationRoot settings = null;
    
    public static IConfigurationRoot Get()
    {
        if (null != settings)
        {
            return settings;
        }
        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json")
            .AddEnvironmentVariables()
            .Build();
        
        return settings;
    }
}