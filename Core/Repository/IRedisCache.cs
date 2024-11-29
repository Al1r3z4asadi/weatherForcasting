using System.Threading.Tasks;
using weather.Core.Entity;

namespace weather.Core.Repository
{
    public interface IRedisCache
    {
        Task<string?> TryGetWeatherFromCacheAsync(string key);
        Task  SetData(string weatherData, string key);
        IEnumerable<List<Weather>> GetWeatherDataStartingWithLatestPaginatedAsync();
    }
}
