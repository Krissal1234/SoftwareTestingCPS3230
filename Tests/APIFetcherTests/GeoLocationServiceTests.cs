using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Exceptions;
using WeatherWear.Models;
using WeatherWear.Services;
using WeatherWear.Services.APIFetchers;

namespace Tests.GeolocationTests
{
    public class GeoLocationServiceTests
    {
        [Fact]
        public async Task GetGeolocation_WhenResponseIsSuccessful_ShouldReturnGeoLocation()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"lat\": 1.0, \"lon\": 2.0}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geolocationFetcher = new GeoLocationFetcher(httpClient);

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
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geolocationFetcher = new GeoLocationFetcher(httpClient);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await geolocationFetcher.GetGeolocation());
            Assert.Equal("API request failed: Request failed", exception.Message);
        }
        [Fact]
        public async Task GetGeolocation_UnsuccessfulResponse_UsesBackupFetcher()
        {
            // Arrange
            var httpClientMock = new Mock<HttpClient>();
            httpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var backupHttpClientMock = new Mock<HttpClient>();
            backupHttpClientMock
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"lat\": 1.0, \"lon\": 2.0}"), // Replace with your sample JSON data
                });

            var backupGeoLocationFetcherMock = new Mock<BackupGeoLocationFetcher>(backupHttpClientMock.Object);
            backupGeoLocationFetcherMock
                .Setup(fetcher => fetcher.GetGeolocation())
                .ReturnsAsync(new GeoLocation { lat = 1.0, lon=2.0});

            var geoLocationFetcher = new GeoLocationFetcher(httpClientMock.Object);

            // Act
            var result = await geoLocationFetcher.GetGeolocation();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1.0,result.lat);
            Assert.Equal(2.0, result.lon);
        }
    }
  }


