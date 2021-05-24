using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentsDAL.DataInit;
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
                //StudentsDataInit.InitData(context);
                //StudentsDataInit.ClearData(context);
            }


            using (var context = new StudentsContext())
            {
                foreach (var student in context.Students.Include(s => s.Group).Include(s => s.Courses))
                {
                    Console.Write($"{student} - {student.Group.Name} - курсы {student.Courses.Count} штук: {student.Courses.Count}");
                    foreach (var course in student.Courses)
                    {
                        Console.Write(course.Name + ",");
                    }
                    Console.WriteLine();
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
