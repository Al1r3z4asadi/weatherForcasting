namespace weather.Core.IServices
{
    public interface IExternalWeatherService
    {
        Task<string?> GetWeatherDataAsync(string key);

    }
}

