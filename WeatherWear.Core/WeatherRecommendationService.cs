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
        public WeatherData.Weather GetWeatherType(WeatherData weatherData)
        {
            if(weatherData.Temperature <= 15)
            {
                return WeatherData.Weather.ColdWeather;
            }
            else 
            { 
                return WeatherData.Weather.WarmWeather;
            }
        }

        public bool IsRaining(WeatherData weatherData)
        {
            return weatherData.Precipitation > 0;
 
        }

    }
}
