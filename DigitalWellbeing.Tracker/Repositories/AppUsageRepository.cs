using DigitalWellbeing.Core.Data;
using DigitalWellbeing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalWellbeing.Tracker.Repositories
{
    public class AppUsageRepository : IAppUsageRepository
    {
        private readonly AppDbContext _context;

        public AppUsageRepository()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DigitalWellbeingDB;Trusted_Connection=True;")
    .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated(); // create DB if not exists
            
        }

        public void Add(AppUsage usage)
        {
            _context.AppUsages.Add(usage);
            _context.SaveChanges();
        }

        public List<AppUsage> GetAll()
        {
            return _context.AppUsages.ToList();
        }
    }
}