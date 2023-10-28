using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;

namespace WeatherWear.Services.APIFetchers
{
    public class WeatherFetcher : IWeatherFetcher
    {
        public async Task<WeatherData> GetWeather(double latitude, double longitude)
        {
            try
            {
                var client = new HttpClient();
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

                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(body);
                        return weatherData;
                    }
                    else
                    {
                        throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new WeatherApiException("Error while fetching weather data", ex);
            }
            catch (JsonException ex)
            {
                throw new WeatherApiException("Error while deserializing weather data", ex);
            }
        }

    }
}
