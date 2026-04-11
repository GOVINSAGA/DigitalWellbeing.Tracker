using Microsoft.EntityFrameworkCore;
using DigitalWellbeing.Tracker.Models;

namespace DigitalWellbeing.Tracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUsage> AppUsages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer(
        "Server=(localdb)\\MSSQLLocalDB;Database=DigitalWellbeingDB;Trusted_Connection=True;");
    }
}