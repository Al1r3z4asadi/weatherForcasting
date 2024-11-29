using StackExchange.Redis;
using weather.Common;
using weather.Core.AppService;
using weather.Core.Exceptions;
using weather.Core.IServices;
using weather.Core.Repository;

namespace weather.Service
{
    public class WeatherServiceHandler : IWeatherService
    {
        private readonly IExternalWeatherService _externalService;
        private readonly IWeatherRepository _weatherRepository; 
        private readonly ILogger<WeatherServiceHandler> _logger;
        private readonly IRedisCache redisCache;

        public WeatherServiceHandler(
            IExternalWeatherService externalService,
            IWeatherRepository weatherRepository,
            ILogger<WeatherServiceHandler> logger,
            IRedisCache _cache)
        {
            _externalService = externalService;
            _weatherRepository = weatherRepository;
            redisCache = _cache;
            _logger = logger;   
        }

 
        public async Task FetchAndStoreWeatherDataAsync()
        {

            //var data = await redisCache.TryGetWeatherFromCacheAsync(key);  
            //if (string.IsNullOrEmpty(data))
            //{
            //    data = await _externalService.GetWeatherDataAsync();
            //}
            //if(!string.IsNullOrEmpty(data)) { 
            //    _weatherRepository.saveData(data);
            //}
        }

        public async Task<ServiceResult<string>> GetLatestWeatherWithFallbackAsync(string key)
        {
            var cachedData = await redisCache.TryGetWeatherFromCacheAsync(key);
            if (cachedData != null)
            {
                return ServiceResult<string>.Success(cachedData);
            }

            var externalData = await TryGetWeatherFromExternalServiceAsync(key);
            if (externalData != null)
            {
                return ServiceResult<string>.Success(externalData);
            }

            var dbData = await TryGetWeatherFromDatabaseAsync(key);
            if (!string.IsNullOrEmpty(dbData))
            {
                return ServiceResult<string>.Success(dbData);
            }

            return ServiceResult<string>.Failure("No data available from any source");
        }



        private async Task<string> TryGetWeatherFromExternalServiceAsync(string key)
        {
            try
            {
                var weatherData = await _externalService.GetWeatherDataAsync(key);
                if (!string.IsNullOrEmpty(weatherData))
                {
                    await redisCache.SetData(weatherData , key);
                }
                return weatherData;
            }
            catch (WeatherServiceException ex)
            {
                _logger.LogWarning(ex, "Weather service exception while fetching weather data.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while calling external weather service.");
                return null;
            }
        }

        private async Task<string> TryGetWeatherFromDatabaseAsync(string key)
        {
            try
            {
                return await _weatherRepository.GetLatestWeatherAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data from the database.");
                return null;
            }
        }


    }
    
}
