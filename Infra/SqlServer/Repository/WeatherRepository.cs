using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using weather.Common;
using weather.Core.Entity;
using weather.Core.Repository;
using weather.Infra.WeatherDbContext;

namespace weather.Infra.SqlServer.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetLatestWeatherAsync(string key)
        {
            var weather = await _context.WeatherData.FirstOrDefaultAsync(instance => instance.Key == HashUtility.ComputeSHA256Hash(key));
            return weather?.Data ?? null ;
        }

        public async Task BatchInsertWeatherAsync(IList<Weather> weatherDataList)
        {
            await _context.BulkInsertAsync(weatherDataList);
        }



        public async Task saveData(string key , string newData )
        {
            _context.WeatherData.Update(new Weather { Data = newData , Key = HashUtility.ComputeSHA256Hash(key)});
            await _context.SaveChangesAsync();
        }

        public async Task TruncateWeatherTableAsync()
        {
            //truncate better than delete because no extra log will be commited
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Weather");
        }
    }
}
