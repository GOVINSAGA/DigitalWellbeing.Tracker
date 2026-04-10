using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DigitalWellbeing.Tracker.Models;
using DigitalWellbeing.Tracker.Helpers;

namespace DigitalWellbeing.Tracker.Services
{
    public class TrackingService
    {
        private string currentApp = "";
        private DateTime startTime;
        private List<AppUsage> usageList = new List<AppUsage>();

        public void StartTracking()
        {
            Console.WriteLine("Tracking started...\n");

            while (true)
            {
                var activeWindow = WindowHelper.GetActiveWindowTitle();

                if (!string.IsNullOrEmpty(activeWindow))
                {
                    var appName = WindowHelper.ExtractAppName(activeWindow);

                    if (string.IsNullOrEmpty(currentApp))
                    {
                        currentApp = appName;
                        startTime = DateTime.Now;
                    }
                    else if (currentApp != appName)
                    {
                        SaveSession();
                        currentApp = appName;
                        startTime = DateTime.Now;
                    }
                }

                Thread.Sleep(2000);
            }
        }

        private void SaveSession()
        {
            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalSeconds;

            var usage = new AppUsage
            {
                AppName = currentApp,
                StartTime = startTime,
                EndTime = endTime,
                DurationSeconds = duration
            };

            usageList.Add(usage);

            Console.WriteLine($"Saved: {currentApp} → {duration} sec");

            PrintSummary();
        }

        private void PrintSummary()
        {
            Console.WriteLine("\n--- Usage Summary ---");

            var summary = usageList
                .GroupBy(x => x.AppName)
                .Select(g => new
                {
                    AppName = g.Key,
                    TotalTime = g.Sum(x => x.DurationSeconds)
                });

            foreach (var item in summary)
            {
                Console.WriteLine($"{item.AppName} → {item.TotalTime} sec");
            }

            Console.WriteLine("---------------------\n");
        }
    }
}