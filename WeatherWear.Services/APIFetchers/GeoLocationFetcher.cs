using Newtonsoft.Json;
using WeatherWear.Exceptions;
using WeatherWear.Models;

namespace WeatherWear.Services
{
    public class GeoLocationFetcher : IGeoLocationFetcher
    {
        private readonly HttpClient _httpClient;

        public GeoLocationFetcher(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(3);
        }

        public async Task<GeoLocation> GetGeolocation()
        {
            string apiUrl = "http://ip-api.com/json/";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GeoLocation>(json);
                }
                else
                {
                    // Use another service
                    return null;
                }
            }
            catch(HttpRequestException ex) 
            {
                throw new ApiException("API request failed: " + ex.Message);

            }
        }

    }
}
