using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    static class Test_TPL
    {
        public static async void TestAsync()
        {

        }


        public static void TestOldVariantTask()
        {
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
            BackgroundWorker worker = new();
            worker.DoWork += (s, e) =>
            {
                if (e.Cancel == true)
                    return;
                var w = (BackgroundWorker) s;
                //w.ReportProgress(100);
                e.Result = Factorial((long) e.Argument);
            };
            worker.ProgressChanged += (_, e) => Console.WriteLine($"Прогресс: {e.ProgressPercentage}");
            worker.RunWorkerCompleted += (_, e) => Console.WriteLine($"Завершена операция, результат: {e.Result}");
            worker.RunWorkerAsync(10l);

            var taskFactorial = Task.Run(() => Factorial(20)); //сам правильный вариант
            Console.WriteLine("3 Ожидание результата расчета:");
            Console.WriteLine("3 результат: " + taskFactorial.Result);

            Action<string> _printer = str =>
            {
                Console.WriteLine($"1 Поток: {Thread.CurrentThread.ManagedThreadId} - {str}");
                Thread.Sleep(3000);
                Console.WriteLine($"1 Завершение в {Thread.CurrentThread.ManagedThreadId}");
            };
            var print_task = new Task(() => _printer("печать"));
            print_task.Start();
            print_task.Wait(); //неправильно
            ////правильно: async/await - магия на уровне компилятора
        }

        public static void TestTPLFunctions()
        {
            var messages = Enumerable.Range(1, 100).Select(i => $"Сообщение № {i}");

            var cancellation = new CancellationTokenSource();
            cancellation.Cancel(); //отмена операции параллельной
            var cancel = cancellation.Token;
            var messages7 = messages
                .AsParallel() //параллельно выполнять
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism) //принудительный параллелизм
                .WithMergeOptions(ParallelMergeOptions.AutoBuffered) //способ соединения
                .WithCancellation(cancel) //с отменой операции
                .Select(Transform)
                .Where(m => m.EndsWith("7"))
                .Count();
            Console.WriteLine($"Сообщений: {messages7}");


            Console.WriteLine("Нажать кнопку");
            Console.ReadKey();

            Parallel.ForEach(messages, PrintMes);
            Parallel.Invoke(() => PrintThread(-1), () => PrintThread(-2), () => PrintThread(-3), () => PrintThread(-4));
            Console.WriteLine("Цикл с отменой:");
            Parallel.For(0, 100, (i, s) =>
            {
                if (i > 20)
                    s.Break();
                PrintThread(i);
            });
            Console.WriteLine("Цикл еще один:");
            Parallel.For(0, 100, new ParallelOptions
            {
                MaxDegreeOfParallelism = 4,
            }, PrintThread);


        }
        
        private static string Transform(string message)
        {
            Thread.Sleep(100);
            return $"Обработанное {message}";
        }

        private static void PrintMes(string message)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} запущен - {message}");
            Thread.Sleep(100);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} остановлен - {message}");
        }

        private static void PrintThread(int i)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} запущен - {i}");
            Thread.Sleep(100);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} остановился - {i}");
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