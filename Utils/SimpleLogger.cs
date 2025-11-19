using System;

namespace RamanSoftwareV2.Utils
{
    public static class SimpleLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
    }
}
