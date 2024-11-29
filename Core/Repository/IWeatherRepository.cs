using weather.Core.Entity;

namespace weather.Core.Repository
{
    public interface IWeatherRepository
    {
        Task<string> GetLatestWeatherAsync(string key);
        Task saveData(string key , string data);
        Task BatchInsertWeatherAsync(IList<Weather> data);
        Task TruncateWeatherTableAsync();
    }
}
