using Microsoft.EntityFrameworkCore;
using System;
using weather.Common;
using weather.Core.Repository;
using weather.Infra.WeatherDbContext;

namespace weather.Infra.SqlServer.Repository
{
    public class WeatherServiceRepo : IWeatherRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherServiceRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetLatestWeatherAsync(string key)
        {
            var weather = await _context.WeatherData.FirstOrDefaultAsync(instance => instance.Key == HashUtility.ComputeSHA256Hash(key));
            return weather?.Data ?? null ;
        }

        public async Task saveData(string key , string newData )
        {
            _context.WeatherData.Update(new Core.Entity.Weather { Data = newData , Key = HashUtility.ComputeSHA256Hash(key)});
            await _context.SaveChangesAsync();
        }
    }
}
