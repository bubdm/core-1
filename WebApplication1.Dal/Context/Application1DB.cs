using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Domain.Entities;
using WebApplication.Domain.Entities.Orders;
using WebApplication.Domain.Identity;

namespace WebApplication1.Dal.Context
{
    public class Application1DB : IdentityDbContext<User, Role, string>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Person> Persons { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public Application1DB(DbContextOptions<Application1DB> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasMany(order => order.Items)
                .WithOne(item => item.Order)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany<Order>()
                .WithOne(order => order.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(item => item.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
