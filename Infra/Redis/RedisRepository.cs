using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using weather.Common;
using weather.Core.Repository;
using weather.Service;

namespace weather.Infra.Redis
{
    public class RedisRepository : IRedisCache
    {
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<WeatherServiceHandler> _logger;
        private readonly TimeSpan _cacheExpiry;
        private readonly IConfiguration _configuration;


        public RedisRepository(IConnectionMultiplexer redis, ILogger<WeatherServiceHandler> logger, IConfiguration configuration)
        {
            _redisDatabase = redis.GetDatabase();
            _configuration = configuration; 
            _logger = logger;   
            _cacheExpiry = TimeSpan.FromDays(int.Parse(_configuration["Redis:CacheKeyExpiryInDays"]));
        }



        public async Task SetData(string weatherData , string key)
        {
            try
            {
                var hashedKey = HashUtility.ComputeSHA256Hash($"{CacheKeys.LatestWeather}-{key}");
                bool result = await _redisDatabase.StringSetAsync( hashedKey, weatherData, _cacheExpiry);
            }
            catch (Exception ex) {
                _logger.LogWarning("cache setData error!!");
            }
        }

        public async Task<string?> TryGetWeatherFromCacheAsync(string key)
        {
            try
            {
                var cachedData = await _redisDatabase.StringGetAsync(HashUtility.ComputeSHA256Hash($"{CacheKeys.LatestWeather}-{key}"));
                return string.IsNullOrEmpty(cachedData) ? RedisValue.Null : cachedData;
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogWarning(ex, "Redis connection issue while fetching weather data.");
                return null;
            }
        }
    }
}
