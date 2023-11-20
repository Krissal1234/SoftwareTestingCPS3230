using Moq;
using Moq.Protected;
using System.Net;
using WeatherWear.Exceptions;
using WeatherWear.Services.APIFetchers;

namespace WeatherWear.Tests.APIFetcherTests
{
    public class BackupGeolocationTests
    {
        [Fact]
        public async Task GetGeolocation_WhenSuccessfulResponse_ShouldReturnGeoLocation()
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
            var backupGeolocationApi = new BackupGeoLocationFetcher();
            backupGeolocationApi.SetHttpClient(httpClient);

            // Act
            var result = await backupGeolocationApi.GetGeolocation();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1.0, result.lat);
            Assert.Equal(2.0, result.lon);
        }

        [Fact]
        public async Task BackupGeoLocation_WhenRequestFails_ShouldThrowApiException()
        {

            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var backupGeoLocationFetcher = new BackupGeoLocationFetcher();
            backupGeoLocationFetcher.SetHttpClient(httpClient);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await backupGeoLocationFetcher.GetGeolocation());
            Assert.Equal("API request failed: Request failed", exception.Message);

        }
        [Fact]
        public async Task BackupGeoLocation_WhenResonseCodeFails_ThrowsException()
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
            var backupGeoLocationFetcher = new BackupGeoLocationFetcher();
            backupGeoLocationFetcher.SetHttpClient(httpClient);
            // Act and Assert
            await Assert.ThrowsAsync<ApiException>(async () => await backupGeoLocationFetcher.GetGeolocation());

        }
    }
}
