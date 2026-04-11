using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DigitalWellbeing.Tracker.Helpers
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowTitle()
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

        public static string ExtractAppName(string windowTitle)
        {
            if (string.IsNullOrWhiteSpace(windowTitle))
                return "Unknown";

            // Case 1: Chrome-like titles
            if (windowTitle.Contains("-"))
            {
                var parts = windowTitle.Split('-');
                return parts[^1].Trim();
            }

            // Case 2: File paths
            if (windowTitle.Contains("\\"))
            {
                var parts = windowTitle.Split('\\');
                return parts[^1].Trim();
            }

            return windowTitle.Trim();
        }
    }
}