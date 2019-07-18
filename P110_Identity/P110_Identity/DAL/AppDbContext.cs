using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using P110_Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace P110_Identity.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seeding Countries table
            builder.Entity(typeof(Country)).HasData(
                new Country { Id = 1, Name = "Azerbaijan" },    
                new Country { Id = 2, Name = "Turkey" },    
                new Country { Id = 3, Name = "USA" },    
                new Country { Id = 4, Name = "Ukraine" }
            );

            //seeding Cities table
            builder.Entity(typeof(City)).HasData(
                new City { Id = 1, Name = "Baku", CountryId = 1  },
                new City { Id = 2, Name = "Sumgayit", CountryId = 1 },
                new City { Id = 3, Name = "Ucar", CountryId = 1 },
                new City { Id = 4, Name = "Istanbul", CountryId = 2 },
                new City { Id = 5, Name = "Ankara", CountryId = 2 },
                new City { Id = 6, Name = "Trabzon", CountryId = 2 },
                new City { Id = 7, Name = "New York", CountryId = 3 },
                new City { Id = 8, Name = "California", CountryId = 3 },
                new City { Id = 9, Name = "Miami", CountryId = 3 },
                new City { Id = 10, Name = "Kiev", CountryId = 4 },
                new City { Id = 11, Name = "Xarkov", CountryId = 4 }
            );
        }
    }
}
