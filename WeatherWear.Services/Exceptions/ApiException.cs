using System;

namespace WeatherWear.Exceptions
{
    public class ApiException : Exception
    {
    
        public string ApiEndpoint { get; }

        public ApiException(string message, string apiEndpoint) : base(message)
        {
            ApiEndpoint = apiEndpoint;
        }

      
        public override string ToString()
        {
            return $"ApiException: {Message}, ApiEndpoint: {ApiEndpoint})";
        }
    }
}
