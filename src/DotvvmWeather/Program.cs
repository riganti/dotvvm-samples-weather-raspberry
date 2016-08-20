using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Web;
using DotvvmWeather.Services;
using Microsoft.Owin.Hosting;

namespace DotvvmWeather
{
    public class Program
    {

        private static Timer timer;

        public static WeatherDataService WeatherService { get; private set; }
        public static string ApplicationDirectory { get; private set; }

        public static string BaseUrl { get; private set; }

        static Program()
        {
            BaseUrl = "http://*:60000";
            ApplicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WeatherService = new WeatherDataService();
            timer = new Timer(OnTimerTick, null, 0, 10000);
        }


        public static void Main(string[] args)
        {
            // run in service mode
            if (args.Length > 0 && args[0] == "--service")
            {
                ServiceBase.Run(new ServiceBase[] { new HostingService() });
                return;
            }

            // start the OWIN host
            try
            {
                Console.WriteLine($"DotVVM Self-Host running on {BaseUrl}");
                Console.WriteLine("Press X to quit...");

                using (WebApp.Start<Startup>(BaseUrl))
                {
                    ConsoleKeyInfo c;
                    do
                    {
                        c = Console.ReadKey();
                    }
                    while (c.KeyChar != 'x' && c.KeyChar != 'X');
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running server: " + ex.Message);
            }
        }


        private static void OnTimerTick(object state)
        {
            // refresh data
            WeatherService.GatherAndStoreNewData();
        }

    }
}