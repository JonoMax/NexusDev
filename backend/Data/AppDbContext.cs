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
    }
}
