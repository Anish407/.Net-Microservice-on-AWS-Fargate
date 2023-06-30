using Microservice.Report.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherReportController : ControllerBase
    {

        [HttpGet("GetWeatherReport/{zip}")]
        public async Task<IActionResult> GetWeatherReport([FromRoute]string zip, [FromQuery]int? days, [FromServices]IWeatherReportAggregator weatherReportAggregator)
        {
            var weatherReport = weatherReportAggregator.BuildReport(zip, days.Value);
            return Ok(weatherReport);
        }
    }
}
