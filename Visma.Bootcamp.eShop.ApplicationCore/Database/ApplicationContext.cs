using Microsoft.EntityFrameworkCore;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;

namespace Visma.Bootcamp.eShop.ApplicationCore.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CarDriver>()
        //        .HasOne(x => x.Car)
        //        .WithMany(x => x.Drivers)
        //        .HasForeignKey(x => x.CarId);

        //    modelBuilder.Entity<CarDriver>()
        //        .HasOne(x => x.Driver)
        //        .WithMany(x => x.Cars)
        //        .HasForeignKey(x => x.DriverId);

        //    modelBuilder.Entity<CarDriver>()
        //        .HasKey(x => new { x.DriverId, x.CarId });
        //}
    }
}
