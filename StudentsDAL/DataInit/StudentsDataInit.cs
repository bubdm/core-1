using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentsDAL.EF;
using StudentsDAL.Models;

namespace StudentsDAL.DataInit
{
    public class StudentsDataInit
    {
        private static Random rnd = new Random();
        public static void RecreateDatabase(StudentsContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
        public static void InitData(StudentsContext context)
        {
            var cources = Enumerable.Range(1, 10).Select(c => new Course
            {
                Name = $"Учебный предмет №{rnd.Next(20)}",
                Description = $"Описание предмета {c}",
            }).ToList();
            context.Courses.AddRange(cources);
            var groups = Enumerable.Range(1, 10).Select(c => new Group
            {
                Name = $"Группа{rnd.Next(100, 900)}",
            }).ToList();
            context.Groups.AddRange(groups);
            var students = Enumerable.Range(1, 30).Select(s => new Student
            {
                LastName = $"Иванов_{s}",
                FirstName = $"Иван_{s + 1}",
                Patronymic = $"Иванович_{s - 1}",
                Birthday = new DateTime(rnd.Next(1930, 1980), 1,1),
                Rating = (float) rnd.NextDouble() * 10.0f,
                Pet = (s % 2 == 1) ? $"Кошка{s}" : String.Empty,
                Courses = Enumerable.Range(1, rnd.Next(8))
                    .Select(c => cources[rnd.Next(cources.Count)]).ToList(),
                Group = groups[rnd.Next(groups.Count)],
            }).ToList();
            context.Students.AddRange(students);
            context.Database.OpenConnection();
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных: {ex.Message}");
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }
        public static void ClearData(StudentsContext context)
        {
            ExecuteDeleteSql(context, "CourseStudent");
            ExecuteDeleteSql(context, "Students");
            ExecuteDeleteSql(context, "Groups");
            ExecuteDeleteSql(context, "Courses");
            static void ExecuteDeleteSql(StudentsContext context, string tableName)
            {
                var rawSqlString = $"Delete from dbo.{tableName}";
                context.Database.ExecuteSqlRaw(rawSqlString);
            }
            static void ResetIdentity(StudentsContext context)
            {
                var tables = new[] { "CourseStudent", "Students", "Groups", "Courses" };
                foreach (var item in tables)
                {
                    var rawSqlString = $"DBCC CHECKIDENT (\"dbo.{item}\", RESEED, -1);";
                    context.Database.ExecuteSqlRaw(rawSqlString);
                }
            }
        }
    }
}
