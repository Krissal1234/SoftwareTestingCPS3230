using IPGeolocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Exceptions;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers.Interfaces;


namespace WeatherWear.Services.APIFetchers
{
    public class BackupGeoLocationFetcher : IGeoLocationFetcher
    {
        private readonly HttpClient _httpClient;
        public BackupGeoLocationFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GeoLocation> GetGeolocation()
        {
            try
            {
                string apiKey = "83c2e33f1d1a41669f9299ebfa8f1f8e";
                string apiUrl = $"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}&ip=1.1.1.1"; // Replace API_KEY with your actual API key


                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GeoLocation>(json);
                }
                else
                {
                    throw new HttpRequestException("API request failed");
                }

                // Handle other scenarios if needed
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API request failed: " + ex.Message);
            }
           
        }
    } 
}
  
    