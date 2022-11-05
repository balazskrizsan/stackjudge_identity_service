using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stackjudge_Identity_Server.Data;
using Stackjudge_Identity_Server.Services;

namespace Stackjudge_Identity_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
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
                        .AddInMemoryClients(Config.Clients)
                        .AddInMemoryApiResources(Config.ApiResources)
                        .AddInMemoryApiScopes(Config.ApiScopes)
                        .AddInMemoryIdentityResources(Config.IdentityResources)
                        .AddProfileService<ProfileService<IdentityUser>>();
                    services.AddLocalApiAuthentication();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}