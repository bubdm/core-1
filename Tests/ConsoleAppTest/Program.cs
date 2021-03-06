using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

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

            ThreadManagement.Start();


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

        private static void Printer(string Message, EventWaitHandle EventWait)
        {
            var th_id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Поток id:{th_id} начал наботу");

            EventWait.WaitOne();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Id: {th_id}\t{Message}");
                Thread.Sleep(10);
            }

            Console.WriteLine($"Поток id:{th_id} завершил работу");
        }
    }
}
