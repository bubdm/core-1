using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

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


            new Thread(() =>
                {
                while (true)
                {
                    Console.Title = $"Время: {DateTime.Now.ToLongTimeString()}";
                    Thread.Sleep(100);
                }
                })
                {IsBackground = true};
            
            


            //printer("Параметер"); //синх
            //printer.BeginInvoke("Параметер асинх", result =>
            //{
            //    Console.WriteLine("Завершено!");
            //}, null);
            //Func<long, long> get_factorial = Factorial;
            //get_factorial.BeginInvoke(30, result =>
            //{
            //    var y = get_factorial.EndInvoke(result);
            //    Console.WriteLine("Результат: {0}", y);
            //}, null);
            //BackgroundWorker worker = new();
            //worker.DoWork += (s, e) =>
            //{
            //    if (e.Cancel == true) 
            //        return;
            //    var w = (BackgroundWorker)s;
            //    //w.ReportProgress(100);
            //    e.Result = Factorial((long)e.Argument);
            //};
            //worker.ProgressChanged += (_, e) => Console.WriteLine($"Прогресс: {e.ProgressPercentage}");
            //worker.RunWorkerCompleted += (_, e) => Console.WriteLine($"Завершена операция, результат: {e.Result}");
            //worker.RunWorkerAsync(10l);

            var task_factorial = Task.Run(() => Factorial(10)); //сам правильный вариант


            Action<string> _printer = str =>
            {
                Console.WriteLine($"1 Поток: {Thread.CurrentThread.ManagedThreadId} - {str}");
                Thread.Sleep(3000);
                Console.WriteLine($"1 Завершение в {Thread.CurrentThread.ManagedThreadId}");
            };
            var print_task = new Task(() => _printer("печать"));
            print_task.Start();
            print_task.Wait(); //неправильно



            Console.WriteLine("Приложение завершило свою работу");
            Console.ReadKey();


        }

        private static long Factorial(long x)
        {
            Console.WriteLine($"2 Факториал в потоке {Thread.CurrentThread.ManagedThreadId}");
            var result = 1l;
            while (x > 1)
            {
                result *= x;
                x--;
                Thread.Sleep(100);
            }
            Console.WriteLine($"2 Завершение вычисления в потоке {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}
