﻿using Moq;
using Moq.Protected;
using System.Net;
using WeatherWear.Core.Models;
using WeatherWear.Exceptions;
using WeatherWear.Services.APIFetchers;

namespace WeatherWear.Tests.APIFetcherTests
{
    public class FutureWeatherFetcherTests
    {

        [Fact]
        public async Task GetWeather_InvalidDateFormat_ThrowsArgumentException()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            var futureWeatherFetcher = new FutureWeatherFetcher();
            futureWeatherFetcher.SetHttpClient(httpClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => futureWeatherFetcher.GetWeather("IATA", "invalid-date"));
        }

        [Fact]
        public async Task GetFutureWeather_WhenRequestFails_ShouldThrowWeatherApiException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var futureWeatherFetcher = new FutureWeatherFetcher();
            futureWeatherFetcher.SetHttpClient(httpClient);

            // Act and Assert
            ApiException exception = await Assert.ThrowsAsync<ApiException>(async () => await futureWeatherFetcher.GetWeather("IATA","2023-10-29"));
            Assert.Contains("Error while fetching weather data", exception.Message);
        }


        [Fact]
        public async Task GetWeather_WhenResponseIsSuccessful_ShouldReturnFutureWeatherData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"location\":{\"name\":\"Malta International Airport\",\"region\":\"Luqa\",\"country\":\"Malta\",\"lat\":35.86,\"lon\":14.48,\"tz_id\":\"Europe/Malta\",\"localtime_epoch\":1698583976,\"localtime\":\"2023-10-29 13:52\"},\"current\":{\"last_updated_epoch\":1698583500,\"last_updated\":\"2023-10-29 13:45\",\"temp_c\":26.0,\"temp_f\":78.8,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":8.1,\"wind_kph\":13.0,\"wind_degree\":150,\"wind_dir\":\"SSE\",\"pressure_mb\":1017.0,\"pressure_in\":30.03,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":58,\"cloud\":0,\"feelslike_c\":27.3,\"feelslike_f\":81.2,\"vis_km\":10.0,\"vis_miles\":6.0,\"uv\":6.0,\"gust_mph\":4.2,\"gust_kph\":6.7},\"forecast\":{\"forecastday\":[{\"date\":\"2023-10-30\",\"date_epoch\":1698624000,\"day\":{\"maxtemp_c\":24.9,\"maxtemp_f\":76.8,\"mintemp_c\":23.0,\"mintemp_f\":73.4,\"avgtemp_c\":24.0,\"avgtemp_f\":75.2,\"maxwind_mph\":20.6,\"maxwind_kph\":33.1,\"totalprecip_mm\":0.0,\"totalprecip_in\":0.0,\"totalsnow_cm\":0.0,\"avgvis_km\":10.0,\"avgvis_miles\":6.0,\"avghumidity\":76.0,\"daily_will_it_rain\":0,\"daily_chance_of_rain\":0,\"daily_will_it_snow\":0,\"daily_chance_of_snow\":0,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"uv\":6.0},\"astro\":{\"sunrise\":\"06:22 AM\",\"sunset\":\"05:09 PM\",\"moonrise\":\"06:08 PM\",\"moonset\":\"08:04 AM\",\"moon_phase\":\"Waning Gibbous\",\"moon_illumination\":98,\"is_moon_up\":1,\"is_sun_up\":0},\"hour\":[{\"time_epoch\":1698620400,\"time\":\"2023-10-30 00:00\",\"temp_c\":23.0,\"temp_f\":73.4,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":8.9,\"wind_kph\":14.4,\"wind_degree\":144,\"wind_dir\":\"SE\",\"pressure_mb\":1017.0,\"pressure_in\":30.03,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":71,\"cloud\":11,\"feelslike_c\":25.0,\"feelslike_f\":77.0,\"windchill_c\":23.0,\"windchill_f\":73.4,\"heatindex_c\":25.0,\"heatindex_f\":77.0,\"dewpoint_c\":17.4,\"dewpoint_f\":63.4,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":13.6,\"gust_kph\":21.9,\"uv\":1.0},{\"time_epoch\":1698624000,\"time\":\"2023-10-30 01:00\",\"temp_c\":23.0,\"temp_f\":73.4,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":10.3,\"wind_kph\":16.6,\"wind_degree\":144,\"wind_dir\":\"SE\",\"pressure_mb\":1017.0,\"pressure_in\":30.03,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":71,\"cloud\":11,\"feelslike_c\":25.0,\"feelslike_f\":77.0,\"windchill_c\":23.0,\"windchill_f\":73.4,\"heatindex_c\":25.0,\"heatindex_f\":77.0,\"dewpoint_c\":17.6,\"dewpoint_f\":63.6,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":15.6,\"gust_kph\":25.1,\"uv\":1.0},{\"time_epoch\":1698627600,\"time\":\"2023-10-30 02:00\",\"temp_c\":23.8,\"temp_f\":74.8,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":12.3,\"wind_kph\":19.8,\"wind_degree\":142,\"wind_dir\":\"SE\",\"pressure_mb\":1017.0,\"pressure_in\":30.02,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":9,\"feelslike_c\":25.5,\"feelslike_f\":78.0,\"windchill_c\":23.8,\"windchill_f\":74.8,\"heatindex_c\":25.5,\"heatindex_f\":78.0,\"dewpoint_c\":18.4,\"dewpoint_f\":65.2,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":16.1,\"gust_kph\":25.9,\"uv\":1.0},{\"time_epoch\":1698631200,\"time\":\"2023-10-30 03:00\",\"temp_c\":23.8,\"temp_f\":74.8,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":13.0,\"wind_kph\":20.9,\"wind_degree\":147,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":30.01,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":13,\"feelslike_c\":25.6,\"feelslike_f\":78.0,\"windchill_c\":23.8,\"windchill_f\":74.8,\"heatindex_c\":25.6,\"heatindex_f\":78.0,\"dewpoint_c\":18.5,\"dewpoint_f\":65.3,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.0,\"gust_kph\":27.3,\"uv\":1.0},{\"time_epoch\":1698634800,\"time\":\"2023-10-30 04:00\",\"temp_c\":23.9,\"temp_f\":74.9,\"is_day\":0,\"condition\":{\"text\":\"Partly cloudy\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/116.png\",\"code\":1003},\"wind_mph\":13.2,\"wind_kph\":21.2,\"wind_degree\":152,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":30.01,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":29,\"feelslike_c\":25.6,\"feelslike_f\":78.1,\"windchill_c\":23.9,\"windchill_f\":74.9,\"heatindex_c\":25.6,\"heatindex_f\":78.1,\"dewpoint_c\":18.6,\"dewpoint_f\":65.5,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.2,\"gust_kph\":27.7,\"uv\":1.0},{\"time_epoch\":1698638400,\"time\":\"2023-10-30 05:00\",\"temp_c\":23.8,\"temp_f\":74.9,\"is_day\":0,\"condition\":{\"text\":\"Partly cloudy\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/116.png\",\"code\":1003},\"wind_mph\":13.4,\"wind_kph\":21.6,\"wind_degree\":151,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":30.01,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":32,\"feelslike_c\":25.6,\"feelslike_f\":78.1,\"windchill_c\":23.8,\"windchill_f\":74.9,\"heatindex_c\":25.6,\"heatindex_f\":78.1,\"dewpoint_c\":18.7,\"dewpoint_f\":65.6,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.6,\"gust_kph\":28.3,\"uv\":1.0},{\"time_epoch\":1698642000,\"time\":\"2023-10-30 06:00\",\"temp_c\":23.8,\"temp_f\":74.9,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":12.1,\"wind_kph\":19.4,\"wind_degree\":158,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":30.02,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":21,\"feelslike_c\":25.6,\"feelslike_f\":78.1,\"windchill_c\":23.8,\"windchill_f\":74.9,\"heatindex_c\":25.6,\"heatindex_f\":78.1,\"dewpoint_c\":18.7,\"dewpoint_f\":65.6,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":15.9,\"gust_kph\":25.6,\"uv\":1.0},{\"time_epoch\":1698645600,\"time\":\"2023-10-30 07:00\",\"temp_c\":23.9,\"temp_f\":75.1,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":13.2,\"wind_kph\":21.2,\"wind_degree\":148,\"wind_dir\":\"SSE\",\"pressure_mb\":1017.0,\"pressure_in\":30.02,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":18,\"feelslike_c\":25.7,\"feelslike_f\":78.2,\"windchill_c\":23.9,\"windchill_f\":75.1,\"heatindex_c\":25.7,\"heatindex_f\":78.2,\"dewpoint_c\":18.8,\"dewpoint_f\":65.8,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.2,\"gust_kph\":27.6,\"uv\":6.0},{\"time_epoch\":1698649200,\"time\":\"2023-10-30 08:00\",\"temp_c\":24.2,\"temp_f\":75.6,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":15.0,\"wind_kph\":24.1,\"wind_degree\":149,\"wind_dir\":\"SSE\",\"pressure_mb\":1017.0,\"pressure_in\":30.04,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":9,\"feelslike_c\":25.9,\"feelslike_f\":78.7,\"windchill_c\":24.2,\"windchill_f\":75.6,\"heatindex_c\":25.9,\"heatindex_f\":78.7,\"dewpoint_c\":18.9,\"dewpoint_f\":66.0,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":18.8,\"gust_kph\":30.3,\"uv\":6.0},{\"time_epoch\":1698652800,\"time\":\"2023-10-30 09:00\",\"temp_c\":24.5,\"temp_f\":76.0,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":14.1,\"wind_kph\":22.7,\"wind_degree\":160,\"wind_dir\":\"SSE\",\"pressure_mb\":1018.0,\"pressure_in\":30.05,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":10,\"feelslike_c\":26.1,\"feelslike_f\":79.1,\"windchill_c\":24.5,\"windchill_f\":76.0,\"heatindex_c\":26.1,\"heatindex_f\":79.1,\"dewpoint_c\":19.1,\"dewpoint_f\":66.5,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.6,\"gust_kph\":28.3,\"uv\":6.0},{\"time_epoch\":1698656400,\"time\":\"2023-10-30 10:00\",\"temp_c\":24.7,\"temp_f\":76.4,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":14.3,\"wind_kph\":23.0,\"wind_degree\":163,\"wind_dir\":\"SSE\",\"pressure_mb\":1018.0,\"pressure_in\":30.05,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":72,\"cloud\":13,\"feelslike_c\":26.4,\"feelslike_f\":79.4,\"windchill_c\":24.7,\"windchill_f\":76.4,\"heatindex_c\":26.4,\"heatindex_f\":79.4,\"dewpoint_c\":19.4,\"dewpoint_f\":66.8,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":17.7,\"gust_kph\":28.5,\"uv\":6.0},{\"time_epoch\":1698660000,\"time\":\"2023-10-30 11:00\",\"temp_c\":24.8,\"temp_f\":76.7,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":15.4,\"wind_kph\":24.8,\"wind_degree\":158,\"wind_dir\":\"SSE\",\"pressure_mb\":1017.0,\"pressure_in\":30.04,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":19,\"feelslike_c\":26.5,\"feelslike_f\":79.7,\"windchill_c\":24.8,\"windchill_f\":76.7,\"heatindex_c\":26.5,\"heatindex_f\":79.7,\"dewpoint_c\":19.5,\"dewpoint_f\":67.2,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":19.0,\"gust_kph\":30.6,\"uv\":6.0},{\"time_epoch\":1698663600,\"time\":\"2023-10-30 12:00\",\"temp_c\":24.9,\"temp_f\":76.8,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":16.1,\"wind_kph\":25.9,\"wind_degree\":159,\"wind_dir\":\"SSE\",\"pressure_mb\":1017.0,\"pressure_in\":30.02,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":11,\"feelslike_c\":26.6,\"feelslike_f\":79.9,\"windchill_c\":24.9,\"windchill_f\":76.8,\"heatindex_c\":26.6,\"heatindex_f\":79.9,\"dewpoint_c\":19.7,\"dewpoint_f\":67.4,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":19.9,\"gust_kph\":32.0,\"uv\":6.0},{\"time_epoch\":1698667200,\"time\":\"2023-10-30 13:00\",\"temp_c\":24.9,\"temp_f\":76.8,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":16.6,\"wind_kph\":26.6,\"wind_degree\":164,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":30.0,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":73,\"cloud\":8,\"feelslike_c\":26.7,\"feelslike_f\":80.0,\"windchill_c\":24.9,\"windchill_f\":76.8,\"heatindex_c\":26.7,\"heatindex_f\":80.0,\"dewpoint_c\":19.8,\"dewpoint_f\":67.6,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":20.5,\"gust_kph\":32.9,\"uv\":6.0},{\"time_epoch\":1698670800,\"time\":\"2023-10-30 14:00\",\"temp_c\":24.9,\"temp_f\":76.8,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":17.0,\"wind_kph\":27.4,\"wind_degree\":162,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.99,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":74,\"cloud\":7,\"feelslike_c\":26.7,\"feelslike_f\":80.0,\"windchill_c\":24.9,\"windchill_f\":76.8,\"heatindex_c\":26.7,\"heatindex_f\":80.0,\"dewpoint_c\":20.1,\"dewpoint_f\":68.1,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":21.2,\"gust_kph\":34.1,\"uv\":6.0},{\"time_epoch\":1698674400,\"time\":\"2023-10-30 15:00\",\"temp_c\":24.8,\"temp_f\":76.6,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":16.6,\"wind_kph\":26.6,\"wind_degree\":161,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.98,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":76,\"cloud\":6,\"feelslike_c\":26.6,\"feelslike_f\":79.9,\"windchill_c\":24.8,\"windchill_f\":76.6,\"heatindex_c\":26.6,\"heatindex_f\":79.9,\"dewpoint_c\":20.3,\"dewpoint_f\":68.5,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":20.8,\"gust_kph\":33.5,\"uv\":6.0},{\"time_epoch\":1698678000,\"time\":\"2023-10-30 16:00\",\"temp_c\":24.6,\"temp_f\":76.2,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":16.6,\"wind_kph\":26.6,\"wind_degree\":156,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.97,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":78,\"cloud\":6,\"feelslike_c\":26.5,\"feelslike_f\":79.7,\"windchill_c\":24.6,\"windchill_f\":76.2,\"heatindex_c\":26.5,\"heatindex_f\":79.7,\"dewpoint_c\":20.5,\"dewpoint_f\":69.0,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":21.0,\"gust_kph\":33.8,\"uv\":6.0},{\"time_epoch\":1698681600,\"time\":\"2023-10-30 17:00\",\"temp_c\":23.7,\"temp_f\":74.7,\"is_day\":1,\"condition\":{\"text\":\"Sunny\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/day/113.png\",\"code\":1000},\"wind_mph\":14.8,\"wind_kph\":23.8,\"wind_degree\":166,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.98,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":81,\"cloud\":6,\"feelslike_c\":25.7,\"feelslike_f\":78.2,\"windchill_c\":23.7,\"windchill_f\":74.7,\"heatindex_c\":25.7,\"heatindex_f\":78.2,\"dewpoint_c\":20.2,\"dewpoint_f\":68.3,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":21.5,\"gust_kph\":34.6,\"uv\":6.0},{\"time_epoch\":1698685200,\"time\":\"2023-10-30 18:00\",\"temp_c\":23.5,\"temp_f\":74.3,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":15.9,\"wind_kph\":25.6,\"wind_degree\":159,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":29.99,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":82,\"cloud\":5,\"feelslike_c\":25.5,\"feelslike_f\":77.9,\"windchill_c\":23.5,\"windchill_f\":74.3,\"heatindex_c\":25.5,\"heatindex_f\":77.9,\"dewpoint_c\":20.2,\"dewpoint_f\":68.4,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":23.6,\"gust_kph\":37.9,\"uv\":1.0},{\"time_epoch\":1698688800,\"time\":\"2023-10-30 19:00\",\"temp_c\":23.5,\"temp_f\":74.3,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":17.2,\"wind_kph\":27.7,\"wind_degree\":159,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":29.99,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":83,\"cloud\":5,\"feelslike_c\":25.5,\"feelslike_f\":78.0,\"windchill_c\":23.5,\"windchill_f\":74.3,\"heatindex_c\":25.5,\"heatindex_f\":78.0,\"dewpoint_c\":20.4,\"dewpoint_f\":68.7,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":25.5,\"gust_kph\":41.0,\"uv\":1.0},{\"time_epoch\":1698692400,\"time\":\"2023-10-30 20:00\",\"temp_c\":23.4,\"temp_f\":74.1,\"is_day\":0,\"condition\":{\"text\":\"Partly cloudy\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/116.png\",\"code\":1003},\"wind_mph\":18.3,\"wind_kph\":29.5,\"wind_degree\":157,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.98,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":84,\"cloud\":29,\"feelslike_c\":25.5,\"feelslike_f\":77.8,\"windchill_c\":23.4,\"windchill_f\":74.1,\"heatindex_c\":25.5,\"heatindex_f\":77.8,\"dewpoint_c\":20.5,\"dewpoint_f\":68.9,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":27.5,\"gust_kph\":44.3,\"uv\":1.0},{\"time_epoch\":1698696000,\"time\":\"2023-10-30 21:00\",\"temp_c\":23.4,\"temp_f\":74.1,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":19.0,\"wind_kph\":30.6,\"wind_degree\":156,\"wind_dir\":\"SSE\",\"pressure_mb\":1016.0,\"pressure_in\":29.99,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":84,\"cloud\":16,\"feelslike_c\":25.4,\"feelslike_f\":77.8,\"windchill_c\":23.4,\"windchill_f\":74.1,\"heatindex_c\":25.4,\"heatindex_f\":77.8,\"dewpoint_c\":20.5,\"dewpoint_f\":68.9,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":28.8,\"gust_kph\":46.4,\"uv\":1.0},{\"time_epoch\":1698699600,\"time\":\"2023-10-30 22:00\",\"temp_c\":23.4,\"temp_f\":74.1,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":19.5,\"wind_kph\":31.3,\"wind_degree\":157,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.99,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":84,\"cloud\":11,\"feelslike_c\":25.5,\"feelslike_f\":77.8,\"windchill_c\":23.4,\"windchill_f\":74.1,\"heatindex_c\":25.5,\"heatindex_f\":77.8,\"dewpoint_c\":20.6,\"dewpoint_f\":69.0,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":29.3,\"gust_kph\":47.2,\"uv\":1.0},{\"time_epoch\":1698703200,\"time\":\"2023-10-30 23:00\",\"temp_c\":23.4,\"temp_f\":74.1,\"is_day\":0,\"condition\":{\"text\":\"Clear\",\"icon\":\"//cdn.weatherapi.com/weather/64x64/night/113.png\",\"code\":1000},\"wind_mph\":20.6,\"wind_kph\":33.1,\"wind_degree\":158,\"wind_dir\":\"SSE\",\"pressure_mb\":1015.0,\"pressure_in\":29.97,\"precip_mm\":0.0,\"precip_in\":0.0,\"humidity\":84,\"cloud\":4,\"feelslike_c\":25.5,\"feelslike_f\":77.9,\"windchill_c\":23.4,\"windchill_f\":74.1,\"heatindex_c\":25.5,\"heatindex_f\":77.9,\"dewpoint_c\":20.5,\"dewpoint_f\":69.0,\"will_it_rain\":0,\"chance_of_rain\":0,\"will_it_snow\":0,\"chance_of_snow\":0,\"vis_km\":10.0,\"vis_miles\":6.0,\"gust_mph\":31.0,\"gust_kph\":49.9,\"uv\":1.0}]}]}}\r\n")

                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var futureWeatherFetcher = new FutureWeatherFetcher();
            futureWeatherFetcher.SetHttpClient(httpClient);

            // Act
            WeatherData weatherData = await futureWeatherFetcher.GetWeather("IATA", "2023-10-30");

            // Assert
            Assert.NotNull(weatherData);
            Assert.Equal(24, weatherData.Temperature);
            Assert.Equal(0, weatherData.Precipitation);
        }

        [Fact]
        public async Task GetWeather_InvalidJson_Throws()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}\r\n")

                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var futureWeatherFetcher = new FutureWeatherFetcher();
            futureWeatherFetcher.SetHttpClient(httpClient);
           // Act and Asert
            await Assert.ThrowsAsync<ApiException>(() => futureWeatherFetcher.GetWeather("IATA", "2023-10-21"));

        }


        [Fact]
        public async Task GetWeather_WhenResponseIsNotSuccessful_ShouldThrow()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("{\r\n  \"forecast\": {\r\n    \"forecastday\": [\r\n      {\r\n        \"date\": \"2023-10-30\",\r\n        \"day\": {\r\n          \"temp_c\": 24.9,\r\n          \"precip_mm\": 0\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}\r\n")

                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var futureWeatherFetcher = new FutureWeatherFetcher();
            futureWeatherFetcher.SetHttpClient(httpClient);


            //Act and Assert
            await Assert.ThrowsAsync<ApiException>(() => futureWeatherFetcher.GetWeather("IATA", "2023-10-21"));

        }


    }
}
