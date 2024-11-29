using Quartz;
using weather.Core.Repository;

namespace weather.Service.Jobs
{
    public class BulkUpdateDataJob : IJob
    {
        private readonly IWeatherRepository _sqlRepo;
        private readonly IRedisCache _redisCache;
        public BulkUpdateDataJob(IWeatherRepository sqlRepo , IRedisCache redisCache) {
            _sqlRepo = sqlRepo;
            _redisCache = redisCache;
        }  

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var weatherBatch in _redisCache.GetWeatherDataStartingWithLatestPaginatedAsync())
            {
                await _sqlRepo.BatchInsertWeatherAsync(weatherBatch.ToList());
            }
        }
    }
}
