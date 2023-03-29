using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackjudgeIdentityServer.Data;
using StackjudgeIdentityServer.Services;

namespace StackjudgeIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IExtensionGrantValidator, TokenExchangeGrantValidatorService>();
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