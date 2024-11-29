namespace weather.Core.Exceptions
{
    public class WeatherServiceException : Exception
    {
        public string ErrorCode { get; }

        public WeatherServiceException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public WeatherServiceException(string errorCode, string message, Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }
    }
}


