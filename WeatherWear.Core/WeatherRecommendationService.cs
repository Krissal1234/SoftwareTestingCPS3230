using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;

namespace WeatherWear.Core
{
    public class WeatherRecommendationService
    {
        private WeatherData _weatherData;

        public WeatherRecommendationService(WeatherData weatherData) 
        {
            _weatherData = weatherData;
        }

        public WeatherData.Weather GetWeatherType()
        {
            if(_weatherData.Temperature <= 15)
            {
                return WeatherData.Weather.ColdWeather;
            }
            else 
            { 
                return WeatherData.Weather.WarmWeather;
            }
        }

        public bool IsRaining()
        {
            return _weatherData.Precipitation > 0;
 
        }

    }
}
