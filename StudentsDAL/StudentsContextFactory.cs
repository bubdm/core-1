using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudentsDAL.EF;

namespace StudentsDAL
{
    public class StudentsContextFactory : IDesignTimeDbContextFactory<StudentsContext>
    {
        public StudentsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentsContext>();
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StudentsDev.DB; "+
                                   "Integrated security=True; MultipleActiveResultSets=True; App=EntityFramework;";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            return new StudentsContext(optionsBuilder.Options);
        }
    }
}
