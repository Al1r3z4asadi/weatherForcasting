using Quartz;
using weather.Core.AppService;

namespace weather.Jobs
{

    public class WeatherFetchJob : IJob
    {
        private readonly IWeatherService _weatherService;


        public WeatherFetchJob(IWeatherService services)
        {
            _weatherService = services;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _weatherService.FetchAndStoreWeatherDataAsync();
        }

    }
   
}
