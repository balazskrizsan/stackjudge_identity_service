using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackjudgeIdentityServer.Data.Entity;

namespace StackjudgeIdentityServer.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<ExtendedUser> ExtendedUsers { set; get; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}