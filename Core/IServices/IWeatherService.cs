using weather.Common;
using weather.Core.Entity;


namespace weather.Core.AppService
{
    public interface IWeatherService
    {
        Task FetchAndStoreWeatherDataAsync();
        Task<ServiceResult<string>> GetLatestWeatherWithFallbackAsync();
        Task DeleteOldDataAsync();
    }
}
