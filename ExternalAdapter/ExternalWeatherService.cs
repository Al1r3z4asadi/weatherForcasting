using weather.Common;
using weather.Core.Exceptions;
using weather.Core.IServices;


namespace weather.ExternalService
{
    public class ExternalWeatherService : IExternalWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _weatherApiUrl;
        private readonly ILogger<ExternalWeatherService> _logger;

        public ExternalWeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ExternalWeatherService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _weatherApiUrl = configuration["WeatherApi:Url"];
            _logger = logger;
        }

        public async Task<String?> GetWeatherDataAsync(string key)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                _logger.LogInformation("Attempting to fetch weather data from API at {ApiUrl}", _weatherApiUrl);

                var response = await client.GetAsync($"{ _weatherApiUrl}?{key}");

                response.EnsureSuccessStatusCode();

                _logger.LogInformation("Successfully fetched weather data.");

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ErrorCodes.WeatherFetchErrorMessage);

                throw new WeatherServiceException(ErrorCodes.WeatherFetchErrorCode, ErrorCodes.WeatherFetchErrorMessage, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorCodes.UnexpectedErrorMessage);

                throw new WeatherServiceException(ErrorCodes.UnexpectedErrorCode, ErrorCodes.UnexpectedErrorMessage, ex);
            }
        }
    }

}

