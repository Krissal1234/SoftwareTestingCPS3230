using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWear.Models;

namespace WeatherWear.Services.APIFetchers.Interfaces
{
    public interface IBackupGeoLocationFetcher
    {
        Task<GeoLocation> GetGeolocation();
    }
}
