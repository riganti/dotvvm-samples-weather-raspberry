using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        public static void Main(string[] args)
        {
            ApplicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WeatherService = new WeatherDataService();
            timer = new Timer(OnTimerTick, null, 0, 10000);

            // start the OWIN host
            try
            {
                var baseUrl = "http://*:60000";
                using (WebApp.Start<Startup>(baseUrl))
                {
                    ConsoleKeyInfo c;
                    do
                    {
                        Console.WriteLine($"DotVVM Self-Host running on {baseUrl}");
                        Console.WriteLine("Press X to quit...");
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