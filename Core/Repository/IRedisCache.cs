namespace weather.Core.Repository
{
    public interface IRedisCache
    {
        Task<string?> TryGetWeatherFromCacheAsync(string key);
        Task  SetData(string weatherData, string key);
    }
}
