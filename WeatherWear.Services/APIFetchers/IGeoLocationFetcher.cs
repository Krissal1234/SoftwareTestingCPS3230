using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WeatherWear.Models;

namespace WeatherWear.Services
{
    public interface IGeoLocationFetcher
    {
        Task<GeoLocation> GetGeolocation();
    }
}
