using Microsoft.EntityFrameworkCore;
using UrlSaver.Models;

namespace UrlSaver.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Url>()
                .HasKey(e => e.Id);

            modelBuilder
                .Entity<Url>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder
                .Entity<Url>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
