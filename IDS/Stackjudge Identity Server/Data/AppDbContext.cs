using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stackjudge_Identity_Server.Data.Entity;

namespace Stackjudge_Identity_Server.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<ExtendedUser> ExtendedUsers { set; get; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}