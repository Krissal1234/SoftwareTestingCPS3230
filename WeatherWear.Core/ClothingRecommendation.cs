using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WeatherWear.Core.Models;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Core
{
    public class ClothingRecommendation
    {
        private readonly IGeoLocationFetcher _geoLocationFetcher;
        private readonly IWeatherFetcher _weatherFetcher;
        private WeatherRecommendationService _weatherRecommendationService;

        public ClothingRecommendation(IGeoLocationFetcher geoLocationFetcher, IWeatherFetcher weatherFetcher)
        {
            _geoLocationFetcher = geoLocationFetcher;
            _weatherFetcher = weatherFetcher;
        }

        public async Task<string> CheckWeatherAsync()
        {
            try
            {
                GeoLocation loc = await GetGeolocationAsync();

                if (loc != null)
                {
                    WeatherData weatherData = await GetWeatherAsync(loc.lat, loc.lon);

                    bool isRaining = _weatherRecommendationService.IsRaining(weatherData);
                    bool isWarm = _weatherRecommendationService.GetWeatherType(weatherData) == WeatherData.Weather.WarmWeather;

                    string umbrellaMessage = isRaining ? "bring an umbrella." : "don't need an umbrella.";
                    string clothingMessage = isWarm ? "wear light clothing." : "wear warm clothing.";

                    string consoleMessage = $"It is {(isRaining ? "currently raining" : "not raining")}, so you should {umbrellaMessage}\nIt is {(isWarm ? "warm" : "cold")}, so you should {clothingMessage}";
                    return consoleMessage;
                }

            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // Log the exception
            }
            return string.Empty;
        }


        protected virtual async Task<GeoLocation> GetGeolocationAsync()
        {
            return await _geoLocationFetcher.GetGeolocation();
        }

        protected virtual async Task<WeatherData> GetWeatherAsync(double latitude, double longitude)
        {
            return await _weatherFetcher.GetWeather(latitude, longitude);
        }

        public void SetWeatherRecommendationService(WeatherRecommendationService weatherRecommendationService)
        {
            _weatherRecommendationService = weatherRecommendationService;
        }
    }
}
