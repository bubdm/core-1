using System;
using System.Collections;
using System.Linq;
using ConsoleAppTest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleAppTest.Data
{
    public class StudentsDb : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public StudentsDb(DbContextOptions<StudentsDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .Property(s => s.Patronymic)
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired();
        }
    }
}
