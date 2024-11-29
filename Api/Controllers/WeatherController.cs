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

        [HttpGet("")]
        public async Task<IActionResult> GetLatestWeather()
        {
            var queryParameters = HttpContext.Request.QueryString.Value?.TrimStart('?'); ;
            var result = await _weatherService.GetLatestWeatherWithFallbackAsync(queryParameters);
            return result.IsSuccess ? Ok(result) : BadRequest();        
        }
    }
}
