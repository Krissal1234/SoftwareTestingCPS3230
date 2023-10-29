using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherWear.ConsoleApp;
using WeatherWear.Core;
using WeatherWear.Services;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Resolve the WeatherWearApplication using the service provider
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var weatherApp = services.GetRequiredService<WeatherWearApplication>();

                // Call the method to begin the application
                weatherApp.StartApplication();
            }

            // Run the host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddSingleton<IGeoLocationFetcher, GeoLocationFetcher>();
                    services.AddSingleton<IWeatherFetcher, WeatherFetcher>();
                    services.AddSingleton<FutureWeatherFetcher>();
                    services.AddSingleton<IBackupGeoLocationFetcher, BackupGeoLocationFetcher>();
                    services.AddSingleton<ClothingRecommendation>();
                    services.AddSingleton<WeatherRecommendationService>();

                    // Add WeatherWearApplication as a singleton service
                    services.AddSingleton<WeatherWearApplication>();
                });

    }
}
