using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using StudentsDAL.EF;

namespace StudentsDAL.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleToRussian();
            Console.WriteLine("Тестирование библиотеки доступа к данным");

            using (var context = new StudentsContext())
            {
                //StudentsDataInit.RecreateDatabase(context);
                //StudentsDataInit.ClearData(context);
                //StudentsDataInit.InitData(context);
            }

            using (var context = new StudentsContext())
            {
                Console.WriteLine("Студенты");
                foreach (var student in context.Students.Include(s => s.Group).Include(s => s.Courses))
                {
                    var mm = student.Courses.ToList();
                    Console.WriteLine($"{student} - {student.Group.Name} - курсы {student.Courses.Count} штук: {student.Courses.Count}, {string.Join(", ", student.Courses)}");
                }
                Console.WriteLine("Курсы:");
                foreach (var course in context.Courses.Include(c => c.Students))
                {
                    Console.WriteLine($"{course.Name} - студентов {course.Students.Count} штук, {string.Join(", ", course.Students)}");
                }

            }

            Console.WriteLine("Нажмите любую кнопку ...");
            Console.ReadKey();
        }
        private static void ConsoleToRussian()
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока
        }
    }
}
