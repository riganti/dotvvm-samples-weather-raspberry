using System;

namespace DotvvmWeather.Services
{
    public class WeatherData
    {
        public float Temperature { get; set; }

        public float Pressure { get; set; }

        public float Humidity { get; set; }

        public DateTime Date { get; set; }
        
        public long DateFormatted => (long)(Date - new DateTime(1970, 1, 1)).TotalMilliseconds;


        public override string ToString()
        {
            return $"Temperature: {Temperature}, Pressure: {Pressure}, Humidity: {Humidity}";
        }
    }
}