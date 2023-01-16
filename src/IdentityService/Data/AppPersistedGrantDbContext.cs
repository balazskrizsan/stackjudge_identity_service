using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class AppPersistedGrantDbContext : DbContext
    {
        public AppPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : base(options)
        {
        }
    }
}
