using weather.Common;
using weather.Core.AppService;
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

        public async  Task<ServiceResult<string>> GetLatestWeatherWithFallbackAsync()
        {
            try
            {
                var result = await _externalService.GetWeatherDataAsync();
                return ServiceResult<string>.Success(result);
            }catch (Exception ex) {
                return ServiceResult<string>.Failure("Empty");
            }
        }
    }
}
