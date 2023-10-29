using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherWear.Core.Models;
using WeatherWear.Exceptions;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Services.APIFetchers
{
    public class WeatherFetcher : IWeatherFetcher
    {
        private HttpClient _httpClient;
      
        public async Task<WeatherData> GetWeather(double latitude, double longitude)
        {
            try
            {
               
                string url = $"https://weatherapi-com.p.rapidapi.com/current.json?q={latitude}%2C{longitude}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "d2c7a7bd86msh82a34e2ff43497ep13923bjsne5a345122fee" },
                        { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
                    },
                };

                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        WeatherData extractedWeatherData = ExtractWeatherData(body);
                        return extractedWeatherData;
                    }
                    else
                    {
                        throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error while fetching weather data");
            }
            catch (JsonException ex)
            {
                throw new ApiException("Error while deserializing weather data");
            }
        }
        public void SetHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private WeatherData ExtractWeatherData(string json)
        {
            var rawWeatherData = JsonConvert.DeserializeObject<RawWeatherData>(json);

            var extractedWeatherData = new WeatherData
            {
                Temperature = rawWeatherData.Current.temp_c, // Temperature in Celsius
                Precipitation = rawWeatherData.Current.precip_mm, // Precipitation in millimeters
            };

            return extractedWeatherData;
        }

        private class RawWeatherData
        {
            public CurrentWeather Current { get; set; }

            public class CurrentWeather
            {
                public double temp_c { get; set; }
                public double precip_mm { get; set; }
            }
        }
    }
}
