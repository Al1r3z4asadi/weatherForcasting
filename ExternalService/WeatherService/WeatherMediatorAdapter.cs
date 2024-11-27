using weather.ExternalService.WeatherService.@interface;

namespace weather.ExternalService.WeatherService
{
    public class WeatherMediatorAdapter : IWeatherMediatorAdapter
    {
        public IWeaterhExternalService weatherExternalService => new OpenMeteoWeatherAPI();

        public IWeaterhExternalService weatherGoogleService => throw new NotImplementedException();

    }
}
