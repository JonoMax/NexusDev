using Microsoft.EntityFrameworkCore;
using NexusDev.Models;

namespace NexusDev.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.CarId });
        }
    }
}
