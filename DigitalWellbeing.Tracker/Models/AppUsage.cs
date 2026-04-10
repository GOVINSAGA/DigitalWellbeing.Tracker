namespace DigitalWellbeing.Tracker.Models
{
    public class AppUsage
    {
        public int Id { get; set; } // ✅ PRIMARY KEY (REQUIRED)

        public string AppName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
    }
}