using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentsDAL.EF;
using StudentsDAL.Models;
using StudentsDAL.Repos.Base;
using StudentsDAL.Repos.Interfaces;
using static Microsoft.EntityFrameworkCore.EF;


namespace StudentsDAL.Repos
{
    public class StudentsRepo : BaseRepo<Student>, IStudentsRepo
    {
        public new IEnumerable<Student> GetAll() => GetAll(x => x.LastName, false);
        public IEnumerable<Student> GetTopRating() => Get(x => x.Rating > 2.0f);

        public List<Student> Search(string SearchString)
        {
            return Context.Students.Where(c => Functions.Like(c.Pet, $"")).ToList();
        }

        public StudentsRepo() : base(new StudentsContext())
        {
        }
        public StudentsRepo(StudentsContext context) : base(context)
        {
        }
        public StudentsRepo(DbContextOptions<StudentsContext> options) : base(options)
        {
        }

        public List<Student> GetRelatedData() => Context.Students.FromSqlRaw("SELECT * FROM Students")
            .Include(x => x.Group).Include(x => x.Courses).ToList();

    }
}
