using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWear.Core.Models
{
    public class WeatherData
    {

        public enum Weather
        {
            ColdWeather,
            WarmWeather,
        }
        public double Temperature { get; set; }
        public string? Country { get; set; }
        public double Precipitation { get; set; }
    }
}
