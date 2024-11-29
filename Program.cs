using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using weather.Core.AppService;
using weather.Core.IServices;
using weather.Core.Repository;
using weather.ExternalService;
using weather.Infra.Redis;
using weather.Infra.SqlServer.Repository;
using weather.Infra.WeatherDbContext;
using weather.Service;

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
builder.Services.AddScoped<IWeatherRepository , WeatherServiceRepo>(); 
builder.Services.AddSingleton<IRedisCache , RedisRepository>(); 



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
