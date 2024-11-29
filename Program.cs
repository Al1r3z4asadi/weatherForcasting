using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using weather.Core.AppService;
using weather.Core.IServices;
using weather.Core.Repository;
using weather.ExternalService;
using weather.Infra.Redis;
using Quartz.Impl;
using weather.Infra.SqlServer.Repository;
using weather.Infra.WeatherDbContext;
using weather.Service;
using Quartz;
using weather.Service.Jobs;

var builder = WebApplication.CreateBuilder(args);


var dbUsername = builder.Configuration["Db:Username"];
var dbPassword = builder.Configuration["Db:Password"];


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("{Db:Username}", dbUsername)
    .Replace("{Db:Password}", dbPassword)));

//Because only spaceship going to call this and we are in spaceship!!
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddQuartz(q =>
{

    var schedulerFactory = new StdSchedulerFactory();
    var scheduler = schedulerFactory.GetScheduler().Result;

    var jobOne = JobBuilder.Create<DeleteOldDataJob>().WithIdentity("DeleteOldDataJob").Build();
    var jobTwo = JobBuilder.Create<BulkUpdateDataJob>().WithIdentity("BulkUpdateDataJob").Build();

    var triggerOne = TriggerBuilder.Create()
        .WithIdentity("DeleteOldDataJob")
        .StartNow()
        .WithCronSchedule("0 0 0 * * ?")  // Every day at midnight (00:00)
        .Build();

    var triggerTwo = TriggerBuilder.Create()
        .WithIdentity("BulkUpdateDataJob")
        .StartNow()
        .WithCronSchedule("0 0 * * * ?")  // Every hour on the hour
        .Build();

    scheduler.ScheduleJob(jobOne, triggerOne).Wait();
    scheduler.ScheduleJob(jobTwo, triggerTwo).Wait();
});



builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});

builder.Services.AddHttpClient();

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(redisConnectionString);
    config.AbortOnConnectFail = false;
    config.ConnectTimeout = 500;
    config.SyncTimeout = 500;
    return ConnectionMultiplexer.Connect(config);
});



builder.Services.AddScoped<IExternalWeatherService, ExternalWeatherService>();
builder.Services.AddScoped<IWeatherService, WeatherServiceHandler>();
builder.Services.AddScoped<IWeatherRepository , WeatherRepository>(); 
builder.Services.AddSingleton<IRedisCache , RedisRepository>(); 



builder.Services.AddControllers();

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
