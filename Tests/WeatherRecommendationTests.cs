using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Core;
using WeatherWear.Core.Models;

namespace WeatherWear.Tests
{
    public class WeatherRecommendationTests
    {
        private WeatherRecommendationService clothingService;
        private WeatherData _weatherData;

        public WeatherRecommendationTests()
        {
            
            _weatherData = new WeatherData();
            clothingService = new WeatherRecommendationService();
            
        }


        [Fact]
        public void Test_GetWeatherType_ColdWeather()
        {
            // Arrange
            WeatherData weatherData = new WeatherData() { Temperature = 10};
            // Act
            var result = clothingService.GetWeatherType(weatherData);

            // Assert
            Assert.Equal(WeatherData.Weather.ColdWeather, result);
        }

        [Fact]
        public void Test_GetWeatherType_WarmWeather()
        {
            // Arrange
            WeatherData weatherData = new WeatherData() { Temperature = 20 };
            // Act
            var result = clothingService.GetWeatherType(weatherData);

            // Assert
            Assert.Equal(WeatherData.Weather.WarmWeather, result);
        }

        [Fact]
        public void Test_IsRaining_True()
        {
            // Arrange
            WeatherData weatherData = new WeatherData() { Precipitation = 10 };
            // Act
            var result = clothingService.IsRaining(weatherData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Test_IsRaining_False()
        {
            // Arrange
            WeatherData weatherData = new WeatherData() { Precipitation = 0 };
            // Act
            var result = clothingService.IsRaining(weatherData);

            // Assert
            Assert.False(result);
        }

    }
}
