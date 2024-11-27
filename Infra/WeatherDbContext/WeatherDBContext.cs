using Microsoft.EntityFrameworkCore;
using weather.Core.Entity;

namespace weather.Infra.WeatherDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Weather> WeatherData { get; set; }
}
