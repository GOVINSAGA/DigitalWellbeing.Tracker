using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

class Program
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    static string currentApp = "";
    static DateTime startTime;

    static void Main()
    {
        Console.WriteLine("Tracking with time started...\n");

        while (true)
        {
            string activeWindow = GetActiveWindowTitle();

            if (!string.IsNullOrEmpty(activeWindow))
            {
                string appName = ExtractAppName(activeWindow);

                // First time initialization
                if (string.IsNullOrEmpty(currentApp))
                {
                    currentApp = appName;
                    startTime = DateTime.Now;
                }

                // App changed
                else if (currentApp != appName)
                {
                    DateTime endTime = DateTime.Now;
                    double duration = (endTime - startTime).TotalSeconds;

                    // Log previous app session
                    Console.WriteLine($"App: {currentApp}");
                    Console.WriteLine($"Start: {startTime}");
                    Console.WriteLine($"End: {endTime}");
                    Console.WriteLine($"Duration: {duration} sec\n");

                    // Switch to new app
                    currentApp = appName;
                    startTime = DateTime.Now;
                }
            }

            Thread.Sleep(2000);
        }
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

    // Basic cleanup of app name //
    static string ExtractAppName(string windowTitle)
    {
        if (windowTitle.Contains("-"))
        {
            string[] parts = windowTitle.Split('-');
            return parts[^1].Trim(); // last part (e.g., Chrome)
        }

        return windowTitle;
    }
}