using Microsoft.EntityFrameworkCore;
using Quartz;
using weather.Infra.WeatherDbContext;
using weather.Jobs;

var builder = WebApplication.CreateBuilder(args);


var dbUsername = builder.Configuration["Db:Username"];
var dbPassword = builder.Configuration["Db:Password"];


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("{Db:Username}", dbUsername)
    .Replace("{Db:Password}", dbPassword)));


builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("WeatherFetchJob");
    q.AddJob<WeatherFetchJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("WeatherFetchTrigger")
        .StartAt(DateTimeOffset.Now.AddSeconds(10))
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromDays(30)) 
            .RepeatForever()) 
    );
});


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();