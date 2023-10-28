using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherWear.Exceptions;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Services
{
    public class GeoLocationFetcher : IGeoLocationFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public GeoLocationFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
            _apiUrl = "http://ip-api.com/json/";
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
                    // Create a new HttpClient for the backup request
                    using (var backupHttpClient = new HttpClient())
                    {
                        backupHttpClient.Timeout = TimeSpan.FromSeconds(3);
                        BackupGeoLocationFetcher backupGeoLocationFetcher = new BackupGeoLocationFetcher(backupHttpClient);
                        return await TryBackupGeoLocationFetcher(backupGeoLocationFetcher);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API request failed: " + ex.Message);
            }
        }

        private async Task<GeoLocation> TryBackupGeoLocationFetcher(BackupGeoLocationFetcher backupGeoLocationFetcher)
        {
            return await backupGeoLocationFetcher.GetGeolocation();
        }
    }
}
