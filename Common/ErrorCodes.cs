namespace weather.Common
{
    public static  class ErrorCodes
    {
        public const string WeatherFetchErrorCode = "WEATHER_FETCH_ERROR";
        public const string WeatherFetchErrorMessage = "An error occurred while fetching weather data from the external API.";

        public const string UnexpectedErrorCode = "UNEXPECTED_ERROR";
        public const string UnexpectedErrorMessage = "An unexpected error occurred while processing the request.";

    }
}
