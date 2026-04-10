using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

class Program
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    static string currentApp = "";
    static DateTime startTime;

    static List<AppUsage> usageList = new List<AppUsage>();

    static void Main()
    {
        Console.WriteLine("Tracking with aggregation started...\n");

        while (true)
        {
            string activeWindow = GetActiveWindowTitle();

            if (!string.IsNullOrEmpty(activeWindow))
            {
                string appName = ExtractAppName(activeWindow);

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

    static void SaveSession()
    {
        DateTime endTime = DateTime.Now;
        double duration = (endTime - startTime).TotalSeconds;

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

    static void PrintSummary()
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

    static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder buff = new StringBuilder(nChars);

        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, buff, nChars) > 0)
        {
            return buff.ToString();
        }

        return null;
    }

    static string ExtractAppName(string windowTitle)
    {
        if (windowTitle.Contains("-"))
        {
            string[] parts = windowTitle.Split('-');
            return parts[^1].Trim();
        }

        return windowTitle;
    }
}

// Model class
class AppUsage
{
    public string AppName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double DurationSeconds { get; set; }
}