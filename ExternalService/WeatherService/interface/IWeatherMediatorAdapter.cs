namespace weather.ExternalService.WeatherService.@interface
{
    public interface IWeatherMediatorAdapter
    {
        IWeaterhExternalService weatherExternalService { get; }
        IWeaterhExternalService weatherGoogleService { get; }
    }
}
