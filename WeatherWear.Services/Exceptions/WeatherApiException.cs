using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

public class WeatherApiException : Exception
{
    public WeatherApiException(string message) : base(message) { }
    public WeatherApiException(string message, Exception innerException) : base(message, innerException) { }
}
