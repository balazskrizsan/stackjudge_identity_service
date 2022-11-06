using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackjudgeIdentityServer.Data;
using StackjudgeIdentityServer.Services;

namespace StackjudgeIdentityServer
{
    public class Program
    {
        public static IConfigurationRoot Configuration;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json")
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("============================================================= App info");
            Console.WriteLine($"Env: {environment}");
            Console.WriteLine("=============================================================");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IAccountService, AccountService>();
                    services.AddDbContext<AppDbContext>(AppConfigService.ConfigDbContext);
                    services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();
                    services.AddAuthentication().AddFacebook(AppConfigService.ConfigFacebookOptions);
                    services.AddIdentityServer(AppConfigService.ConfigIdentityServer)
                        .AddAspNetIdentity<IdentityUser>()
                        .AddConfigurationStore(AppConfigService.ConfigConfigurationStore)
                        .AddOperationalStore(AppConfigService.ConfigOperationalStore)
                        .AddInMemoryClients(OidcConfig.Clients)
                        .AddInMemoryApiResources(OidcConfig.ApiResources)
                        .AddInMemoryApiScopes(OidcConfig.ApiScopes)
                        .AddInMemoryIdentityResources(OidcConfig.IdentityResources)
                        .AddProfileService<ProfileService<IdentityUser>>();
                    services.AddLocalApiAuthentication();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}