using System;
using System.Runtime.InteropServices;

namespace StudentsDAL.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleToRussian();
            Console.WriteLine("Тестирование библиотеки доступа к данным");


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
