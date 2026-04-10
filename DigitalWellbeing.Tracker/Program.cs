using DigitalWellbeing.Tracker.Services;

class Program
{
    static void Main()
    {
        var tracker = new TrackingService();
        tracker.StartTracking();
    }
}