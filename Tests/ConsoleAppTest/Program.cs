using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
                .UseLazyLoadingProxies() //для ленивой загрузки
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Students.DB")
                .Options;

            await using (var db = new StudentsDb(options)) 
                await InitDbAsync(db);

            await using (var db = new StudentsDb(options))
            {
                var query = db.Students
                    //.Include(s => s.Courses) //энергичная загрузка
                    .Where(s => s.BirthDay >= new DateTime(1995, 1, 1) && s.BirthDay <= new DateTime(2008, 1, 1));

                var students = await query.ToArrayAsync();

                StringBuilder sb = new StringBuilder();
                foreach (var student in students)
                {
                    Console.WriteLine("{0} {1} {2} - {3:d}", student.LastName, student.Name, student.Patronymic, student.BirthDay);
                    sb.Clear();
                    //await db.Entry(student).Collection(x => x.Courses).LoadAsync(); //явная загрузка
                    if (student.Courses.Count > 0)
                    {
                        sb.Append("   Курсы:");
                        foreach (var c in student.Courses) sb.Append(" " + c.Name);
                        Console.WriteLine(sb.ToString());
                    }
                }

                Console.WriteLine($"Количество студентов всего: {students.Count()}");
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
