using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Stackjudge_Identity_Server.Data
{
    public class AppConfigurationDbContext : DbContext
    {
        public AppConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
        }
    }
}
