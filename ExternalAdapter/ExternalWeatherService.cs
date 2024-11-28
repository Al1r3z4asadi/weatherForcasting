using weather.Core.Entity;
using weather.Core.IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace weather.ExternalService
{
    public class ExternalWeatherService : IExternalWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _weatherApiUrl;


        public ExternalWeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _weatherApiUrl = configuration["WeatherApi:Url"];

        }
        public async Task<String?> GetWeatherDataAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(_weatherApiUrl);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException ex)
            {
                // Log the exception (placeholder)
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }
    }

}

