using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;

namespace WeatherWear.Services.APIFetchers.Interfaces
{
    public interface IFutureWeatherFetcher
    {

        Task<WeatherData> GetWeather(string IATA, string date);
        void SetHttpClient(HttpClient httpClient);
    }
}
