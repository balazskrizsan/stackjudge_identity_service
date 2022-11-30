using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    services.AddIdentityServer(options =>
                        {
                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseInformationEvents = true;
                            options.Events.RaiseFailureEvents = true;
                            options.Events.RaiseSuccessEvents = true;

                            options.EmitStaticAudienceClaim = true;
                        })
                        .AddTestUsers(Config.Users)
                        .AddInMemoryClients(Config.Clients)
                        .AddInMemoryApiResources(Config.ApiResources)
                        .AddInMemoryApiScopes(Config.ApiScopes)
                        .AddInMemoryIdentityResources(Config.IdentityResources);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
