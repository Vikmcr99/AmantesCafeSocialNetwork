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

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            
            builder.Entity<Coffee_User>()
                .HasOne(c => c.Coffee)
                .WithMany(cu => cu.CoffeeUsers)
                .HasForeignKey(ci => ci.CoffeeId);

            builder.Entity<Coffee_User>()
                .HasOne(c => c.Usercoffee)
                .WithMany(cu => cu.CoffeeUsers)
                .HasForeignKey(ci => ci.UserId);

            base.OnModelCreating(builder);
        }



        public DbSet<Coffee> Coffee { get; set; }
        public DbSet<Post> Post { get; set; }

        public DbSet<UserCoffee> UserCoffees{ get; set; }

        public DbSet<Coffee_User> Coffee_Users { get; set; }

        public DbSet<MidiaGallery> MidiaGalleries { get; set; }

        public virtual DbSet<User> Users { get; set; }
      
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
