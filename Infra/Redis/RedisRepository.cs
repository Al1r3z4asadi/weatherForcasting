using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using weather.Common;
using weather.Core.Entity;
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
        private readonly IConnectionMultiplexer _redis;
        private readonly string host;
        private readonly int port;


        public RedisRepository(IConnectionMultiplexer redis, ILogger<WeatherServiceHandler> logger, IConfiguration configuration)
        {
            _redis = redis;
            _redisDatabase = redis.GetDatabase();
            _configuration = configuration;
            _logger = logger;
            _cacheExpiry = TimeSpan.FromHours(int.Parse(_configuration["Redis:CacheKeyExpiryInHours"]));
            var redisConfig = configuration.GetSection("Redis");
            host = redisConfig.GetValue<string>("Host");
            port = redisConfig.GetValue<int>("Port");
        }

        public string GetKey(string key)
        {
            return $"{CacheKeys.LatestWeather}{HashUtility.ComputeSHA256Hash(key)}";
        }

        public async Task SetData(string weatherData, string key)
        {
            try
            {
                var hashedKey = GetKey(key);
                bool result = await _redisDatabase.StringSetAsync(hashedKey, weatherData, _cacheExpiry);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("cache setData error!!");
            }
        }

        public async Task<string?> TryGetWeatherFromCacheAsync(string key)
        {
            try
            {
                var cachedData = await _redisDatabase.StringGetAsync(GetKey(key));
                return string.IsNullOrEmpty(cachedData) ? RedisValue.Null : cachedData;
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogWarning(ex, "Redis connection issue while fetching weather data.");
                return null;
            }
        }

        public  IEnumerable<List<Weather>> GetWeatherDataStartingWithLatestPaginatedAsync()
        {
            var cursor = 0L; 
            do
            {
                var scanResult =  _redisDatabase.Execute("SCAN", cursor.ToString(), "MATCH", $"{CacheKeys.LatestWeather}*");
                var keys = (RedisKey[])scanResult[1];
                var weatherList = new List<Weather>();

                foreach (var key in keys)
                {
                    var data =  _redisDatabase.StringGet(key);
                    if (!data.IsNullOrEmpty)
                    {
                        weatherList.Add(new Weather
                        {
                            Key = key,
                            Data = data
                        });
                    }
                }

                if (weatherList.Any())
                {
                    yield return weatherList;
                }
                cursor = (long)scanResult[0];
            } while (cursor != 0);
        }

    }
}