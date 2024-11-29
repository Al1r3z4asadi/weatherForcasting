using Quartz;
using weather.Core.Repository;

namespace weather.Service.Jobs
{
    public class DeleteOldDataJob : IJob
    {
        private readonly IWeatherRepository _sqlRepo;
        public DeleteOldDataJob(IWeatherRepository sqlRepo)
        {
            _sqlRepo = sqlRepo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _sqlRepo.TruncateWeatherTableAsync();
        }
    }
}
