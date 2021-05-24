using System;
using Microsoft.EntityFrameworkCore;
using StudentsDAL.Models;

namespace StudentsDAL.EF
{
    public class StudentsContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public StudentsContext(DbContextOptions options) : base(options)
        {
        }
        internal StudentsContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StudentsDev.DB; "+
                                       "Integrated security=True; MultipleActiveResultSets=True; App=EntityFramework;";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
