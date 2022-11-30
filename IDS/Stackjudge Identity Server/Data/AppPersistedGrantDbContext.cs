using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Stackjudge_Identity_Server.Data
{
    public class AppPersistedGrantDbContext : DbContext
    {
        public AppPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : base(options)
        {
        }
    }
}
