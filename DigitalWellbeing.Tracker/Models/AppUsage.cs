namespace DigitalWellbeing.Tracker.Models
{
    public class AppUsage
    {
        public string AppName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
    }
}