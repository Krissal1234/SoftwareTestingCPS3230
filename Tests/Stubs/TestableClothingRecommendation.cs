using WeatherWear.Core;
using WeatherWear.Models;
using WeatherWear.Services.APIFetchers.Interfaces;

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
            return null;
        }
    }
}
