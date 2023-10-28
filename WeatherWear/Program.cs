
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherWear.Services;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IGeoLocationFetcher,GeoLocationFetcher>();
                    services.AddSingleton<IWeatherFetcher, WeatherFetcher>();
                });
    }
}