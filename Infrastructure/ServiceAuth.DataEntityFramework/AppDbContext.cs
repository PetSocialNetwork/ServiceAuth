using Microsoft.EntityFrameworkCore;
using ServiceAuth.Domain.Entities;

namespace ServiceAuth.DataEntityFramework
{
    public class AppDbContext : DbContext
    {
        DbSet<Account> Account => Set<Account>();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .OwnsOne(a => a.Email);
        }
    }
}
