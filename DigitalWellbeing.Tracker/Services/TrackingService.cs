using DigitalWellbeing.Tracker.Helpers;
using DigitalWellbeing.Tracker.Models;
using DigitalWellbeing.Tracker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DigitalWellbeing.Tracker.Services
{
    public class TrackingService
    {
        private string currentApp = "";
        private DateTime startTime;
        
        private readonly IAppUsageRepository _repository;

        public TrackingService()
        {
            _repository = new AppUsageRepository();
        }
        public void StartTracking()
        {
            Console.WriteLine("Tracking started...\n");

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true; // prevent abrupt termination
                Console.WriteLine("\nStopping tracking...");
                SaveSession();
                Environment.Exit(0);
            };

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

            _repository.Add(usage); // 🔥 save to DB

            Console.WriteLine($"Saved to DB: {currentApp} → {duration} sec");

            PrintSummary();
        }

        private void PrintSummary()
        {
            Console.WriteLine("\n--- Usage Summary (DB) ---");

            var data = _repository.GetAll();

            var summary = data
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

            Console.WriteLine("--------------------------\n");
        }
    }
}