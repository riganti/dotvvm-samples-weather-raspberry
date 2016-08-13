using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotvvmWeather.Services
{
    public class WeatherDataService
    {

        private readonly string logFilePath;
        private readonly string dataFilePath;
        private readonly BME280 bme280;

        private List<WeatherData> cache;
        private const int maxCacheSize = 24 * 3600 / 10;
        private readonly object locker = new object();

        public WeatherDataService()
        {
            logFilePath = Path.Combine(Program.ApplicationDirectory, "weather_log.txt");
            dataFilePath = Path.Combine(Program.ApplicationDirectory, "weather_data.dat");

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // run only on Raspberry PI
                bme280 = new BME280();
                bme280.Initialize();
            }
        }

        public void GatherAndStoreNewData()
        {
            try
            {
                var data = new WeatherData()
                {
                    Date = DateTime.Now,
                    Temperature = bme280.ReadTemperature(),
                    Pressure = bme280.ReadPreasure(),
                    Humidity = bme280.ReadHumidity()
                };

                // write to the cache
                lock (locker)
                {
                    if (cache != null)
                    {
                        cache.Add(data);
                        while (cache.Count > maxCacheSize)
                        {
                            cache.RemoveAt(0);
                        }
                    }
                }

                // write to the file
                using (var stream = new FileStream(dataFilePath, FileMode.Append, FileAccess.Write))
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(data.Date.Ticks);
                    writer.Write(data.Temperature);
                    writer.Write(data.Pressure);
                    writer.Write(data.Humidity);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now:s} WeatherDataService.GetNewData() error " + ex.Message + "\n\n");
            }
        }

        public List<WeatherData> GetData(int numberOfRecords, int skipRecords = 0)
        {
            lock (locker)
            {
                var results = new List<WeatherData>();

                if (cache == null)
                {
                    cache = ReadDataFromFile(maxCacheSize);
                }

                var i = Math.Max(0, cache.Count - numberOfRecords - skipRecords * (numberOfRecords - 1));
                while (i < cache.Count)
                {
                    results.Add(cache[i]);
                    i += skipRecords + 1;
                }
                return results;
            }
        }

        public WeatherData GetLastData()
        {
            lock (locker)
            {
                if (cache == null)
                {
                    cache = ReadDataFromFile(maxCacheSize);
                }

                if (cache.Count == 0)
                {
                    return new WeatherData()
                    {
                        Date = DateTime.Now
                    };
                }
                return cache[cache.Count - 1];
            }
        }

        private List<WeatherData> ReadDataFromFile(int numberOfRecords)
        {
            var entries = new List<WeatherData>();
            var recordLength = 8 + 4 + 4 + 4;

            try
            {
                using (var stream = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read))
                {
                    stream.Position = Math.Max(stream.Length - numberOfRecords * recordLength, 0);
                    numberOfRecords = (int) (stream.Length - stream.Position) / recordLength;

                    using (var reader = new BinaryReader(stream))
                    {
                        for (var i = 0; i < numberOfRecords; i++)
                        {
                            entries.Add(new WeatherData()
                            {
                                Date = new DateTime(reader.ReadInt64()),
                                Temperature = reader.ReadSingle(),
                                Pressure = reader.ReadSingle(),
                                Humidity = reader.ReadSingle()
                            });
                        }
                    }
                }
            }
            catch (IOException)
            {
            }

            return entries;
        }
    }
}
