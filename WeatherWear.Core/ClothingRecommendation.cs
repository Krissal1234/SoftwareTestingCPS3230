using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core.Models;
using WeatherWear.Services;
//using WeatherWear.Services;
namespace WeatherWear.Core
{
    public class ClothingRecommendation
    {

        private readonly  IGeoLocationFetcher _geoLocationFetcher;
        public ClothingRecommendation(GeoLocationFetcher geoLocationFetcher) 
        {
               _geoLocationFetcher = geoLocationFetcher;
        }

 
    }
}
