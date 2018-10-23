using System;
using System.Threading;

namespace D3NsCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var svc = new D3NsClient("project.db3");
            svc.Log += (sender, eventArgs) => Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {(eventArgs.Level + "").PadRight(7)} -- {eventArgs.Log}");
            svc.Start();
            try
            {
                Console.Title = "Direct DDNS - " + svc.Domain;
            }
            catch
            {
                //Do nothing
            }

            Console.WriteLine("Started.");

            if (args.Length > 0 && args[0].ToLower() == "--no-enter")
            {
                Console.WriteLine($"To stop, kill me, process id:{System.Diagnostics.Process.GetCurrentProcess().Id}");
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Press ENTER to stop.");
            Console.ReadLine();
            Console.WriteLine("Stopping...");
            svc.Stop();

            Console.WriteLine();
            Console.Write("Stopped. Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}