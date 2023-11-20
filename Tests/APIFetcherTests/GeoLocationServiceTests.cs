using Moq;
using Moq.Protected;
using System.Net;
using WeatherWear.Exceptions;
using WeatherWear.Models;
using WeatherWear.Services;
using WeatherWear.Services.APIFetchers.Interfaces;

namespace Tests.GeolocationTests
{
    public class GeoLocationServiceTests
    {
        [Fact]
        public async Task GetGeolocation_WhenResponseIsSuccessful_ShouldReturnGeoLocation()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockBackupFetcher = new Mock<IBackupGeoLocationFetcher>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"lat\": 1.0, \"lon\": 2.0}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geolocationFetcher = new GeoLocationFetcher();
            geolocationFetcher.SetHttpClient(httpClient);
            geolocationFetcher.SetBackupGeoLocationFetcher(mockBackupFetcher.Object);

            // Act
            GeoLocation geoLocation = await geolocationFetcher.GetGeolocation();

            // Assert
            Assert.NotNull(geoLocation);
            Assert.Equal(1.0, geoLocation.lat);
            Assert.Equal(2.0, geoLocation.lon);
        }

        [Fact]
        public async Task GetGeolocation_WhenRequestFails_ShouldThrowApiException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockBackupFetcher = new Mock<IBackupGeoLocationFetcher>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geolocationFetcher = new GeoLocationFetcher();
            geolocationFetcher.SetHttpClient(httpClient);
            geolocationFetcher.SetBackupGeoLocationFetcher(mockBackupFetcher.Object);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await geolocationFetcher.GetGeolocation());
            Assert.Equal("API request failed: Request failed", exception.Message);
        }
        [Fact]
        public async Task GetGeolocation_UnsuccessfulResponse_UsesBackupFetcher()
        {
            // Arrange
            var mockBackupFetcher = new Mock<IBackupGeoLocationFetcher>();
            var expectedGeoLocation = new GeoLocation { lat=1.0,lon=2.0 };

            
            mockBackupFetcher.Setup(f => f.GetGeolocation()).ReturnsAsync(expectedGeoLocation);

            // Configure the first API call to fail
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeResponse);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geoLocationFetcher = new GeoLocationFetcher();
            geoLocationFetcher.SetHttpClient(httpClient);
            geoLocationFetcher.SetBackupGeoLocationFetcher(mockBackupFetcher.Object);

            // Act
            var result = await geoLocationFetcher.GetGeolocation();

            // Assert
            mockBackupFetcher.Verify(f => f.GetGeolocation(), Times.Once);
            // Assert result matches expected GeoLocation
            Assert.Equal(expectedGeoLocation, result);
        }
    }
  }


