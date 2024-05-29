using F_LocalBrand.Attributes;
using F_LocalBrand.Services;
using F_LocalBrand;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace F_LocalBrand.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IResponseCacheService _responseCacheService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet]
        [Cache(1000)]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        public async Task<IActionResult> GetAsync(string keyword = null, int page = 1, int pageSize = 3)
        {
            var result = new List<WeatherForecast>()
            {
                new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Hot" },
                new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Cool" },
                new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), TemperatureC = 30, Summary = "Warm" },
                new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), TemperatureC = 35, Summary = "Cold" },
                new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), TemperatureC = 40, Summary = "Freezing" }
            };

            return Ok(result);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            await _responseCacheService.RemoveCacheRepsonseAsync("/WeatherForecast");
            return Ok("Create");
        }
    }
}
