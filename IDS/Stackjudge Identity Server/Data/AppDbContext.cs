using Microsoft.EntityFrameworkCore;
using Stackjudge_Identity_Server.Models;

namespace Stackjudge_Identity_Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ExtendedUser> ExtendedUsers { set; get; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
