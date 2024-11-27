using weather.Core.IServices;
using weather.ExternalService.WeatherService.@interface;

namespace weather.Service
{
    public class ExternalWeatherService : IExternalWeatherService
    {
        public readonly IWeatherMediatorAdapter externalWeatherImp;
    
        public ExternalWeatherService(IWeatherMediatorAdapter externalWeatherImp)
        {
            this.externalWeatherImp = externalWeatherImp;   
        }

        public void getWeatherFromExternalSource()
        {
            this.externalWeatherImp.weatherExternalService.getLatestData();
        }

    }
}
