﻿using Microservices.Precipitation.Infra.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Precipitation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecipitationController : ControllerBase
    {
        private readonly PrecipitationDBContext _context;

        public PrecipitationController(PrecipitationDBContext context)
        {
            _context = context;
        }


        [HttpGet("observation/{zip}")]
        public async Task<IActionResult> Observation(string zip, [FromQuery] int? days)
        {
            if (days == null && days != 0 || days > 30) return BadRequest($"Days is needed");

            DateTime startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
            var data = await _context.Preceipitation.Where(i => i.Zipcode == zip && i.CreatedOn >= startDate).ToListAsync();

            return Ok(data);
        }
    }
}
