using weather.Core.Entity;

namespace weather.Core.AppService
{
    public interface IWeatherService
    {
        Task FetchAndStoreWeatherDataAsync();
        Task<Weather?> GetLatestWeatherWithFallbackAsync();
        Task DeleteOldDataAsync();
    }
}
