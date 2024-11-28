using Microsoft.AspNetCore.Mvc;
using weather.Core.AppService;

namespace weather.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestWeather()
        {
            var result = await _weatherService.GetLatestWeatherWithFallbackAsync();
            return result.IsSuccess ? Ok(result) : BadRequest();        
        }
    }
}
