using Microservices.Precipitation.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Precipitation.Infra.Context
{
    public class PrecipitationDBContext:DbContext
    {
        public PrecipitationDBContext(DbContextOptions<PrecipitationDBContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        public DbSet<Preceipitation> Preceipitation { get; set; }
    }
}
