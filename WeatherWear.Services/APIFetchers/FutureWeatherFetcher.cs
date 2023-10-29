using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;
using WeatherWear.Exceptions;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Services.APIFetchers
{
    public class FutureWeatherFetcher
    {
        private HttpClient _httpClient;

        public async Task<WeatherData> GetWeather(string IATA, string date)
        {
            if (!IsValidDateFormat(date))
            {
                throw new ArgumentException("Date is not in the required format (YY-MM-dd).");
            }

            try
            {
                string url = $"https://weatherapi-com.p.rapidapi.com/forecast.json?q={IATA}&dt={date}";
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
                        WeatherData extractedWeatherData = ExtractWeatherData(body, date);
                        return extractedWeatherData;
                    }
                    else
                    {
                        throw new HttpRequestException("API request failed");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException("Error while fetching weather data");
            }
        }

        public bool IsValidDateFormat(string date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }


        private WeatherData ExtractWeatherData(string json, string targetDate)
        {
            var data = JsonConvert.DeserializeObject<Root>(json);

            if (data.forecast != null && data.forecast.forecastday.Length > 0)
            {
                // Extracting precipitation and temperature for the first forecast day
                float totalPrecipitation = data.forecast.forecastday[0].day.totalprecip_mm;
                float averageTemperature = data.forecast.forecastday[0].day.avgtemp_c;

                return new WeatherData
                {
                    Temperature = averageTemperature,
                    Precipitation = totalPrecipitation
                };
            }
            else
            {
                throw new Exception("Invalid JSON structure or no forecast data found.");
            }
        }


        public void SetHttpClient(HttpClient client)
        {
            _httpClient = client;
        }

    }

    public class Root
    {
        public Forecast forecast { get; set; }
    }

    public class Forecast
    {
        public Forecastday[] forecastday { get; set; }
    }

    public class Forecastday
    {
        public Day day { get; set; }
    }

    public class Day
    {
        public float totalprecip_mm { get; set; }
        public float avgtemp_c { get; set; }
    }

}
