using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ConsoleAppTest.Data;
using ConsoleAppTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAppTest
{
    static class Program
    {
        private static Random _rnd = new Random();
        static async Task Main(string[] args)
        {
            #region Русификация консоли обязательная
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока
            #endregion

            var options = new DbContextOptionsBuilder<StudentsDb>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Students.DB")
                .Options;

            await using (var db = new StudentsDb(options)) 
                await InitDbAsync(db);

            await using (var db = new StudentsDb(options))
            {
                var query = db.Students
                    .Where(s => s.BirthDay >= new DateTime(1995, 1, 1) && s.BirthDay <= new DateTime(2008, 1, 1));
                var students = await query.ToArrayAsync();
                var lastNames = await query.Select(s => s.LastName).Distinct().ToArrayAsync();
                foreach (var ln in students)
                    Console.WriteLine("{0} {1} {2} - {3:d}", ln.LastName, ln.Name, ln.Patronymic, ln.BirthDay);

                Console.WriteLine($"Количество: {students.Count()}");
            }

            Console.WriteLine("Нажмите любую кнопку ...");
            Console.ReadKey();
        }

        private static async Task InitDbAsync(StudentsDb db)
        {
            //await db.Database.EnsureDeletedAsync();
            if (await db.Database.EnsureCreatedAsync())
                Console.WriteLine("База данных создана");

            if (!await db.Courses.AnyAsync())
            {
                var courses = Enumerable.Range(1, 10)
                    .Select(i => new Course
                    {
                        Name = $"Предмет {i}"
                    }).ToArray();
                await db.Courses.AddRangeAsync(courses);
                await db.SaveChangesAsync();

                if (!await db.Students.AnyAsync())
                {
                    var students = Enumerable.Range(1, 1000)
                        .Select(i => new Student
                        {
                            LastName = $"Иванов_{i}",
                            Name = $"Иван_{i}",
                            Patronymic = $"Иванович_{i}",
                            BirthDay = DateTime.Now.Date.AddDays(_rnd.Next(365)).AddYears(-_rnd.Next(20, 40)),
                            Courses = Enumerable.Range(0, _rnd.Next(1, 8))
                                .Select(_ => courses[_rnd.Next(courses.Length)])
                                .Distinct()
                                .ToArray()
                        });
                    await db.Students.AddRangeAsync(students);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
