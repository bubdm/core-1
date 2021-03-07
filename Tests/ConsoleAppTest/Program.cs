using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока

            //Test_TPL.TestOldVariantTask();
            //Test_TPL.TestTPLFunctions();



            var task = Tasks.TestGoodAsync();
            Console.WriteLine("Задания когда уже начали или не начали выполняться асинхронно!?!?!?");
            await task;

            var val = 0;
            while (!task.IsCompleted)
            {
                val++;
                Console.Title = $"Задания выполняется {val} секунд";
                Thread.Sleep(1000);
            }
            if (task.IsCompletedSuccessfully)
                Console.Title = "Задача выполнена успешно";
            if (task.IsFaulted)
                Console.Title = "Ошибка в выполнении задачи";
            if (task.IsCanceled)
                Console.Title = "Задача отменена";


            Console.WriteLine("Главный поток завершил работу свою");
            Console.ReadKey();
        }
    }
}
