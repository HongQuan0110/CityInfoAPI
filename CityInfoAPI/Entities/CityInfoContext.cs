using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions options)
            :base(options)
        {
            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> poinstOfInterest { get; set; }
    }
}
