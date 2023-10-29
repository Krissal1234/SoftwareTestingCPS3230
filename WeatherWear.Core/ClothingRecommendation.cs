using WeatherWear.Core.Models;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Core
{
    public class ClothingRecommendation
    {
        private readonly IGeoLocationFetcher _geoLocationFetcher;
        private readonly IWeatherFetcher _weatherFetcher;
        private WeatherRecommendationService _weatherRecommendationService;
        private readonly IBackupGeoLocationFetcher _backupGeoLocationFetcher;
        private IFutureWeatherFetcher _futureWeatherFetcher;

        public ClothingRecommendation(IGeoLocationFetcher geoLocationFetcher, IWeatherFetcher weatherFetcher)
        {  
            _geoLocationFetcher = geoLocationFetcher;
            _weatherFetcher = weatherFetcher;
            _weatherRecommendationService = new WeatherRecommendationService();
        }

        public void SetFutureWeatherFetcher(IFutureWeatherFetcher futureWeatherFetcher)
        {
            _futureWeatherFetcher = futureWeatherFetcher;
        }

        public async Task<string> CheckCurrentWeather()
        {
            try
            {
                GeoLocation loc = await GetGeolocationAsync();

                if (loc != null)
                {
                    WeatherData weatherData = await GetCurrentWeatherAsync(loc.lat, loc.lon);

                    return GenerateConsoleMessage(weatherData);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while trying to get weather data.");
            }
            return string.Empty;
        }

        public async Task<string> CheckFutureWeather(string IATA, string date)
        {
            try
            {
               _futureWeatherFetcher.SetHttpClient(new HttpClient());
               WeatherData weather =  await _futureWeatherFetcher.GetWeather(IATA, date);

                return GenerateConsoleMessage(weather);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while trying to get future weather data.");
               
            }
            return string.Empty;
        }

        private string GenerateConsoleMessage(WeatherData weatherData)
        {
            bool isRaining = _weatherRecommendationService.IsRaining(weatherData);
            bool isWarm = _weatherRecommendationService.GetWeatherType(weatherData) == WeatherData.Weather.WarmWeather;

            string umbrellaMessage = isRaining ? "bring an umbrella." : "don't need an umbrella.";
            string clothingMessage = isWarm ? "wear light clothing." : "wear warm clothing.";

            string consoleMessage = $"It is {(isRaining ? "currently raining" : "not raining")}, so you should {umbrellaMessage}\nIt is {(isWarm ? "warm" : "cold")}, so you should {clothingMessage}";
            return consoleMessage;
        }
        protected virtual async Task<GeoLocation> GetGeolocationAsync()
        {
            _geoLocationFetcher.SetHttpClient(new HttpClient());
            _geoLocationFetcher.SetBackupGeoLocationFetcher(new BackupGeoLocationFetcher());
            return await _geoLocationFetcher.GetGeolocation();
        }

        protected virtual async Task<WeatherData> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            _weatherFetcher.SetHttpClient(new HttpClient());    
            return await _weatherFetcher.GetWeather(latitude, longitude);
        }

        public void SetWeatherRecommendationService(WeatherRecommendationService weatherRecommendationService)
        {
            _weatherRecommendationService = weatherRecommendationService;
        }
    }
}
