namespace DigitalWellbeing.Tracker
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    class Program
    {
        // Get handle of active window
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        // Get window title
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        static void Main()
        {
            Console.WriteLine("Tracking started...\n");

            while (true)
            {
                string activeWindow = GetActiveWindowTitle();

                if (!string.IsNullOrEmpty(activeWindow))
                {
                    Console.WriteLine($"Active App: {activeWindow}");
                }

                Thread.Sleep(2000); // check every 2 seconds
            }
        }

        static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }

            return null;
        }
    }
}
