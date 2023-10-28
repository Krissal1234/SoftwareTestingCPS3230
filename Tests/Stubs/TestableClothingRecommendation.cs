using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core;
using WeatherWear.Models;
using WeatherWear.Services;

namespace WeatherWear.Tests.Stubs
{
    public class TestableClothingRecommendation : ClothingRecommendation
    {
        public TestableClothingRecommendation(IGeoLocationFetcher geoLocationFetcher, IWeatherFetcher weatherFetcher)
            : base(geoLocationFetcher, weatherFetcher)
        {
        }

        protected override async Task<GeoLocation> GetGeolocationAsync()
        {
            // Mocked behavior for GetGeolocationAsync
            return null;
        }
    }
}
