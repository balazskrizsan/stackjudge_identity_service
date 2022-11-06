using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace StackjudgeIdentityServer.Data;

public class AppPersistedGrantDbContext : DbContext
{
    public AppPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : base(options)
    {
    }
}