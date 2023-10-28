using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Services;
namespace Tests.GeolocationTests
{
    public class GeoLocationServiceTests
    {
        [Fact]
        public async Task GetGeolocation_Success()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            httpClient
                .Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"query\":\"78.133.2.167\",\"status\":\"success\",\"country\":\"Malta\", ... }")
                });

            var geoLocationService = new GeoLocationFetcher(httpClient.Object);

            // Act
            var result = await geoLocationService.GetGeolocation();

            // Assert
            Assert.NotNull(result);
            // You can add more specific assertions based on the expected GeoLocation data.
        }
    }
}
