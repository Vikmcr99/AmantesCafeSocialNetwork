using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data
{
    public class CoffeeDbContext : IdentityDbContext<User>
    {
        public CoffeeDbContext(DbContextOptions<CoffeeDbContext> options) : base(options) { }

        public DbSet<Coffee> Coffee { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoffeeDbContext>
    {
        public CoffeeDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../CoffeeAPI/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<CoffeeDbContext>();

            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new CoffeeDbContext(builder.Options);
        }

    }
}
