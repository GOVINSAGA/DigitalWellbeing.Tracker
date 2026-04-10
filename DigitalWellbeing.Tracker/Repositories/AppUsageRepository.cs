using DigitalWellbeing.Tracker.Data;
using DigitalWellbeing.Tracker.Models;

namespace DigitalWellbeing.Tracker.Repositories
{
    public class AppUsageRepository : IAppUsageRepository
    {
        private readonly AppDbContext _context;

        public AppUsageRepository()
        {
            _context = new AppDbContext();
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