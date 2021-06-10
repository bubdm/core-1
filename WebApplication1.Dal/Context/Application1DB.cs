using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Domain.Entities;
using WebApplication.Domain.Identity;

namespace WebApplication1.Dal.Context
{
    public class Application1DB : IdentityDbContext<User, Role, string>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public Application1DB(DbContextOptions<Application1DB> options) : base(options) { }
    }
}
