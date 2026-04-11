using Microsoft.EntityFrameworkCore;
using DigitalWellbeing.API.Models;

namespace DigitalWellbeing.API.Data
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