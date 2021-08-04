using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Entities.Orders;
using WebApplication1.Domain.Identity;

namespace WebApplication1.Dal.Context
{
    public class Application1Context : IdentityDbContext<User, Role, string>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Person> Persons { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        public Application1Context(DbContextOptions<Application1Context> options) : base(options) { }

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
