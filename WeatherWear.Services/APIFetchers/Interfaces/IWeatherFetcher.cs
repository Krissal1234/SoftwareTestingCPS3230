using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;

namespace WeatherWear.Services.APIFetchers.Interfaces
{
    public interface IWeatherFetcher
    {

        public Task<WeatherData> GetWeather(double latitude, double longitude);

    }
}
