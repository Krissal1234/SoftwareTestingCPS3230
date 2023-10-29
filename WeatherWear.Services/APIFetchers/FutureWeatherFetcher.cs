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
        private readonly HttpClient _httpClient;
        public FutureWeatherFetcher(HttpClient client)
        {
            _httpClient = client;
        }
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
                        WeatherData extractedWeatherData = ExtractWeatherData(body,date);
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
            var rawWeatherData = JsonConvert.DeserializeObject<RawWeatherData>(json);
            var forecastData = rawWeatherData.Forecast?.ForecastDay.Find(forecast => forecast.Date == targetDate);

            if (forecastData == null)
            {
                return null;
            }

            return new WeatherData
            {
                Temperature = forecastData.Day.Temp_c,
                Precipitation = forecastData.Day.Precip_mm,
            };
        }

        private class RawWeatherData
        {
            public ForecastData Forecast { get; set; }
        }

        private class ForecastData
        {
            public List<ForecastDay> ForecastDay { get; set; }
        }

        private class ForecastDay
        {
            public string Date { get; set; }
            public DayData Day { get; set; }
        }

        private class DayData
        {
            public double Temp_c { get; set; }
            public double Precip_mm { get; set; }
        }

    }
}
