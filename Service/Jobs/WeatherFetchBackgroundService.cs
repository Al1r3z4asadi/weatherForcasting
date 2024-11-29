using weather.Core.AppService;

namespace weather.Service.Jobs
{
    public class WeatherFetchBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(20);

        public WeatherFetchBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                await weatherService.FetchAndStoreWeatherDataAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
