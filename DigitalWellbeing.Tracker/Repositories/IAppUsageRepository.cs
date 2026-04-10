using DigitalWellbeing.Tracker.Models;

namespace DigitalWellbeing.Tracker.Repositories
{
    public interface IAppUsageRepository
    {
        void Add(AppUsage usage);
        List<AppUsage> GetAll();
    }
}