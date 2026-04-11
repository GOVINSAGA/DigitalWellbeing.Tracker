using Microsoft.EntityFrameworkCore;
using DigitalWellbeing.Core.Models;

namespace DigitalWellbeing.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUsage> AppUsages { get; set; }
    }
}