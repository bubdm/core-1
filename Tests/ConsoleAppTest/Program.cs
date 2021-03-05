using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока



            Console.WriteLine("Test Program");


            var myThread = new Thread(PrintMessages) {IsBackground = true, Priority = ThreadPriority.Lowest};
            myThread.Start();

            Console.WriteLine("Введите свое имя:");
            var name = Console.ReadLine();
            Console.WriteLine($"Привет, {name}!");


            Console.WriteLine("Приложение завершило свою работу");
            Console.ReadKey();
        }

        private static void PrintMessages()
        {
            while (true)
            {
                Console.Title = $"Текущее время: {DateTime.Now.TimeOfDay}";
                Thread.Sleep(100);
            }
        }

    }

}
