namespace DigitalWellbeing.Core.Models
{
    public class AppUsage
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
    }
}