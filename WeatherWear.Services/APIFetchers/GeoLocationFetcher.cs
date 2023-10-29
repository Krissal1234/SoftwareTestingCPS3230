using Newtonsoft.Json;
using WeatherWear.Exceptions;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Services
{
    public class GeoLocationFetcher : IGeoLocationFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly IBackupGeoLocationFetcher _backupGeoLocationFetcher;

        public GeoLocationFetcher(HttpClient httpClient, IBackupGeoLocationFetcher backupGeoLocationFetcher)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            _apiUrl = "http://ip-api.com/json/";
            _backupGeoLocationFetcher = backupGeoLocationFetcher;
        }

        public async Task<GeoLocation> GetGeolocation()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GeoLocation>(json);
                }
                else
                {       
                        return await _backupGeoLocationFetcher.GetGeolocation();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API request failed: " + ex.Message);
            }
        }

 
    }
}
