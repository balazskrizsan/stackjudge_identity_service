using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class AppConfigurationDbContext : DbContext
    {
        public AppConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
        }
    }
}
