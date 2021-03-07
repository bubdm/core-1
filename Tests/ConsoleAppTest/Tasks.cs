using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    static class Tasks
    {
        public static void TestTasks()
        {
            var value30Task = Task.Run(() => Sum(30));
            var value10Task = Task.Run(() => Sum(10));
            var x = 90;
            var valueXTask = Task.Factory.StartNew(s => Sum((long)s), x);
            var value1000Task = Task.Factory.StartNew(() => Sum(1000), TaskCreationOptions.LongRunning);

            //Task.WaitAll(value10Task, value30Task, valueXTask, value1000Task); //плохой способ
            //var result = value30Task.Result; //плохой способ
            //Task.WaitAny(value10Task, value30Task); //тоже плохой способ
        }

        public static async void TestBadAsync()
        {
            try
            {
                var value30Task = Task.Run(() => Sum(30));
                var value30 = await value30Task;
                //var value30 = await value30Task.ConfigureAwait(false); //вернуться в произвольный поток
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static async Task TestGoodAsync()
        {
            Console.WriteLine($"ВНИМАНИЕ! Поток: {Thread.CurrentThread.ManagedThreadId} Асинхронная задача начала выполняться");
            var messages = Enumerable.Range(1, 10).Select(i => $"Сообщение № {i}");

            Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} Последовательно - асинхронно:");
            foreach (var m in messages)
            {
                var result2 = await Task.Run(() => MessageTransform(m)).ConfigureAwait(false);
                Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} знач: " + result2);
            }
        }

        private static string MessageTransform(string message)
        {
            Thread.Sleep(100);
            return $"Обработанное: {message}";
        }

        private static long Factorial(long x)
        {
            var result = 1L;
            while (x > 1)
            {
                result *= x;
                x--;
                Thread.Sleep(100);
            }
            return result;
        }
        private static long Sum(long x)
        {
            if (x < 0) return Sum(-x);

            var result = 0L;
            while (x > 0)
            {
                result += x;
                x--;
                Thread.Sleep(100);
            }
            return result;
        }


    }
}
