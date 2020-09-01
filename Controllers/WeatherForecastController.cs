using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JwtAuth.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<WeatherForecast> Get()
        {
            double daysOld = 0;
            if (HttpContext.User.HasClaim(claim => claim.Type == "DateCreated"))
            {
                DateTime date = DateTime.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DateCreated").Value);
                daysOld = (DateTime.Now - date).TotalDays;
            }
            var rng = new Random();
            if (daysOld >= 5)
            {
                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
            }
            else
            {
                return Unauthorized("Top Secret");
            }
        }
    }
}
