using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core;
using WeatherWear.Core.Models;
using WeatherWear.Models;
using WeatherWear.Services;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.ConsoleApp
{
    public class WeatherWearApplication
    {
        private readonly IGeoLocationFetcher _geoLocationFetcher;
        private readonly IBackupGeoLocationFetcher _backupGeoLocationFetcher;
        private readonly IWeatherFetcher _weatherFetcher;
        private readonly FutureWeatherFetcher _futureWeatherFetcher;
        private readonly ClothingRecommendation _clothingRecommendation;
        private readonly WeatherRecommendationService _weatherRecommendation;
        public WeatherWearApplication(IGeoLocationFetcher geoLocationFetcher, 
                                        IBackupGeoLocationFetcher backupGeoLocationFetcher, 
                                        IWeatherFetcher weatherFetcher,
                                        ClothingRecommendation clothingRecommendation,
                                        WeatherRecommendationService weatherRecommendationService,
                                        FutureWeatherFetcher futureWeatherFetcher)
        {
            _geoLocationFetcher = geoLocationFetcher;
            _backupGeoLocationFetcher = backupGeoLocationFetcher;
            _weatherFetcher = weatherFetcher;
            _clothingRecommendation = clothingRecommendation;
            _weatherRecommendation = weatherRecommendationService;
            _futureWeatherFetcher = futureWeatherFetcher;
        }

        public void StartApplication()
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                Console.Clear();
                Console.WriteLine("WeatherWear.com");
                Console.WriteLine("---------------");
                Console.WriteLine("1. Recommend clothing for current location");
                Console.WriteLine("2. Recommend clothing for future location");
                Console.WriteLine("3. Exit");
                Console.Write("Enter choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RecommendClothingForCurrentLocationAsync();
                            break;
                        case 2:
                            RecommendClothingForFutureLocation();
                            break;
                        case 3:
                            exitRequested = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task RecommendClothingForCurrentLocationAsync()
        {
            _clothingRecommendation.SetWeatherRecommendationService(_weatherRecommendation);
            Console.WriteLine(await _clothingRecommendation.CheckCurrentWeather());
            
        }

        private async void RecommendClothingForFutureLocation()
        {
            Console.WriteLine("Please enter the IATA code: ");
            string iataCode = Console.ReadLine();

            Console.WriteLine("Please enter the date of arrival (yyyy-MM-dd): ");
            string dateOfArrival = Console.ReadLine();
            _clothingRecommendation.SetFutureWeatherFetcher(_futureWeatherFetcher);
            Console.WriteLine(await _clothingRecommendation.CheckFutureWeather(iataCode, dateOfArrival));


        }

    }
}
