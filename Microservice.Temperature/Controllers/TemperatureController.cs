using Microservice.Temperature.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using en=Microservice.Temperature.Infra.Entities;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microservice.Temperature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly TemperatureDbContext _context;

        public TemperatureController(TemperatureDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("observation/{zip}")]
        public async Task<IActionResult> Observation(string zip, [FromQuery] int? days)
        {
            if (days == null && days != 0 || days > 30) return BadRequest($"Days is needed");

            DateTime startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
            var data = await _context.Temperature.Where(i => i.Zipcode == zip && i.CreatedOn >= startDate).ToListAsync();

            return Ok(data);
        }

        [HttpPost("observation")]
        public async Task<IActionResult> Observation([FromBody] en.Temperature temperature)
        {
            try
            {
                _context.Temperature.Add(temperature);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
