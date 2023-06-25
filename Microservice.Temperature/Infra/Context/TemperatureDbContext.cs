using en =Microservice.Temperature.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Temperature.Infra.Context
{
    public class TemperatureDbContext:DbContext
    {
        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<en.Temperature> Temperature { get; set; }
    }
}
