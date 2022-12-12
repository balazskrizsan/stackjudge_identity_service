using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Options;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stackjudge_Identity_Server.Services
{
    public static class AppConfigService
    {
        public static void ConfigDbContext(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(GetConnectionString());
        }

        public static void ConfigFacebookOptions(FacebookOptions options)
        {
            options.AppId = "708557713838713";
            options.AppSecret = "d91ce749c2874c5dcacd57619f305865";
            options.Scope.Add("public_profile");
            options.Fields.Add("picture");

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    ClaimsIdentity identity = (ClaimsIdentity)context.Principal.Identity;

                    string profileImg = context.User
                        .GetProperty("picture")
                        .GetProperty("data")
                        .GetProperty("url")
                        .ToString();
                    string id = context.User.GetProperty("id").ToString();

                    identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                    identity.AddClaim(new Claim(JwtClaimTypes.Id, id));
                    identity.AddClaim(new Claim(JwtClaimTypes.AccessTokenHash, context.AccessToken));

                    return Task.CompletedTask;
                }
            };
        }

        public static void ConfigIdentityServer(IdentityServerOptions options)
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.EmitStaticAudienceClaim = true;
            options.Discovery.CustomEntries.Add("local_api", "/api/account/list");
        }

        public static void ConfigConfigurationStore(ConfigurationStoreOptions options)
        {
            options.ConfigureDbContext = b => b.UseNpgsql(
                GetConnectionString(),
                sql => sql.MigrationsAssembly(GetMigrationsAssembly())
            );
        }

        public static void ConfigOperationalStore(OperationalStoreOptions options)
        {
            options.ConfigureDbContext = b => b.UseNpgsql(GetConnectionString(),
                sql => sql.MigrationsAssembly(GetMigrationsAssembly())
            );
        }

        private static string GetMigrationsAssembly()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        }

        private static string GetConnectionString()
        {
            return AppSettingsService.Get()["psqlConnectionString"];
        }
    }
}
