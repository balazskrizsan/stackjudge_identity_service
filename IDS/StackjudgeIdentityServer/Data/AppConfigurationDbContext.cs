using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace StackjudgeIdentityServer.Data;

public class AppConfigurationDbContext : DbContext
{
    public AppConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
    {
    }
}