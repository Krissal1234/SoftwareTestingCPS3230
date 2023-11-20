using Moq.Protected;
using Moq;
using System.Net;
using WeatherWear.Core.Models;
using WeatherWear.Services.APIFetchers;
using WeatherWear.Exceptions;

namespace WeatherWear.Tests.APIFetcherTests
{
    public class WeatherFetcherTests
    {
        [Fact]
        public async Task GetWeather_WhenResponseIsSuccessful_ShouldReturnWeatherData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"current\": {\"temp_c\": 20.5, \"precip_mm\": 0.0}}")

                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherFetcher = new WeatherFetcher();
            weatherFetcher.SetHttpClient(httpClient);

            // Act
            WeatherData weatherData = await weatherFetcher.GetWeather(1.0, 3.0);

            // Assert
            Assert.NotNull(weatherData);
            Assert.Equal(20.5, weatherData.Temperature);
            Assert.Equal(0, weatherData.Precipitation);

        }

        [Fact]
        public async Task GetWeather_WhenRequestFails_ShouldThrowWeatherApiException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherFetcher = new WeatherFetcher();
            weatherFetcher.SetHttpClient(httpClient);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await weatherFetcher.GetWeather(1.0, 2.0));
            Assert.Contains("Error while fetching weather data", exception.Message);
        }

        [Fact]
        public async Task GetWeather_WhenResponseContentIsNotJson_ShouldThrowWeatherApiException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Invalid JSON response")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherFetcher = new WeatherFetcher();
            weatherFetcher.SetHttpClient(httpClient);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await weatherFetcher.GetWeather(1.0, 2.0));
            Assert.Contains("Error while deserializing weather data", exception.Message);
        }

        [Fact]
        public async Task GetWeather_WhenResponseIsNotOK_ShouldThrowHttpRequestException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound, 
                    Content = new StringContent("Not Found")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var weatherFetcher = new WeatherFetcher();
            weatherFetcher.SetHttpClient(httpClient);

            // Act and Assert
            await Assert.ThrowsAsync<ApiException>(async () => await weatherFetcher.GetWeather(1.0, 2.0));
        }


    }
}

