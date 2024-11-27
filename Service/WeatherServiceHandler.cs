using weather.Core.AppService;
using weather.Core.Entity;
using weather.Core.IServices;

namespace weather.Service
{
    public class WeatherServiceHandler : IWeatherService
    {
        private readonly IExternalWeatherService _externalService;
        public WeatherServiceHandler(IExternalWeatherService externalService)
        {
            _externalService = externalService; 
        }

        public Task DeleteOldDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task FetchAndStoreWeatherDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Weather?> GetLatestWeatherWithFallbackAsync()
        {
            return null;
        }
    }
}
