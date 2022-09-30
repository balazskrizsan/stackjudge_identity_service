using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stackjudge_Identity_Server.Data;
using Stackjudge_Identity_Server.Services;

namespace Stackjudge_Identity_Server
{
    public class Program
    {
        private static string CONNECTION_STRING =
            "Host=192.168.33.10;Database=stackjudge;Port=54322;Username=admin;Password=admin_pass;";

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
                    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

                    services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(CONNECTION_STRING); });

                    services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();
                    

                    services.AddAuthentication().AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = "6256044054421319";
                        facebookOptions.AppSecret = "02d0121396bc2c5f2a3e5713a620a7c2";
                        facebookOptions.Scope.Add("public_profile");
                        facebookOptions.Fields.Add("picture");

                        facebookOptions.Events = new OAuthEvents
                        {
                            OnCreatingTicket = context =>
                            {
                                ClaimsIdentity identity = (ClaimsIdentity)context.Principal.Identity;

                                string profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                                string id = context.User.GetProperty("id").ToString();

                                identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                                identity.AddClaim(new Claim(JwtClaimTypes.Id, id));
                                identity.AddClaim(new Claim(JwtClaimTypes.AccessTokenHash, context.AccessToken));

                                return Task.CompletedTask;
                            }
                        };
                    });

                    services.AddIdentityServer(options =>
                        {
                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseInformationEvents = true;
                            options.Events.RaiseFailureEvents = true;
                            options.Events.RaiseSuccessEvents = true;
                            options.EmitStaticAudienceClaim = true;
                        })
                        .AddAspNetIdentity<IdentityUser>()
                        .AddConfigurationStore(options =>
                        {
                            options.ConfigureDbContext = b => b.UseNpgsql(
                                CONNECTION_STRING,
                                sql => sql.MigrationsAssembly(migrationsAssembly)
                            );
                        })
                        .AddOperationalStore(options =>
                        {
                            options.ConfigureDbContext = b => b.UseNpgsql(CONNECTION_STRING,
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                        })
                        // .AddTestUsers(Config.Users)
                        .AddInMemoryClients(Config.Clients)
                        .AddInMemoryApiResources(Config.ApiResources)
                        .AddInMemoryApiScopes(Config.ApiScopes)
                        .AddInMemoryIdentityResources(Config.IdentityResources)
                        .AddProfileService<ProfileService<IdentityUser>>();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}