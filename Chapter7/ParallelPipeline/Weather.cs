using System;
using System.Threading.Tasks;

namespace ParallelPipeline
{
    class Weather
    { 
        public Weather(int temperatureCelcius, string city)
        {
            TemperatureCelcius = temperatureCelcius;
            City = city;
        }

        public int TemperatureCelcius { get; private set; }

        public string City { get; private set; }

        public override string ToString()
        {
            return string.Format("(City: {0}, Temp: {1}C)", City, TemperatureCelcius);
        }
    }

    public class WeatherUnavailableException : Exception
    {
        public WeatherUnavailableException(string city)
            : base(string.Format("Fail to get the weather for '{0}'", city))
        {
        }
    }

    internal static class WeatherService
    {
        public static Task<Weather> GetWeatherAsync(string city)
        {
            return Task.Run(
                async () =>
                {
                    await Task.Yield();

                    // Each task should take random amount of time
                    var interval = 1 + new Random(Guid.NewGuid().GetHashCode()).Next(7);
                    //var interval = city == "San Francisco" ? 1 : 5;

                    await Task.Delay(TimeSpan.FromSeconds(interval));

                    // Faking the temerature by city name length :)
                    var result = new Weather(city.Length, city);
                    return result;
                });
        }
    }

}