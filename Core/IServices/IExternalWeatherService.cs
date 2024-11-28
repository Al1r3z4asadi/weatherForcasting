using Newtonsoft.Json.Linq;
using weather.Core.Entity;

namespace weather.Core.IServices
{
    public interface IExternalWeatherService
    {
        Task<String?> GetWeatherDataAsync();

    }
}

