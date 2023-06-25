using Microservice.Report.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Report.Infra.Context
{
    public class WeatherReportContext:DbContext
    {
        public WeatherReportContext(DbContextOptions<WeatherReportContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<WeatherReport> WeatherReport { get; set; }
    }
}
