using Moq;

using WeatherWear.Core.Models;
using WeatherWear.Core;
using WeatherWear.Models;
using WeatherWear.Tests.Stubs;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace WeatherWear.Tests
{
    public class ClothingRecommendationTests
    {
        [Fact]
        public async Task CheckWeatherAsync_ReturnsWeatherType()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();

            // Mock the behavior of GetGeolocation
            geoLocationFetcherMock.Setup(x => x.GetGeolocation())
                .ReturnsAsync(new GeoLocation
                {
                    lat = 123.45,
                    lon = 67.89
                });

            // Mock the behavior of GetWeather
            weatherFetcherMock.Setup(x => x.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(new WeatherData
                {
                    Temperature = 15,
                    Precipitation = 2
                });

            var clothingRecommendation = new ClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);



            // Act
            string result = await clothingRecommendation.CheckCurrentWeather();


            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task CheckWeatherAsync_LocationIsNull_ReturnsEmptyString()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();

            // Set up the mock to return null for GetGeolocationAsync
            geoLocationFetcherMock.Setup(x => x.GetGeolocation()).ReturnsAsync((GeoLocation)null);


            var clothingRecommendation = new TestableClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);

            // Act
            string result = await clothingRecommendation.CheckCurrentWeather();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task TestCheckWeatherAsync_WhenLocIsNotNull_AndIsRainingIsFalse_AndIsWarmIsTrue()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();

            // Mock the behavior of GetGeolocation
            geoLocationFetcherMock.Setup(x => x.GetGeolocation())
                .ReturnsAsync(new GeoLocation
                {
                    lat = 123.45,
                    lon = 67.89
                });

            // Mock the behavior of GetWeather
            weatherFetcherMock.Setup(x => x.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(new WeatherData
                {
                    Temperature = 16,
                    Precipitation = 0
                });

            var clothingRecommendation = new ClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);
            var weatherRecommendationService = new WeatherRecommendationService();
            // Act
            clothingRecommendation.SetWeatherRecommendationService(weatherRecommendationService);

            // Act
            string consoleMessage = await clothingRecommendation.CheckCurrentWeather();

            // Assert
            Assert.Contains("wear light clothing", consoleMessage);
            Assert.Contains("don't need an umbrella", consoleMessage);
        }
        [Fact]
        public async Task TestCheckWeatherAsync_WhenLocIsNotNull_AndIsRainingIsTrue_AndIsWarmIsFalse()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();

            // Mock the behavior of GetGeolocation
            geoLocationFetcherMock.Setup(x => x.GetGeolocation())
                .ReturnsAsync(new GeoLocation
                {
                    lat = 123.45,
                    lon = 67.89
                });

            // Mock the behavior of GetWeather
            weatherFetcherMock.Setup(x => x.GetWeather(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(new WeatherData
                {
                    Temperature = 10,
                    Precipitation = 4
                });

            var clothingRecommendation = new ClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);
            var weatherRecommendationService = new WeatherRecommendationService();
            // Act
            clothingRecommendation.SetWeatherRecommendationService(weatherRecommendationService);

            // Act
            string consoleMessage = await clothingRecommendation.CheckCurrentWeather();

            // Assert
            Assert.Contains("wear warm clothing", consoleMessage);
            Assert.Contains("bring an umbrella", consoleMessage);
        }


        [Fact]
        public async Task CheckFutureWeather_ShouldReturnValidMessage()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();
            var futureWeatherFetcherMock = new Mock<IFutureWeatherFetcher>();

            var clothingRecommendation = new ClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);
            clothingRecommendation.SetFutureWeatherFetcher(futureWeatherFetcherMock.Object);

            WeatherData expectedWeatherData = new WeatherData
            {
                Temperature = 25.0, 
                Precipitation = 5.0, 
            };
            futureWeatherFetcherMock.Setup(f => f.GetWeather(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedWeatherData);

            // Act
            string result = await clothingRecommendation.CheckFutureWeather("IATA_CODE", "2023-10-30");

            // Assert
            string expectedMessage = "It is currently raining, so you should bring an umbrella.\nIt is warm, so you should wear light clothing.";
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async Task CheckCurrentWeather_Exception_ShouldReturnEmptyString()
        {
            // Arrange
            var geoLocationFetcherMock = new Mock<IGeoLocationFetcher>();
            var weatherFetcherMock = new Mock<IWeatherFetcher>();

            geoLocationFetcherMock.Setup(g => g.GetGeolocation()).Throws<Exception>();

            var clothingRecommendation = new ClothingRecommendation(geoLocationFetcherMock.Object, weatherFetcherMock.Object);

            // Act
            string result = await clothingRecommendation.CheckCurrentWeather();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task CheckFutureWeather_Exception_ShouldReturnEmptyString()
        {
            // Arrange
            var futureWeatherFetcherMock = new Mock<IFutureWeatherFetcher>();
            futureWeatherFetcherMock.Setup(f => f.GetWeather(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>(); 

            var clothingRecommendation = new ClothingRecommendation(null, null); 
            clothingRecommendation.SetFutureWeatherFetcher(futureWeatherFetcherMock.Object);

            // Act
            string result = await clothingRecommendation.CheckFutureWeather("IATA_CODE", "2023-10-30");

            // Assert
            Assert.Equal(string.Empty, result);
        }

    }



}

